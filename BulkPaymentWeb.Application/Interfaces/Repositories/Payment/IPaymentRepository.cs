
using BulkPaymentWeb.Application.Dto.Output.Payment;

namespace BulkPaymentWeb.Application.Interfaces.Repositories.Payment
{
    /// <summary>
    /// Интерфейс репозитория платежей.
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// Метод получает список платажей по Id реестра.
        /// </summary>
        /// <param name="registryId">Id реестра.</param>
        /// <returns>Список платежей.</returns>
        Task<IEnumerable<PaymentOutput>> GetPaymentsByRegistryIdAsync(int registryId);
    }
}
