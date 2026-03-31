using BulkPaymentWeb.Domain.Entities;

namespace BulkPaymentWeb.Application.Interfaces.Validator
{
    /// <summary>
    /// Интерфейс валидатора реквизитов платежей.
    /// </summary>
    public interface IPaymentValidator
    {
        /// <summary>
        /// Метод выполняет проверку реквизитов платежа на соответствие банковским стандартам.
        /// </summary>
        /// <param name="payment">Объект платежа для проверки.</param>
        void Validate(PaymentItemEntity payment);
    }
}
