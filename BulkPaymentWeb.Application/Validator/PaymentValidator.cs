using BulkPaymentWeb.Application.Interfaces.Validator;
using BulkPaymentWeb.Domain.Entities;
using System.Text.RegularExpressions;

namespace BulkPaymentWeb.Application.Validator
{
    /// <summary>
    /// Класс реализует валидатор реквизитов платежей.
    /// </summary>
    public class PaymentValidator : IPaymentValidator
    {
        /// <inheritdoc />
        public void Validate(PaymentItemEntity payment)
        {
            List<string> errors = new List<string>();

            // 1. Проверка ИНН (10 цифр для ЮЛ, 12 для ИП).
            if (!Regex.IsMatch(payment.PayerInn, @"^(\d{10}|\d{12})$"))
                errors.Add("Некорректный ИНН плательщика.");

            if (!Regex.IsMatch(payment.ReceiverInn, @"^(\d{10}|\d{12})$"))
                errors.Add("Некорректный ИНН получателя.");

            // 2. Проверка расчетных счетов (строго 20 цифр)
            if (!Regex.IsMatch(payment.PayerAccount, @"^\d{20}$"))
                errors.Add("Счет плательщика должен состоять из 20 цифр.");

            if (!Regex.IsMatch(payment.ReceiverAccount, @"^\d{20}$"))
                errors.Add("Счет получателя должен состоять из 20 цифр.");

            // 3. Проверка БИК (строго 9 цифр, обычно начинается с 04)
            if (!Regex.IsMatch(payment.ReceiverBik, @"^\d{9}$"))
                errors.Add("БИК должен состоять из 9 цифр.");

            // 4. Проверка суммы
            if (payment.Amount <= 0)
                errors.Add("Сумма платежа должна быть больше нуля.");

            // 5. Назначение платежа
            if (string.IsNullOrWhiteSpace(payment.Purpose))
                errors.Add("Назначение платежа не может быть пустым.");

            if (errors.Any())
            {
                payment.IsValid = false;
                payment.ValidationError = string.Join(" | ", errors);

                return;
            }

            payment.IsValid = true;
            payment.ValidationError = null;
        }
    }
}
