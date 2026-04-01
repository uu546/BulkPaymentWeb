using BulkPaymentWeb.Application.Dto.Output.Payment;
using BulkPaymentWeb.Application.Interfaces.Repositories.Payment;
using BulkPaymentWeb.Application.Interfaces.Services.Payment;
using Microsoft.Extensions.Logging;

namespace BulkPaymentWeb.Application.Services.Payment
{
    /// <summary>
    /// Класс реализует сервис для платежей.   
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentService> _logger;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="paymentRepository">Репозиторий платежей</param>
        public PaymentService(IPaymentRepository paymentRepository,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PaymentOutput>> GetPaymentsByRegistryIdAsync(int registryId)
        {
            try
            {
                IEnumerable<PaymentOutput> result = await _paymentRepository.GetPaymentsByRegistryIdAsync(registryId);

                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}