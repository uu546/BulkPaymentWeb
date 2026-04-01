using BulkPaymentWeb.Application.Interfaces.Services.Payment;
using BulkPaymentWeb.Application.Interfaces.Validator;
using BulkPaymentWeb.Application.Services.Payment;
using BulkPaymentWeb.Application.Validator;
using Microsoft.Extensions.DependencyInjection;

namespace BulkPaymentWeb.Application
{
    /// <summary>
    /// Класс для настройки зависимостей слоя Application.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Метод регистрирует сервисы слоя Application.
        /// </summary>
        /// <param name="services">Регистрация зависимостей.</param>
        /// <returns>Коллекция зарегистрированных сервисов.</returns>
        public static IServiceCollection AddApplicationInit(this IServiceCollection services)
        {
            ServicesInit(services);

            return services;
        }

        /// <summary>
        /// Метод регистрирует сервисы.
        /// </summary>
        /// <param name="services">Регистрация зависимостей.</param>
        private static void ServicesInit(IServiceCollection services)
        {
            services.AddScoped<IPaymentValidator, PaymentValidator>();
            services.AddScoped<IPaymentService, PaymentService>();
        }
    }
}