namespace BulkPaymentWeb.Application.Dto.Output.Payment
{
    /// <summary>
    /// Класс выходной модели платежа.
    /// </summary>
    public class PaymentOutput
    {
        /// PK.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id реестра.
        /// </summary>
        public int RegistryId { get; set; }

        /// <summary>
        /// ИНН организации, которая платит.
        /// </summary>
        public string? PayerInn { get; set; }

        /// <summary>
        /// Расчетный счет плательщика.
        /// </summary>
        public string? PayerAccount { get; set; }

        /// <summary>
        /// ИНН организации, которая получает деньги.
        /// </summary>
        public string? ReceiverInn { get; set; }

        /// <summary>
        /// Расчет счет получателя.
        /// </summary>
        public string? ReceiverAccount { get; set; }

        /// <summary>
        /// Банковский Идентификационный Код банка получателя.
        /// </summary>
        public string? ReceiverBik { get; set; }

        /// <summary>
        /// Сумма платежа.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Текстовое описание: за что переводятся деньги.
        /// </summary>
        public string? Purpose { get; set; }

        /// <summary>
        /// Признак: прошел ли платеж валидацию.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Текст ошибки.
        /// </summary>
        public string? ValidationError { get; set; }
    }
}
