using System.Collections.Generic;

namespace Terminal.DirectoryAndFiles
{
    public class Directory
    {
        public string Name { get; set; }
        public Dictionary<string, Directory> Subdirectories { get; set; }
        public Dictionary<string, Files> Files { get; set; }
        public Directory ParentDirectory { get; set; }

        public Directory(string name, Directory parentDirectory = null)
        {
            Name = name;
            Subdirectories = new Dictionary<string, Directory>();
            Files = new Dictionary<string, Files>();
            ParentDirectory = parentDirectory;
        }
    }

    public class Files
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public Files(string name, string content = "")
        {
            Name = name;
            Content = content;
        }
    }
}
