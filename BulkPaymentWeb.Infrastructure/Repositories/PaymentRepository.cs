using BulkPaymentWeb.Application.Dto.Output.Payment;
using BulkPaymentWeb.Application.Interfaces.Repositories.Payment;
using BulkPaymentWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BulkPaymentWeb.Infrastructure.Repositories
{
    /// <summary>
    /// Класс реализует репозиторий платежей.
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dbContext">Класс контекста.</param>
        public PaymentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PaymentOutput>> GetPaymentsByRegistryIdAsync(int registryId)
        {
            bool registryExists = await _dbContext.Registries.AnyAsync(r => r.Id == registryId);

            if (!registryExists)
            {
                throw new InvalidOperationException("Реестр не найден." +
                                                    $"RegistryId: {registryId}.");
            }

            IEnumerable<PaymentOutput> result = await _dbContext.Payments
                .AsNoTracking()
                .Where(p => p.RegistryId == registryId)
                .OrderBy(p => p.Id)
                .Select(p => new PaymentOutput
                {
                    Id = p.Id,
                    RegistryId = p.RegistryId,
                    PayerInn = p.PayerInn,
                    PayerAccount = p.PayerAccount,
                    ReceiverInn = p.ReceiverInn,
                    ReceiverAccount = p.ReceiverAccount,
                    ReceiverBik = p.ReceiverBik,
                    Amount = p.Amount,
                    Purpose = p.Purpose,
                    IsValid = p.IsValid,
                    ValidationError = p.ValidationError
                }).ToListAsync();

            return result;
        }
    }
}
