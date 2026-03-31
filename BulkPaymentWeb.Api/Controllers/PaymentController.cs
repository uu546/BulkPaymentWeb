using BulkPaymentWeb.Infrastructure.Data;
using BulkPaymentWeb.Infrastructure.Interfaces.PaymentRegistry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Метод получает список платежей реестра.
        /// </summary>
        /// <param name="registryId">Id реестра.</param>
        /// <returns>Список платажей реестра.</returns>
        [HttpGet("registries/{registryId}/payments")]
        public async Task<IActionResult> GetPaymentsAsync([FromRoute] int registryId,
            [FromServices] ApplicationDbContext dbContext)
        {
            bool registryExists = await dbContext.Registries.AnyAsync(r => r.Id == registryId);

            if (!registryExists)
            {
                return NotFound("Реестр не найден.");
            }

            var payments = await dbContext.Payments
                .Where(p => p.RegistryId == registryId)
                .OrderBy(p => p.Id)
                .Select(p => new
                {
                    p.Id,
                    p.RegistryId,
                    p.PayerInn,
                    p.PayerAccount,
                    p.ReceiverInn,
                    p.ReceiverAccount,
                    p.ReceiverBik,
                    p.Amount,
                    p.Purpose,
                    p.IsValid,
                    p.ValidationError
                })
                .ToListAsync();

            return Ok(payments);
        }
    }
}
