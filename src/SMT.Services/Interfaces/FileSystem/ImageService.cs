using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.FileSystem
{
    public class ImageService : IImageService
    {
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IFileSystem _fileSystem;

        public ImageService(IHostingEnvironment environment, IFileSystem fileSystem, IConfiguration configuration)
        {
            _environment = environment;
            _fileSystem = fileSystem;
            _configuration = configuration;
        }

        public string LoadUrl(string fileName)
        {
            return $"{_configuration["AppSettings:EmployeeImagesPathString"]}/{fileName}";
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            if (file == null)
                throw new NullReferenceException();

            if (file.Length < 1)
                throw new InvalidDataException();

            var folder = _fileSystem.Combine(_environment.WebRootPath, _configuration["AppSettings:EmployeeImagesFolder"]);
            _fileSystem.CreateFolder(folder);
            var fileExtension = _fileSystem.GetFileExtension(file.FileName);
            var fileName = GenerateUniqueFileName(fileExtension);
            var filePath = _fileSystem.Combine($"{folder}\\", fileName);
            using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);
            return fileName;
        }

        private static string GenerateUniqueFileName(string fileExtension)
        {
            return $"{Guid.NewGuid()}{fileExtension}";
        }
    }
}
