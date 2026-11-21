using System;
using System.Collections.Generic;

namespace SMT.ViewModel.Dto.DocumentDto
{
    public class DocumentEntry
    {
        public string Name { get; set; }

        public bool IsFolder { get; set; }

        /// <summary>Bytes; 0 for folders.</summary>
        public long Size { get; set; }

        public DateTime Modified { get; set; }
    }

    public class DocumentListing
    {
        public string Owner { get; set; }

        /// <summary>Folder path inside the owner's folder, "/"-separated, "" for the top.</summary>
        public string Path { get; set; }

        /// <summary>Whether the current user may upload/rename/delete here. Filled in by the API.</summary>
        public bool CanEdit { get; set; }

        public List<DocumentEntry> Entries { get; set; } = new List<DocumentEntry>();
    }

    public class DocumentFolderOwner
    {
        public string Username { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class DocumentCreateFolder
    {
        public string Owner { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }
    }

    public class DocumentRename
    {
        public string Owner { get; set; }

        /// <summary>Path of the file/folder being renamed.</summary>
        public string Path { get; set; }

        public string NewName { get; set; }
    }

    public class DocumentStaffCreate
    {
        public string Username { get; set; }

        public string Password { get; set; }

        /// <summary>Admins can manage every folder and the staff accounts.</summary>
        public bool IsAdmin { get; set; }
    }

    public class DocumentPasswordReset
    {
        public string Password { get; set; }
    }
}
