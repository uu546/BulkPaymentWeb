using BulkPaymentWeb.Domain.Entities;

namespace BulkPaymentWeb.Application.Interfaces.Services.ExcelParser
{
    /// <summary>
    /// Интерфейс сервиса парсинга Excel файлов реестра.
    /// </summary>
    public interface IExcelParserService
    {
        /// <summary>
        /// Метод преобразует поток байтов Excel-файла в список сущностей платежей.
        /// </summary>
        /// <param name="stream">Входящий поток данных файла.</param>
        /// <param name="registryId">Идентификатор реестра, к которому будут привязаны платежи.</param>
        /// <returns>Список извлеченных платежей из файла.</returns>
        List<PaymentItemEntity> ParseStream(Stream stream, int registryId);
    }
}
