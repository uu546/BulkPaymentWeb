using BulkPaymentWeb.Application.Interfaces.Services.FileStorageService;
using BulkPaymentWeb.Domain.Entities;
using BulkPaymentWeb.Infrastructure.Data;
using BulkPaymentWeb.Infrastructure.Interfaces.PaymentRegistry;
using BulkPaymentWeb.Infrastructure.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Http;
namespace BulkPaymentWeb.Application.Services.PaymentRegistry
{
    public class PaymentRegistryService : IPaymentRegistryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorageService _fileStorage;
        private readonly IBackgroundJobClient _jobClient;

        public PaymentRegistryService(ApplicationDbContext dbContext,
            IFileStorageService fileStorage,
            IBackgroundJobClient jobClient)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
            _jobClient = jobClient;
        }

        public async Task<int> CreateAndSendEnqueueAsync(IFormFile file)
        {
            PaymentRegistryEntity registry = new PaymentRegistryEntity
            {
                FileName = file.FileName,
                UploadedAt = DateTime.UtcNow,
                Status = "Created"
            };

            _dbContext.Registries.Add(registry);
            await _dbContext.SaveChangesAsync();

            string filePath = await _fileStorage.SaveTempFileAsync(file);

            // 3. Очередь
            _jobClient.Enqueue<IPaymentBackgroundJob>(job => job.ProcessRegistryAsync(registry.Id, filePath));

            return registry.Id;
        }
    }
}