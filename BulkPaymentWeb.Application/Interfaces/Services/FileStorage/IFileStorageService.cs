using Microsoft.AspNetCore.Http;

namespace BulkPaymentWeb.Application.Interfaces.Services.FileStorage
{
    /// <summary>
    /// Интерфейс сервиса хранения и удаления файлов.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Метод сохраняет файл и возвращает путь к нему.
        /// </summary>
        Task<string> SaveTempFileAsync(IFormFile file);

        /// <summary> 
        /// Метод удаляет файл.
        /// </summary>
        void DeleteFile(string filePath);
    }
}
