using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Terminalv3 : MonoBehaviour
{
    [SerializeField] GameObject valueUserInput;
    [SerializeField] GameObject valueUserOutput;
    [SerializeField] GameObject valueUserError;
    [SerializeField] TMP_InputField tmpInputField;
    [SerializeField] Transform parentPanel;
    [SerializeField] RectTransform targetRectTransform;
    [SerializeField] RectTransform toAdjustRectTransform;
    [SerializeField] RectTransform DirName;
    public List<string> inputArray;
    public List<string> outputArray = new List<string>();
    public List<string> cmdList = new List<string>();
    public bool Admin = false;

    [SerializeField] float minHeight = 2160f;
    [SerializeField] int i = 1;
    public new string name = "user";
    public int ListOfCommandsIndex;

    public Directory rootDirectory;
    public Directory currentDirectory;
    public Directory previousDirectory;
    public string currentPath;


    void Start()
    {
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

    private void MakeInitialDirectorySystem() {
        currentPath = "/";
        rootDirectory = new Directory(currentPath);
        currentDirectory = rootDirectory;
        previousDirectory = rootDirectory;

        rootDirectory.Subdirectories["Home"] = new Directory("Home");
        rootDirectory.Subdirectories["Home"].Files["file1"] = new Files("file1", "This is file1.");
        
    }

    public void CreateDirectory(string directoryName)
    {
        if (currentDirectory.Subdirectories.ContainsKey(directoryName))
        {
            outputArray.Add($"Directory {directoryName} already exists.");
        }
        else
        {
            currentDirectory.Subdirectories[directoryName] = new Directory(directoryName);
            outputArray.Add($"Directory '{directoryName}' created.");
        }
    }

    public void RemoveDirectory(string directoryName) {
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

    public void RemoveFile(string fileName) {
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

    public void OpenFile(string fileName) {
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


    public void ChangeDirectory(string path)
    {
        if (currentDirectory.Subdirectories.ContainsKey(path))
        {
            previousDirectory = currentDirectory;
            currentDirectory = currentDirectory.Subdirectories[path];
            currentPath += path + "/";
        }
        else if (path == "..")
        {
            currentDirectory = previousDirectory;
            currentPath = currentPath.Substring(0, currentPath.Length - previousDirectory.Name.Length - 1);
            // currentDirectory = previousDirectory;
            // currentPath = currentPath.Substring(0, currentPath.Length - currentDirectory.Name.Length - 1);
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

    public void PrintWorkingDirectory() {
        outputArray.Add("Path:");
        outputArray.Add(name + currentPath);
    }

    public void ChangeUser(string nm) {
        name = nm;
    }





    private void FixedUpdate()
    {
        tmpInputField.Select();
        tmpInputField.ActivateInputField();
    }

    private void HandleInputEndEdit(string userInput)
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            Printer(userInput);
        }
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
            //+ Help Command

            if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("You can type 'help' to get a list of commands."); //Just Print //Done
                outputArray.Add("You can type 'help [name of the command]' to get a detailed working of that command."); //Just Print //Done
                outputArray.Add("You can type 'cd' to change directories."); //Do Something //Done
                outputArray.Add("You can type 'ls' to list the contents of a directory."); //Just Print //Rem for imp //Done
                outputArray.Add("You can type 'cat' to read the contents of a file."); //Do Something //Some need Admin Access //Done //TODO Admin Access
                outputArray.Add("You can type 'echo' to print text to the screen."); //Just Print //Done
                outputArray.Add("You can type 'pwd' to print the current working directory."); //Just Print //Done
                outputArray.Add("You can type 'mkdir' to create a new directory."); //Do Something //Need Admin Access //Rem for imp //Done //TODO Admin Access
                // outputArray.Add("You can type 'cp' to copy a file or directory."); //Do Something //Need Admin Access //TODO
                outputArray.Add("You can type 'rm' to remove a file."); //Do Something //Need Admin Access //Done
                outputArray.Add("You can type 'rmdir' to remove a directory."); //Do Something //Need Admin Access //Done
                outputArray.Add("You can type 'touch' to create a new file."); //Do Something //Need Admin Access //Done //TODO Admin Access
                outputArray.Add("You can type 'clear' to clear the screen."); //Do Something //Done
                outputArray.Add("You can type 'user' to change the username."); //Do Something //Done
                outputArray.Add("You can type 'exit' to exit the terminal."); //Do Something //Done
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && (inputArray[1] == "help" || inputArray[1] == "/help")) {
                outputArray.Clear();
                outputArray.Add("help: help");
                outputArray.Add("Print information about all the available commands.");
                outputArray.Add("help: help [command]");
                outputArray.Add("Print information about [command].");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "cd") {
                outputArray.Clear();
                outputArray.Add("cd: cd [directory]");
                outputArray.Add("Change the current directory to [directory].");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "ls") {
                outputArray.Clear();
                outputArray.Add("ls: ls");
                outputArray.Add("List the contents of the current directory.");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "cat") {
                outputArray.Clear();
                outputArray.Add("cat: cat [file]");
                outputArray.Add("Print the contents of [file].");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "echo") {
                outputArray.Clear();
                outputArray.Add("echo: echo [text]");
                outputArray.Add("Print [text] to the screen.");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "pwd") {
                outputArray.Clear();
                outputArray.Add("pwd: pwd");
                outputArray.Add("Print the current working directory.");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "mkdir") {
                outputArray.Clear();
                outputArray.Add("mkdir: mkdir [directory]");
                outputArray.Add("Create a new directory named [directory].");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "rm") {
                outputArray.Clear();
                outputArray.Add("rm: rm [file]");
                outputArray.Add("Remove the file named [file].");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "rmdir") {
                outputArray.Clear();
                outputArray.Add("rmdir: rmdir [directory]");
                outputArray.Add("Remove the directory named [directory].");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "touch") {
                outputArray.Clear();
                outputArray.Add("touch: touch [file] [content]");
                outputArray.Add("Create a new file named [file] with [content].");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "clear") {
                outputArray.Clear();
                outputArray.Add("clear: clear");
                outputArray.Add("Clear the screen.");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "user") {
                outputArray.Clear();
                outputArray.Add("user: user [name]");
                outputArray.Add("Change the username to [name].");
            }

            else if ((inputArray[0] == "help" || inputArray[0] == "/help") && inputArray[1] == "exit") {
                outputArray.Clear();
                outputArray.Add("exit: exit");
                outputArray.Add("Exit the terminal.");
            }

            //+ Echo Command

            else if (inputArray[0] == "echo" && inputArray.Count > 1)
            {
                outputArray.Clear();
                outputArray.Add(inputArray[1]);
            }

            else if (inputArray[0] == "echo" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("echo: missing operand");
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

            //+ Cd Command

            else if (inputArray[0] == "cd" && inputArray.Count > 1) {
                outputArray.Clear();
                ChangeDirectory(inputArray[1]);
            }
            else if (inputArray[0] == "cd" && inputArray.Count <= 1) {
                outputArray.Clear();
                outputArray.Add("cd: missing operand");
            }
            else if (inputArray[0] == "cd" && inputArray[1] == "..") {
                outputArray.Clear();
                
                ChangeDirectory("..");
            }

            //+ Pwd Command

            else if (inputArray[0] == "pwd") {
                outputArray.Clear();
                PrintWorkingDirectory();
            }

            //+ Cat Command

            else if (inputArray[0] == "cat" && inputArray.Count > 1) {
                outputArray.Clear();
                OpenFile(inputArray[1]);
            }
            else if (inputArray[0] == "cat" && inputArray.Count <= 1) {
                outputArray.Clear();
                outputArray.Add("cat: missing operand");
            }

            //+ Touch Command

            else if (inputArray[0] == "touch" && inputArray.Count > 2) {
                outputArray.Clear();
                string fileContent = string.Join(" ", inputArray.GetRange(2, inputArray.Count - 2));
                currentDirectory.Files[inputArray[1]] = new Files(inputArray[1], fileContent);
            }
            else if (inputArray[0] == "touch" && inputArray.Count <= 1) {
                outputArray.Clear();
                outputArray.Add("touch: missing operand");
            }
            else if (inputArray[0] == "touch" && inputArray.Count <= 2) {
                outputArray.Clear();
                outputArray.Add("touch: missing file content");
            }

            //+ rm Command

            else if (inputArray[0] == "rm" && inputArray.Count > 1 && Admin) {
                outputArray.Clear();
                RemoveFile(inputArray[1]);
            }
            else if (inputArray[0] == "rm" && inputArray.Count > 1 && !Admin) {
                outputArray.Clear();
                outputArray.Add("rm: permission denied");
            }
            else if (inputArray[0] == "rm" && inputArray.Count <= 1) {
                outputArray.Clear();
                outputArray.Add("rm: missing operand");
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

            //+ Clear Command

            else if (inputArray[0] == "clear")
            {
                ClearTerminal();
            }

            //+ Exit Command

            else if (inputArray[0] == "exit")
            {
                outputArray.Clear();
                Application.Quit();
            }

            //+ Other Commands

            //+ Change Username Command

            else if (inputArray[0] == "user" && inputArray.Count > 1) {
                outputArray.Clear();
                ChangeUser(inputArray[1]);
            }

            else if (inputArray[0] == "user" && inputArray.Count <= 1) {
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
