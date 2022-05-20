using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.FileSystem
{
    public class ImageService : IImageService
    {
        private readonly IHostingEnvironment _environment;
        private readonly IFileSystem _fileSystem;

        public ImageService(IHostingEnvironment environment, IFileSystem fileSystem)
        {
            _environment = environment;
            _fileSystem = fileSystem;
        }

        public string LoadUrl(string requestpath, string fileName)
        {
            return $"{requestpath}/{fileName}";
        }

        public async Task<string> SaveAsync(IFormFile file, string folderToSave)
        {
            if (file == null)
                throw new NullReferenceException();

            if (file.Length < 1)
                throw new InvalidDataException();

            var folder = _fileSystem.Combine(_environment.WebRootPath, folderToSave); 
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
