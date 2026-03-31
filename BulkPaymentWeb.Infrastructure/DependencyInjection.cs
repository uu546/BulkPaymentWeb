using BulkPaymentWeb.Application.Interfaces.Services.ExcelParser;
using BulkPaymentWeb.Application.Interfaces.Services.FileStorageService;
using BulkPaymentWeb.Application.Services.PaymentRegistry;
using BulkPaymentWeb.Infrastructure.Data;
using BulkPaymentWeb.Infrastructure.Interfaces.PaymentRegistry;
using BulkPaymentWeb.Infrastructure.Jobs;
using BulkPaymentWeb.Infrastructure.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BulkPaymentWeb.Infrastructure
{
    /// <summary>
    /// Класс для настройки зависимостей слоя Infrastructure.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Метод регистрирует инфраструктурные сервисы.
        /// </summary>
        /// <param name="services">Регистрация зависимостей.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <returns>Коллекция зарегистрированных сервисов.</returns>
        public static IServiceCollection AddInfrastructureInit(this IServiceCollection services,
            IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            ServicesInit(services);
            HangfireInit(services, connectionString);
            JobInit(services);

            return services;
        }

        /// <summary>
        /// Метод регистрирует сервисы.
        /// </summary>
        /// <param name="services">Регистрация зависимостей.</param>
        private static void ServicesInit(IServiceCollection services)
        {
            services.AddScoped<IExcelParserService, ExcelParserService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IPaymentRegistryService, PaymentRegistryService>();
        }

        private static void JobInit(IServiceCollection services)
        {
            services.AddTransient<IPaymentBackgroundJob, PaymentBackgroundJob>();
        }

        /// <summary>
        /// Метод регистрирует Hangfire.
        /// </summary>
        /// <param name="services">Регистрация зависимостей.</param>
        private static void HangfireInit(IServiceCollection services, string connectionString)
        {
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options =>
            {
                options.UseNpgsqlConnection(connectionString);
            }));
            //.UseRedisStorage("localhost:6379"));

            services.AddHangfireServer();
        }
    }
}
