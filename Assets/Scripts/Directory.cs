using System.Collections.Generic;

public class Directory
{
    public string Name { get; set; }
    public Dictionary<string, Directory> Subdirectories { get; set; }
    public Dictionary<string, Files> Files { get; set; }

    public Directory(string name)
    {
        Name = name;
        Subdirectories = new Dictionary<string, Directory>();
        Files = new Dictionary<string, Files>();
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
