using BulkPaymentWeb.Application.Dto.Output.Progress;
using BulkPaymentWeb.Application.Interfaces.Services.ExcelParser;
using BulkPaymentWeb.Application.Interfaces.Validator;
using BulkPaymentWeb.Domain.Entities;
using BulkPaymentWeb.Infrastructure.Data;
using BulkPaymentWeb.Infrastructure.Data.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BulkPaymentWeb.Infrastructure.Jobs
{
    /// <summary>
    /// Класс реализует фонового обработчика реестра платежей.
    /// </summary>
    public class PaymentBackgroundJob : IPaymentBackgroundJob
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<PaymentHub> _paymentHub;
        private readonly ILogger<PaymentBackgroundJob> _logger;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="scopeFactory">Создание скоупа.</param>
        /// <param name="paymentHub">Хаб платежей.</param>
        public PaymentBackgroundJob(IServiceScopeFactory scopeFactory,
            IHubContext<PaymentHub> paymentHub,
            ILogger<PaymentBackgroundJob> logger)
        {
            _scopeFactory = scopeFactory;
            _paymentHub = paymentHub;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task ProcessRegistryAsync(int registryId, string filePath)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            IExcelParserService parser = scope.ServiceProvider.GetRequiredService<IExcelParserService>();
            IPaymentValidator validator = scope.ServiceProvider.GetRequiredService<IPaymentValidator>();

            try
            {
                // 1. Обновляем статус в БД.
                PaymentRegistryEntity? registry = await dbContext.Registries.FindAsync(registryId);

                if (registry == null) return;

                registry.Status = "Processing";
                await dbContext.SaveChangesAsync();

                // 2. Парсим файл.
                List<PaymentItemEntity> items;
                using (FileStream stream = File.OpenRead(filePath))
                {
                    items = parser.ParseStream(stream, registryId);
                }

                int total = items.Count;
                int processed = 0;

                // 3. Валидируем и сохраняем порциями (Batching).
                foreach (var item in items)
                {
                    validator.Validate(item);
                    dbContext.Payments.Add(item);

                    processed++;

                    // Отправляем прогресс каждые 5 строк.
                    if (processed % 2 == 0 || processed == total)
                    {
                        await Task.Delay(1500);
                        int percent = (int)((double)processed / total * 100);

                        Console.WriteLine($"Отправка в группу {registryId}. Прогресс: {percent}%");

                        await _paymentHub.Clients.Group(registryId.ToString()).SendAsync("UpdateProgress",
                            new ProcessingProgressOutput
                            {
                                RegistryId = registryId,
                                Percent = percent,
                                ProcessedRows = processed,
                                TotalRows = total,
                                Status = "Processing",
                                ErrorCount = items.Take(processed).Count(x => !x.IsValid)
                            });
                    }
                }

                // 4. Финализация.
                registry.Status = "Completed";
                await dbContext.SaveChangesAsync();

                // Уведомляем о завершении
                await _paymentHub.Clients.Group(registryId.ToString()).SendAsync("UpdateProgress",
                    new ProcessingProgressOutput
                    {
                        RegistryId = registryId,
                        Percent = 100,
                        ProcessedRows = total,
                        TotalRows = total,
                        Status = "Completed"
                    });
            }

            catch (Exception ex)
            {
                // Обработка критической ошибки
                PaymentRegistryEntity? registry = await dbContext.Registries.FindAsync(registryId);
                if (registry != null)
                {
                    registry.Status = "Failed";
                    await dbContext.SaveChangesAsync();
                }

                await _paymentHub.Clients.Group(registryId.ToString()).SendAsync("ErrorMessage", ex.Message);

                _logger.LogError(ex, ex.Message);
                throw;
            }

            finally
            {
                // Чистим временный файл после обработки
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }
    }
}
