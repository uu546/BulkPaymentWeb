using BulkPaymentWeb.Application.Interfaces.Services.FileStorageService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BulkPaymentWeb.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;

        public LocalFileStorageService(IWebHostEnvironment env) => _env = env;

        public async Task<string> SaveTempFileAsync(IFormFile file)
        {
            string uploadsFolder = Path.Combine(_env.ContentRootPath, "TempUploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");
            using FileStream stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return filePath;
        }

        public void DeleteFile(string filePath) => File.Delete(filePath);
    }
}
