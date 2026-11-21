using SMT.Services.Documents;
using SMT.Services.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SMT.Access.Test.Documents
{
    public class DocumentStorageTests : IDisposable
    {
        private readonly string _root;
        private readonly DocumentStorage _storage;

        public DocumentStorageTests()
        {
            _root = Path.Combine(Path.GetTempPath(), "smt-docs-test-" + Guid.NewGuid().ToString("N"));
            _storage = new DocumentStorage(new DocumentStorageOptions { RootFolder = _root });
        }

        public void Dispose()
        {
            if (Directory.Exists(_root))
                Directory.Delete(_root, recursive: true);
        }

        private static Stream Text(string s) => new MemoryStream(Encoding.UTF8.GetBytes(s));

        [Fact]
        public void List_EmptyOwnerFolder_ReturnsNoEntries()
        {
            var listing = _storage.List("ali", "");

            Assert.Equal("ali", listing.Owner);
            Assert.Equal("", listing.Path);
            Assert.Empty(listing.Entries);
        }

        [Fact]
        public async Task Save_List_Download_RoundTrip()
        {
            _storage.CreateFolder("ali", "", "BOM");
            await _storage.SaveAsync("ali", "BOM", "model-a.csv", Text("ref,qty"), overwrite: false);

            var top = _storage.List("ali", "");
            var folder = Assert.Single(top.Entries);
            Assert.True(folder.IsFolder);
            Assert.Equal("BOM", folder.Name);

            var inner = _storage.List("ali", "BOM");
            var file = Assert.Single(inner.Entries);
            Assert.Equal("model-a.csv", file.Name);
            Assert.Equal(7, file.Size);

            using var stream = _storage.OpenRead("ali", "BOM/model-a.csv", out var name);
            Assert.Equal("model-a.csv", name);
            Assert.Equal("ref,qty", new StreamReader(stream).ReadToEnd());
        }

        [Fact]
        public async Task Save_ExistingFile_ConflictsUnlessOverwrite()
        {
            await _storage.SaveAsync("ali", "", "a.txt", Text("one"), overwrite: false);

            await Assert.ThrowsAsync<ConflictException>(() => _storage.SaveAsync("ali", "", "a.txt", Text("two"), overwrite: false));

            await _storage.SaveAsync("ali", "", "a.txt", Text("three"), overwrite: true);
            using var stream = _storage.OpenRead("ali", "a.txt", out _);
            Assert.Equal("three", new StreamReader(stream).ReadToEnd());
        }

        [Fact]
        public async Task Save_StripsClientPathFromFileName()
        {
            var entry = await _storage.SaveAsync("ali", "", @"C:\fakepath\gerber.zip", Text("x"), overwrite: false);

            Assert.Equal("gerber.zip", entry.Name);
            Assert.True(File.Exists(Path.Combine(_root, "ali", "gerber.zip")));
        }

        [Theory]
        [InlineData("..")]
        [InlineData("../bob")]
        [InlineData("a/../../bob")]
        [InlineData(@"..\bob")]
        [InlineData("C:/Windows")]
        [InlineData("a/./b")]
        public void PathsOutsideTheOwnerFolder_AreRejected(string path)
        {
            Assert.Throws<InvalidOperationException>(() => _storage.List("ali", path));
            Assert.Throws<InvalidOperationException>(() => _storage.Delete("ali", path));
        }

        [Theory]
        [InlineData("..")]
        [InlineData("../x")]
        [InlineData("a:b")]
        [InlineData("CON")]
        [InlineData("nul.txt")]
        [InlineData("trailing.")]
        [InlineData(" lead")]
        [InlineData("")]
        public void InvalidNames_AreRejected(string name)
        {
            Assert.Throws<InvalidOperationException>(() => _storage.CreateFolder("ali", "", name));
        }

        [Fact]
        public void InvalidOwner_IsRejected()
        {
            Assert.Throws<InvalidOperationException>(() => _storage.List("..", ""));
            Assert.Throws<InvalidOperationException>(() => _storage.List("a/b", ""));
        }

        [Fact]
        public async Task Rename_FileAndFolder()
        {
            _storage.CreateFolder("ali", "", "old");
            await _storage.SaveAsync("ali", "old", "x.pdf", Text("x"), overwrite: false);

            _storage.Rename("ali", "old/x.pdf", "y.pdf");
            _storage.Rename("ali", "old", "new");

            Assert.Equal("y.pdf", Assert.Single(_storage.List("ali", "new").Entries).Name);
            Assert.Throws<NotFoundException>(() => _storage.List("ali", "old"));
        }

        [Fact]
        public void Rename_CaseOnly_Works()
        {
            _storage.CreateFolder("ali", "", "bom");

            _storage.Rename("ali", "bom", "BOM");

            Assert.Equal("BOM", Assert.Single(_storage.List("ali", "").Entries).Name);
        }

        [Fact]
        public async Task Rename_OntoExistingName_Conflicts()
        {
            await _storage.SaveAsync("ali", "", "a.txt", Text("a"), overwrite: false);
            await _storage.SaveAsync("ali", "", "b.txt", Text("b"), overwrite: false);

            Assert.Throws<ConflictException>(() => _storage.Rename("ali", "a.txt", "b.txt"));
        }

        [Fact]
        public async Task Delete_FolderRemovesContents_TopFolderIsProtected()
        {
            _storage.CreateFolder("ali", "", "tmp");
            await _storage.SaveAsync("ali", "tmp", "a.txt", Text("a"), overwrite: false);

            _storage.Delete("ali", "tmp");

            Assert.Empty(_storage.List("ali", "").Entries);
            Assert.Throws<InvalidOperationException>(() => _storage.Delete("ali", ""));
            Assert.Throws<NotFoundException>(() => _storage.Delete("ali", "tmp"));
        }

        [Fact]
        public async Task Owners_AreIsolated()
        {
            await _storage.SaveAsync("ali", "", "mine.txt", Text("a"), overwrite: false);

            Assert.Empty(_storage.List("bob", "").Entries);
            Assert.Throws<NotFoundException>(() => _storage.OpenRead("bob", "mine.txt", out _));
            Assert.Single(_storage.List("ali", "").Entries.Where(e => e.Name == "mine.txt"));
        }
    }
}
