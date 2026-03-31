using Microsoft.AspNetCore.Http;

namespace BulkPaymentWeb.Infrastructure.Interfaces.PaymentRegistry
{
    public interface IPaymentRegistryService
    {
        /// <summary>
        /// Метод создает запись в БД и ставит задачу в очередь.
        /// </summary>
        Task<int> CreateAndSendEnqueueAsync(IFormFile file);
    }
}
