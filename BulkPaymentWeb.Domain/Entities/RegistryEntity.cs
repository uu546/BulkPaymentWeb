namespace BulkPaymentWeb.Domain.Entities
{
    /// <summary>
    /// Класс реестра платежей.
    /// </summary>
    public class RegistryEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя загружаемого файла реестра.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Дата и время загрузки.
        /// </summary>
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// Текущее состояние обработки реестра.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Список платежей, которые связанны с реестром.
        /// </summary>
        public List<PaymentItemEntity>? PaymentItemEntities { get; set; }
    }
}
