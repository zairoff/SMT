using SMT.Services.Exceptions;
using SMT.Services.Interfaces.Documents;
using SMT.ViewModel.Dto.DocumentDto;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services.Documents
{
    public class DocumentStorageOptions
    {
        /// <summary>Absolute folder that holds one sub-folder per staff member. Back this folder up.</summary>
        public string RootFolder { get; set; }
    }

    public class DocumentStorage : IDocumentStorage
    {
        // Names Windows refuses (or treats as devices) regardless of extension.
        private static readonly string[] ReservedNames =
        {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
        };

        // Path.GetInvalidFileNameChars() differs per OS; use the Windows set everywhere so a
        // folder created under Docker/Linux can still be copied to a Windows backup.
        private static readonly char[] InvalidNameChars =
            "<>:\"/\\|?*".ToCharArray().Concat(Enumerable.Range(0, 32).Select(i => (char)i)).ToArray();

        // In-progress uploads; hidden from listings.
        private const string TempPrefix = ".upload-";

        private readonly string _root;

        public DocumentStorage(DocumentStorageOptions options)
        {
            if (string.IsNullOrWhiteSpace(options?.RootFolder))
                throw new ArgumentException("Documents root folder is not configured");

            _root = Path.GetFullPath(options.RootFolder);
        }

        public DocumentListing List(string owner, string path)
        {
            var segments = SplitPath(path);
            var folder = Resolve(owner, segments);
            var listing = new DocumentListing { Owner = owner, Path = string.Join("/", segments) };

            if (!Directory.Exists(folder))
            {
                // A staff member's own folder is created lazily on first upload; an empty top
                // level is normal. A missing sub-folder means a stale link.
                if (segments.Length > 0)
                    throw new NotFoundException("Folder not found");
                return listing;
            }

            var dir = new DirectoryInfo(folder);
            listing.Entries.AddRange(dir.GetDirectories()
                .OrderBy(d => d.Name, StringComparer.OrdinalIgnoreCase)
                .Select(d => new DocumentEntry { Name = d.Name, IsFolder = true, Modified = d.LastWriteTime }));
            listing.Entries.AddRange(dir.GetFiles()
                .Where(f => !f.Name.StartsWith(TempPrefix))
                .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
                .Select(ToEntry));

            return listing;
        }

        public async Task<DocumentEntry> SaveAsync(string owner, string path, string fileName, Stream content, bool overwrite)
        {
            // Browsers may send a full client path ("C:\fakepath\a.pdf"); keep just the name.
            fileName = Path.GetFileName((fileName ?? "").Replace('\\', '/'));
            ValidateName(fileName);

            var folder = Resolve(owner, SplitPath(path));
            var target = Path.Combine(folder, fileName);

            if (Directory.Exists(target))
                throw new ConflictException($"A folder named \"{fileName}\" already exists");
            if (File.Exists(target) && !overwrite)
                throw new ConflictException($"\"{fileName}\" already exists");

            Directory.CreateDirectory(folder);

            // Write to a temp file first so a dropped upload never leaves a half-written
            // file in place of a good one.
            var temp = Path.Combine(folder, $"{TempPrefix}{Guid.NewGuid():N}.tmp");
            try
            {
                using (var output = new FileStream(temp, FileMode.CreateNew, FileAccess.Write))
                    await content.CopyToAsync(output);

                File.Move(temp, target, overwrite: true);
            }
            finally
            {
                if (File.Exists(temp))
                    File.Delete(temp);
            }

            return ToEntry(new FileInfo(target));
        }

        public Stream OpenRead(string owner, string path, out string fileName)
        {
            var segments = SplitPath(path);
            if (segments.Length == 0)
                throw new NotFoundException("File not found");

            var file = Resolve(owner, segments);
            if (!File.Exists(file))
                throw new NotFoundException("File not found");

            fileName = Path.GetFileName(file);
            return new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public DocumentEntry CreateFolder(string owner, string path, string name)
        {
            ValidateName(name);
            var parent = Resolve(owner, SplitPath(path));
            var target = Path.Combine(parent, name);

            if (Directory.Exists(target) || File.Exists(target))
                throw new ConflictException($"\"{name}\" already exists");

            var dir = Directory.CreateDirectory(target);
            return new DocumentEntry { Name = dir.Name, IsFolder = true, Modified = dir.LastWriteTime };
        }

        public DocumentEntry Rename(string owner, string path, string newName)
        {
            ValidateName(newName);
            var segments = SplitPath(path);
            if (segments.Length == 0)
                throw new InvalidOperationException("The top folder cannot be renamed");

            var source = Resolve(owner, segments);
            var target = Path.Combine(Path.GetDirectoryName(source), newName);
            var caseOnly = string.Equals(source, target, StringComparison.OrdinalIgnoreCase);

            if (!caseOnly && (Directory.Exists(target) || File.Exists(target)))
                throw new ConflictException($"\"{newName}\" already exists");

            if (Directory.Exists(source))
            {
                if (caseOnly)
                {
                    // Windows can't rename "a" to "A" directly.
                    var step = source + ".rename-" + Guid.NewGuid().ToString("N");
                    Directory.Move(source, step);
                    source = step;
                }
                Directory.Move(source, target);
                var dir = new DirectoryInfo(target);
                return new DocumentEntry { Name = dir.Name, IsFolder = true, Modified = dir.LastWriteTime };
            }

            if (File.Exists(source))
            {
                File.Move(source, target, overwrite: caseOnly);
                return ToEntry(new FileInfo(target));
            }

            throw new NotFoundException("File or folder not found");
        }

        public void Delete(string owner, string path)
        {
            var segments = SplitPath(path);
            if (segments.Length == 0)
                throw new InvalidOperationException("The top folder cannot be deleted");

            var target = Resolve(owner, segments);
            if (Directory.Exists(target))
                Directory.Delete(target, recursive: true);
            else if (File.Exists(target))
                File.Delete(target);
            else
                throw new NotFoundException("File or folder not found");
        }

        public void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("Name is required");
            if (name.Length > 200)
                throw new InvalidOperationException("Name is too long (200 characters max)");
            if (name == "." || name == "..")
                throw new InvalidOperationException("Invalid name");
            if (name.IndexOfAny(InvalidNameChars) >= 0)
                throw new InvalidOperationException("Name cannot contain any of: < > : \" / \\ | ? *");
            if (name != name.Trim() || name.EndsWith("."))
                throw new InvalidOperationException("Name cannot start or end with a space, or end with a dot");

            var stem = name.Split('.')[0].ToUpperInvariant();
            if (ReservedNames.Contains(stem))
                throw new InvalidOperationException($"\"{name}\" is a reserved name on Windows");
        }

        // "a/b/c" (either slash) -> ["a","b","c"], each segment validated as a plain name so
        // nothing like ".." or an absolute path can get past this point.
        private string[] SplitPath(string path)
        {
            var segments = (path ?? "")
                .Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var segment in segments)
                ValidateName(segment);
            return segments;
        }

        private string Resolve(string owner, string[] segments)
        {
            ValidateName(owner);
            var ownerRoot = Path.GetFullPath(Path.Combine(_root, owner));
            var full = Path.GetFullPath(Path.Combine(new[] { ownerRoot }.Concat(segments).ToArray()));

            // Defence in depth: segments are already validated, but never act outside the owner's folder.
            if (!full.Equals(ownerRoot, StringComparison.OrdinalIgnoreCase) &&
                !full.StartsWith(ownerRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Invalid path");

            return full;
        }

        private static DocumentEntry ToEntry(FileInfo file)
        {
            return new DocumentEntry { Name = file.Name, IsFolder = false, Size = file.Length, Modified = file.LastWriteTime };
        }
    }
}
