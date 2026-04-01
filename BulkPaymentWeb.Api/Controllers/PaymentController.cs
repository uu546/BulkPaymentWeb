using BulkPaymentWeb.Infrastructure.Interfaces.PaymentRegistry;
using Microsoft.AspNetCore.Mvc;

namespace BulkPaymentWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRegistryService _paymentRegistryService;

        public PaymentController(IPaymentRegistryService paymentRegistryService)
        {
            _paymentRegistryService = paymentRegistryService;
        }

        /// <summary>
        /// Метод для массовой загрузки реестра платежей.
        /// </summary>
        /// <param name="file">Файл реестра (Excel).</param>
        /// <returns>ID созданного реестра для отслеживания статуса.</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadRegistryAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл не выбран или пуст.");
            }

            string extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".xls" && extension != ".xlsx")
            {
                return BadRequest("Поддерживаются только форматы Excel (.xls, .xlsx).");
            }

            int registryId = await _paymentRegistryService.CreateAndSendEnqueueAsync(file);

            return Accepted(new
            {
                RegistryId = registryId,
                Message = "Файл принят в обработку."
            });
        }
    }
}
