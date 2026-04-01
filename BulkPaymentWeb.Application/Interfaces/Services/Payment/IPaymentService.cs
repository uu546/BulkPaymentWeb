
using BulkPaymentWeb.Application.Dto.Output.Payment;

namespace BulkPaymentWeb.Application.Interfaces.Services.Payment
{
    /// <summary>
    /// Интерфейс сервиса для платежей.   
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Метод получает список платажей по Id реестра.
        /// </summary>
        /// <param name="registryId">Id реестра.</param>
        /// <returns>Список платежей.</returns>
        Task<IEnumerable<PaymentOutput>> GetPaymentsByRegistryIdAsync(int registryId);
    }
}
