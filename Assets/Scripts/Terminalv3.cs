using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Terminalv3 : MonoBehaviour
{
    // Serialized Fields
    [SerializeField] GameObject valueUserInput; // Input field for user input
    [SerializeField] GameObject valueUserOutput; // Output field for terminal output
    [SerializeField] GameObject valueUserError; // Error message display
    [SerializeField] TMP_InputField tmpInputField; // Input field component
    [SerializeField] Transform parentPanel; // Parent panel for UI elements
    [SerializeField] RectTransform targetRectTransform; // Target RectTransform for adjustments
    [SerializeField] RectTransform toAdjustRectTransform; // RectTransform to be adjusted
    [SerializeField] RectTransform DirName; // RectTransform for directory name display

    // Lists and Variables
    public List<string> inputArray; // Array to store user input
    public List<string> outputArray = new List<string>(); // Array to store terminal output
    public List<string> cmdList = new List<string>(); // List of available commands
    public bool Admin = false; // Flag indicating admin privileges

    [SerializeField] float minHeight = 2160f; // Minimum height for adjustments
    [SerializeField] int i = 1; // Index variable
    public new string name = "user"; // Username
    public int ListOfCommandsIndex; // Index for list of commands

    // Directories and Paths
    public Directory rootDirectory; // Root directory
    public Directory currentDirectory; // Current working directory
    public Directory previousDirectory; // Previous directory
    public string currentPath; // Current directory path
    public string previousPath; // Previous directory path

    // Other Variables
    public float startTime; // Start time for terminal session

    void Start()
    {

        startTime = Time.time;

        tmpInputField.onEndEdit.AddListener(HandleInputEndEdit);  //wtf is HandleInputEndEdit not a function?
        tmpInputField.Select();
        tmpInputField.ActivateInputField();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        VerticalLayoutGroup verticalLayoutGroup = parentPanel.GetComponent<VerticalLayoutGroup>();
        if (verticalLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
        }

        MakeInitialDirectorySystem();

    }

    void Update()
    {
        // Update TextMeshPro text with username
        string username = name;
        Admin = username == "Admin" ? true : false;
        TextMeshProUGUI[] textMeshPro = valueUserInput.GetComponentsInChildren<TextMeshProUGUI>();
        if (textMeshPro.Length > 0)
        {
            textMeshPro[0].text = $"TS {username}";
        }

        // Adjust RectTransform size if target and adjustment RectTransforms are assigned
        if (targetRectTransform != null && toAdjustRectTransform != null)
        {
            float targetY = targetRectTransform.anchoredPosition.y;
            float newHeight = Mathf.Max(minHeight, -targetY + (60f * i));
            toAdjustRectTransform.sizeDelta = new Vector2(toAdjustRectTransform.sizeDelta.x, newHeight);
        }
        else
        {
            Debug.LogError("Target RectTransforms are not assigned!");
        }

        // Up Arrow: Navigate command history up
        if (Input.GetKeyDown(KeyCode.UpArrow) && cmdList.Count > 0)
        {
            inputArray.Clear();
            ListOfCommandsIndex = Mathf.Min(ListOfCommandsIndex + 1, cmdList.Count);
            tmpInputField.text = cmdList[cmdList.Count - ListOfCommandsIndex];
            tmpInputField.caretPosition = tmpInputField.text.Length;
        }

        // Down Arrow: Navigate command history down
        if (Input.GetKeyDown(KeyCode.DownArrow) && cmdList.Count > 0)
        {
            if (ListOfCommandsIndex > 1)
            {
                inputArray.Clear();
                ListOfCommandsIndex--;
                tmpInputField.text = cmdList[cmdList.Count - ListOfCommandsIndex];
                tmpInputField.caretPosition = tmpInputField.text.Length;
            }
            else if (ListOfCommandsIndex == 1)
            {
                inputArray.Clear();
                ListOfCommandsIndex--;
                tmpInputField.text = "";
            }
        }

        // Reset command history navigation if any edits or typing occur
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.DownArrow))
        {
            ListOfCommandsIndex = 0;
        }

        DirName.GetComponent<TMP_Text>().text = currentPath;
        VerticalLayoutGroup verticalLayoutGroup = parentPanel.GetComponent<VerticalLayoutGroup>();
        if (verticalLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
        }


    }

    private void FixedUpdate()
    {
        tmpInputField.Select();
        tmpInputField.ActivateInputField();
    }

    private void HelpCommand()
    {
        // Help and Manual Commands
        outputArray.Add("You can type 'help' to get a list of commands."); // Just Print // Done
        outputArray.Add("You can type 'man [command]' to get a detailed working of [command]."); // Just Print // Done

        // Navigation and Directory Commands
        outputArray.Add("You can type 'pwd' to print the current working directory."); // Just Print // Done
        outputArray.Add("You can type 'cd' to change directories."); // Do Something // Done
        outputArray.Add("You can type 'ls' to list the contents of a directory."); // Just Print // Rem for imp // Done
        outputArray.Add("You can type 'mkdir' to create a new directory."); // Do Something // Need Admin Access // Rem for imp // Done // TODO Admin Access
        outputArray.Add("You can type 'rmdir' to remove a directory."); // Do Something // Need Admin Access // Done

        // File Manipulation Commands
        outputArray.Add("You can type 'touch' to create a new file."); // Do Something // Need Admin Access // Done // TODO Admin Access
        outputArray.Add("You can type 'rm' to remove a file."); // Do Something // Need Admin Access // Done
        outputArray.Add("You can type 'cat' to read the contents of a file."); // Do Something // Some need Admin Access // Done // TODO Admin Access
        outputArray.Add("You can type 'echo' to print text to the screen."); // Just Print // Done

        // Miscellaneous Commands
        outputArray.Add("You can type 'clear' to clear the screen."); // Do Something // Done
        outputArray.Add("You can type 'whoami' to print the current username."); // Just Print // Done
        outputArray.Add("You can type 'date' to print the current date and time."); // Just Print // Done
        outputArray.Add("You can type 'uptime' to print the time since the terminal started."); // Just Print // Done
        outputArray.Add("You can type 'exit' to exit the terminal."); // Do Something // Done

        // Other Commands // not actual linux commands
        outputArray.Add("You can type 'user [name]' to change the username to [name]."); // Do Something // Done
    }

    private void manCommand()
    {
        // Help and Manual Commands
            
        if (inputArray[1] == "help")
        {
            outputArray.Clear();
            outputArray.Add("help: help");
            outputArray.Add("Print information about all the available commands.");
        }

        else if (inputArray[1] == "man")
        {
            outputArray.Clear();
            outputArray.Add("man: man [command]");
            outputArray.Add("Print detailed information about [command].");
        }

        // Navigation and Directory Commands

        else if (inputArray[1] == "pwd")
        {
            outputArray.Clear();
            outputArray.Add("pwd: pwd");
            outputArray.Add("Print the current working directory.");
        }

        else if (inputArray[1] == "cd")
        {
            outputArray.Clear();
            outputArray.Add("cd: cd [directory]");
            outputArray.Add("Change the current directory to [directory].");
        }

        else if (inputArray[1] == "ls")
        {
            outputArray.Clear();
            outputArray.Add("ls: ls");
            outputArray.Add("List the contents of the current directory.");
        }

        else if (inputArray[1] == "mkdir")
        {
            outputArray.Clear();
            outputArray.Add("mkdir: mkdir [directory]");
            outputArray.Add("Create a new directory named [directory].");
        }

        else if (inputArray[1] == "rmdir")
        {
            outputArray.Clear();
            outputArray.Add("rmdir: rmdir [directory]");
            outputArray.Add("Remove the directory named [directory].");
        }

        // File Manipulation Commands

        else if (inputArray[1] == "touch")
        {
            outputArray.Clear();
            outputArray.Add("touch: touch [file] [content]");
            outputArray.Add("Create a new file named [file] with [content].");
        }

        else if (inputArray[1] == "rm")
        {
            outputArray.Clear();
            outputArray.Add("rm: rm [file]");
            outputArray.Add("Remove the file named [file].");
        }

        else if (inputArray[1] == "cat")
        {
            outputArray.Clear();
            outputArray.Add("cat: cat [file]");
            outputArray.Add("Print the contents of [file].");
        }

        else if (inputArray[1] == "echo")
        {
            outputArray.Clear();
            outputArray.Add("echo: echo [text]");
            outputArray.Add("Print [text] to the screen.");
        }

        // Miscellaneous Commands

        else if (inputArray[1] == "clear")
        {
            outputArray.Clear();
            outputArray.Add("clear: clear");
            outputArray.Add("Clear the screen.");
        }

        else if (inputArray[1] == "whoami")
        {
            outputArray.Clear();
            outputArray.Add("whoami: whoami");
            outputArray.Add("Print the current username.");
        }

        else if (inputArray[1] == "date")
        {
            outputArray.Clear();
            outputArray.Add("date: date");
            outputArray.Add("Print the current date and time.");
        }

        else if (inputArray[1] == "uptime")
        {
            outputArray.Clear();
            outputArray.Add("uptime: uptime");
            outputArray.Add("Print the time since the terminal started.");
        }

        else if (inputArray[1] == "exit")
        {
            outputArray.Clear();
            outputArray.Add("exit: exit");
            outputArray.Add("Exit the terminal.");
        }

        // Other Commands // not actual linux commands

        else if (inputArray[1] == "user")
        {
            outputArray.Clear();
            outputArray.Add("user: user [name]");
            outputArray.Add("Change the username to [name].");
        }
    }

    public void PrintWorkingDirectory()
    {
        outputArray.Add("Path:");
        outputArray.Add(name + currentPath);
    }
    
    public void ChangeDirectory(string path)
    {
        if (path == "..")
        {
            if (currentDirectory != rootDirectory)
            {
                currentDirectory = currentDirectory.ParentDirectory;
                currentPath = currentPath.Substring(0, currentPath.LastIndexOf('/'));
                if (currentPath == "")
                {
                    currentPath = "/";
                }
            }
            else
            {
                outputArray.Add("You are already at the root directory.");
            }
        }
        else if (currentDirectory.Subdirectories.ContainsKey(path))
        {
            currentDirectory = currentDirectory.Subdirectories[path];
            currentPath = currentPath == "/" ? $"/{path}" : $"{currentPath}/{path}";
        }
        else
        {
            outputArray.Add($"Directory {path} does not exist.");
        }
    }
    
    public void ListDirectory()
    {
        // outputArray.Add("Contents of directory:");
        outputArray.Add("Other Directories:");
        foreach (var directoryName in currentDirectory.Subdirectories.Keys)
        {
            outputArray.Add($"  {directoryName}");
        }
        outputArray.Add("Files:");
        foreach (var fileName in currentDirectory.Files)
        {
            outputArray.Add($"  {fileName.Key}");
        }
    }

    public void CreateDirectory(string directoryName)
    {
        if (currentDirectory.Subdirectories.ContainsKey(directoryName))
        {
            outputArray.Add($"Directory {directoryName} already exists.");
        }
        else
        {
            currentDirectory.Subdirectories[directoryName] = new Directory(directoryName, currentDirectory);
            outputArray.Add($"Directory '{directoryName}' created.");
        }
    }

    public void RemoveDirectory(string directoryName)
    {
        if (currentDirectory.Subdirectories.ContainsKey(directoryName))
        {
            currentDirectory.Subdirectories.Remove(directoryName);
            outputArray.Add($"Directory {directoryName} removed.");
        }
        else
        {
            outputArray.Add($"Directory {directoryName} does not exist.");
        }
    }

    public void TouchCommand() {
        string fileContent = string.Join(" ", inputArray.GetRange(2, inputArray.Count - 2));
        currentDirectory.Files[inputArray[1]] = new Files(inputArray[1], fileContent);
    }

    public void RemoveFile(string fileName)
    {
        if (currentDirectory.Files.ContainsKey(fileName))
        {
            currentDirectory.Files.Remove(fileName);
            outputArray.Add($"File {fileName} removed.");
        }
        else
        {
            outputArray.Add($"File {fileName} does not exist.");
        }
    }

    public void OpenFile(string fileName)
    {
        if (currentDirectory.Files.ContainsKey(fileName))
        {
            outputArray.Add($"Contents of {fileName}:");
            outputArray.Add(currentDirectory.Files[fileName].Content);
        }
        else
        {
            outputArray.Add($"File {fileName} does not exist.");
        }
    }

    public void EchoCommand() {
        outputArray.Add(inputArray[1]);
    }

    private void ClearTerminal()
    {
        // Destroy only the instantiated input and output panels
        foreach (Transform child in parentPanel.transform)
        {
            if (child.CompareTag("InstantiatedPanel"))
            {
                Destroy(child.gameObject);
            }
        }

        // Reset the input and output arrays
        inputArray.Clear();
        outputArray.Clear();

        // Adjust the height after clearing
        AdjustHeight();
    }

    public void whoami()
    {
        outputArray.Add(name);
    }

    public void GetDate()
    {
        outputArray.Add(System.DateTime.Now.ToString());
    }

    public void GetUptime()
    {
        float uptime = Time.time - startTime;

        int hours = (int)(uptime / 3600);
        int minutes = (int)((uptime % 3600) / 60);
        int seconds = (int)(uptime % 60);

        outputArray.Add($"Uptime: {hours} hours, {minutes} minutes, {seconds} seconds.");
    }

    public void ExitTerminal()
    {
        Application.Quit();
    }

    public void ChangeUser(string nm)
    {
        name = nm;
    }

    private void Printer(string userInput)
    {
        string username = name;

        GameObject inputPanel = Instantiate(valueUserInput, parentPanel);
        inputPanel.tag = "InstantiatedPanel";
        TextMeshProUGUI[] InputComponents = inputPanel.GetComponentsInChildren<TextMeshProUGUI>();
        InputComponents[0].text = $"TS {username}";
        InputComponents[InputComponents.Length - 1].text = userInput;
        tmpInputField.text = "";

        inputArray = new List<string>(userInput.Split());
        if (userInput != "")
        {
            cmdList.Add(userInput);
        }

        if (inputArray.Count > 0)
        {
            // Help and Manual Commands

            //+ Help Command

            if ((inputArray[0] == "help" || inputArray[0] == "/help" ) && inputArray.Count <= 1)
            {
                outputArray.Clear();
                HelpCommand();
            }

            //+ man Command

            else if (inputArray[0] == "man" && inputArray.Count > 1)
            {
                outputArray.Clear();
                manCommand();
            }

            // Navigation and Directory Commands

            //+ Pwd Command

            else if (inputArray[0] == "pwd")
            {
                outputArray.Clear();
                PrintWorkingDirectory();
            }

            //+ Cd Command

            else if (inputArray[0] == "cd" && inputArray.Count > 1)
            {
                outputArray.Clear();
                ChangeDirectory(inputArray[1]);
            }
            else if (inputArray[0] == "cd" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("cd: missing operand");
            }
            else if (inputArray[0] == "cd" && inputArray[1] == "..")
            {
                outputArray.Clear();

                ChangeDirectory("..");
            }

            //+ LS Command

            else if (inputArray[0] == "ls")
            {
                outputArray.Clear();
                ListDirectory();
            }

            //+ Mkdir Command

            else if (inputArray[0] == "mkdir" && inputArray.Count > 1)
            {
                outputArray.Clear();
                CreateDirectory(inputArray[1]);
            }
            else if (inputArray[0] == "mkdir" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("mkdir: missing operand");
            }

            //+ rmdir Command

            else if (inputArray[0] == "rmdir" && inputArray.Count > 1 && Admin)
            {
                outputArray.Clear();
                RemoveDirectory(inputArray[1]);
            }
            else if (inputArray[0] == "rmdir" && inputArray.Count > 1 && !Admin)
            {
                outputArray.Clear();
                outputArray.Add("rmdir: permission denied");
            }
            else if (inputArray[0] == "rmdir" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("rmdir: missing operand");
            }

            // File Manipulation Commands

            //+ Touch Command

            else if (inputArray[0] == "touch" && inputArray.Count > 2)
            {
                outputArray.Clear();
                TouchCommand();
            }
            else if (inputArray[0] == "touch" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("touch: missing operand");
            }
            else if (inputArray[0] == "touch" && inputArray.Count <= 2)
            {
                outputArray.Clear();
                outputArray.Add("touch: missing file content");
            }

            //+ rm Command

            else if (inputArray[0] == "rm" && inputArray.Count > 1 && Admin)
            {
                outputArray.Clear();
                RemoveFile(inputArray[1]);
            }
            else if (inputArray[0] == "rm" && inputArray.Count > 1 && !Admin)
            {
                outputArray.Clear();
                outputArray.Add("rm: permission denied");
            }
            else if (inputArray[0] == "rm" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("rm: missing operand");
            }

            //+ Cat Command

            else if (inputArray[0] == "cat" && inputArray.Count > 1)
            {
                outputArray.Clear();
                OpenFile(inputArray[1]);
            }
            else if (inputArray[0] == "cat" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("cat: missing operand");
            }

            //+ Echo Command

            else if (inputArray[0] == "echo" && inputArray.Count > 1)
            {
                outputArray.Clear();
                EchoCommand();
            }

            else if (inputArray[0] == "echo" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("echo: missing operand");
            }

            // Miscellaneous Commands

            //+ Clear Command

            else if (inputArray[0] == "clear")
            {
                ClearTerminal();
            }
            else if (inputArray[0] == "cls")
            {
                ClearTerminal();
            }

            //+ Whoami Command

            else if (inputArray[0] == "whoami")
            {
                outputArray.Clear();
                whoami();
            }

            //+ date Command

            else if (inputArray[0] == "date")
            {
                outputArray.Clear();
                GetDate();
            }

            //+ uptime Command

            else if (inputArray[0] == "uptime")
            {
                outputArray.Clear();
                GetUptime();
            }

            //+ Exit Command

            else if (inputArray[0] == "exit")
            {
                outputArray.Clear();
                ExitTerminal();
            }

            // Other Commands // not actual linux commands

            //+ Change Username Command

            else if (inputArray[0] == "user" && inputArray.Count > 1)
            {
                outputArray.Clear();
                ChangeUser(inputArray[1]);
            }

            else if (inputArray[0] == "user" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("user: missing operand");
            }

            //+ else

            else
            {
                outputArray.Clear();
                GameObject errorPanel = Instantiate(valueUserError, parentPanel);
                errorPanel.tag = "InstantiatedPanel";
            }

        }

        foreach (string word in outputArray)
        {
            GameObject outputPanel = Instantiate(valueUserOutput, parentPanel);
            outputPanel.tag = "InstantiatedPanel";
            TextMeshProUGUI[] OutputComponents = outputPanel.GetComponentsInChildren<TextMeshProUGUI>();
            OutputComponents[0].text = word;
        }

        AdjustHeight();
    }

    private void MakeInitialDirectorySystem()
    {
        currentPath = "/";
        rootDirectory = new Directory(currentPath);
        currentDirectory = rootDirectory;
        previousDirectory = rootDirectory;

        rootDirectory.Subdirectories["Home"] = new Directory("Home", rootDirectory);
        rootDirectory.Subdirectories["Home"].Files["file1"] = new Files("file1", "This is file1.");
    }

    private void HandleInputEndEdit(string userInput)
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            Printer(userInput);
        }
    }

    private void AdjustHeight()
    {
        float targetY = targetRectTransform.anchoredPosition.y;
        float newHeight = Mathf.Max(minHeight, -targetY + 60f);
        toAdjustRectTransform.anchoredPosition = new Vector2(toAdjustRectTransform.anchoredPosition.x, (newHeight + 60) / 2);

        valueUserInput.transform.SetAsLastSibling();

        VerticalLayoutGroup verticalLayoutGroup = parentPanel.GetComponent<VerticalLayoutGroup>();
        if (verticalLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
        }
    }

}