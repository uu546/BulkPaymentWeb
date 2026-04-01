using BulkPaymentWeb.Application.Dto.Output.Payment;
using BulkPaymentWeb.Application.Interfaces.Services.Payment;
using Microsoft.AspNetCore.SignalR;

namespace BulkPaymentWeb.Infrastructure.Data.Hubs
{
    /// <summary>
    /// Класс хаба для передачи обновлений о статусе обработки платежных реестров.
    /// </summary>
    public class PaymentHub : Hub
    {
        private readonly IPaymentService _paymentService;

        public PaymentHub(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<IEnumerable<PaymentOutput>> GetPaymentsByRegistryIdAsync(int registryId)
        {
            IEnumerable<PaymentOutput> payments = await _paymentService.GetPaymentsByRegistryIdAsync(registryId);

            return payments;
        }

        /// <summary>
        /// Метод подключает клиента к группе конкретного реестра. 
        /// </summary>
        /// <param name="registryId">Id реестра.</param>
        public async Task JoinRegistryGroupAsync(int registryId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, registryId.ToString());
        }
    }
}
