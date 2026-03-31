namespace BulkPaymentWeb.Application.Dto.Output.Progress
{
    /// <summary>
    /// Класс выходной модели прогресса обработки реестра.
    /// </summary>
    public class ProcessingProgressOutput
    {
        /// <summary>
        /// Id реестра.
        /// </summary>
        public int RegistryId { get; set; }

        /// <summary>
        /// Процент выполнения (0-100).
        /// </summary>
        public int Percent { get; set; }

        /// <summary>
        /// Количество уже обработанных строк.
        /// </summary>
        public int ProcessedRows { get; set; }

        /// <summary>
        /// Общее количество строк в файле.
        /// </summary>
        public int TotalRows { get; set; }

        /// <summary>
        /// Количество строк, не прошедших валидацию.
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Текстовый статус (например, "Парсинг", "Валидация", "Завершено").
        /// </summary>
        public string? Status { get; set; }
    }
}
