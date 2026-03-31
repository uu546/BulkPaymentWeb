using Microsoft.AspNetCore.SignalR;

namespace BulkPaymentWeb.Infrastructure.Data.Hubs
{
    /// <summary>
    /// Класс хаба для передачи обновлений о статусе обработки платежных реестров.
    /// </summary>
    public class PaymentHub : Hub
    {
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
