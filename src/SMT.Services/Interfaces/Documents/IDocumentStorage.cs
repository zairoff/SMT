using SMT.ViewModel.Dto.DocumentDto;
using System.IO;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.Documents
{
    /// <summary>
    /// Staff document folders, stored as plain folders on disk: {root}/{owner}/{path...}.
    /// Permission checks (who may write where) are the caller's job; this class only
    /// guarantees that every path stays inside the owner's folder.
    /// </summary>
    public interface IDocumentStorage
    {
        DocumentListing List(string owner, string path);

        /// <summary>Saves the stream as {path}/{fileName}. Throws ConflictException when it exists and overwrite is false.</summary>
        Task<DocumentEntry> SaveAsync(string owner, string path, string fileName, Stream content, bool overwrite);

        /// <summary>Opens a file for reading. Throws NotFoundException when it does not exist.</summary>
        Stream OpenRead(string owner, string path, out string fileName);

        DocumentEntry CreateFolder(string owner, string path, string name);

        /// <summary>Renames a file or folder in place (same parent folder).</summary>
        DocumentEntry Rename(string owner, string path, string newName);

        /// <summary>Deletes a file, or a folder with everything in it. The owner's root folder cannot be deleted.</summary>
        void Delete(string owner, string path);

        /// <summary>Throws InvalidOperationException when the name cannot be used as a folder/file name.</summary>
        void ValidateName(string name);
    }
}
