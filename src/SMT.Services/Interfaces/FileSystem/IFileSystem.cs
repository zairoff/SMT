namespace SMT.Services.Interfaces.FileSystem
{
    public interface IFileSystem
    {
        void CreateFolder(string path);

        string Combine(string path, string combine);

        string GetFileExtension(string fileName);
    }
}
