namespace BulkPaymentWeb.Infrastructure.Interfaces.Jobs
{
    /// <summary>
    /// Интерфейс фонового обработчика реестра платежей.
    /// </summary>
    public interface IPaymentBackgroundJob
    {
        /// <summary>
        /// Метод выполняет чтение, валидацию и сохранение платежей из файла Excel.
        /// </summary>
        /// <param name="registryId">Id реестра в базе данных.</param>
        /// <param name="filePath">Путь к временно сохраненному файлу на диске.</param>
        Task ProcessRegistryAsync(int registryId, string filePath);
    }
}
