using System;
using System.IO;

namespace SMT.Services.Interfaces.FileSystem
{
    public class FileSystem : IFileSystem
    {
        public string Combine(string path, string combine)
        {
            return Path.Combine(path, combine);
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }
    }
}
