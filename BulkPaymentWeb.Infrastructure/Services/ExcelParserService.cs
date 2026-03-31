using BulkPaymentWeb.Application.Interfaces.Services.ExcelParser;
using BulkPaymentWeb.Domain.Entities;
using ExcelDataReader;
using System.Globalization;
using System.Text;

namespace BulkPaymentWeb.Infrastructure.Services
{
    /// <summary>
    /// Класс реализует сервис парсинга Excel файлов реестра.
    /// </summary>
    public class ExcelParserService : IExcelParserService
    {
        /// <inheritdoc />
        public List<PaymentItemEntity> ParseStream(Stream stream, int registryId)
        {
            List<PaymentItemEntity> payments = new List<PaymentItemEntity>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

            bool isHeader = true;

            while (reader.Read())
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                if (reader.GetValue(0) == null && reader.GetValue(1) == null)
                    break;

                PaymentItemEntity payment = new PaymentItemEntity
                {
                    RegistryId = registryId,
                    PayerInn = GetStringValue(reader, 0),
                    PayerAccount = GetStringValue(reader, 1),
                    ReceiverInn = GetStringValue(reader, 2),
                    ReceiverAccount = GetStringValue(reader, 3),
                    ReceiverBik = GetStringValue(reader, 4),
                    Purpose = GetStringValue(reader, 6),
                    IsValid = false
                };

                string amountStr = GetStringValue(reader, 5).Replace(',', '.');

                if (decimal.TryParse(amountStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
                {
                    payment.Amount = amount;
                }

                payments.Add(payment);
            }

            return payments;
        }

        /// <summary>
        /// Метод получает значение из ячейки Excel файла.
        /// </summary>
        /// <param name="reader">Объект чтения Excel.</param>
        /// <param name="columnIndex">Индекс колонки.</param>
        /// <returns>Значение определенной ячейки.</returns>
        private string GetStringValue(IExcelDataReader reader, int columnIndex)
        {
            return reader.GetValue(columnIndex).ToString()?.Trim() ?? string.Empty;
        }
    }
}
