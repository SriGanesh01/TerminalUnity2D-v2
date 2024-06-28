using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.IO;

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

    [SerializeField] float minHeight = 2160f;
    [SerializeField] int i = 1;
    public new string name = "Cat";
    public int ListOfCommandsIndex;

    public Directory rootDirectory;
    public Directory currentDirectory;
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
        }
        else
        {
            outputArray.Add($"File {fileName} does not exist.");
        }
    }

    public void OpenFile(string fileName) {
        if (currentDirectory.Files.ContainsKey(fileName))
        {
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
            currentDirectory = currentDirectory.Subdirectories[path];
            currentPath += path + "/";
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





    private void FixedUpdate()
    {
        tmpInputField.Select();
        tmpInputField.ActivateInputField();
    }

    private void HandleInputEndEdit(string userInput)
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            ExecuteCommand(userInput);
        }
    }

    private void ExecuteCommand(string userInput)
    {
        Printer(userInput); 
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

            if (inputArray[0] == "Help" || inputArray[0] == "help") //Done
            {
                outputArray.Clear();
                outputArray.Add("You can type 'help' to get a list of commands."); //Just Print //Done
                outputArray.Add("You can type 'cd' to change directories."); //Do Something //Done
                outputArray.Add("You can type 'ls' to list the contents of a directory."); //Just Print //Rem for imp //Done
                outputArray.Add("You can type 'cat' to read the contents of a file."); //Do Something //Some need Admin Access //Done //TODO Admin Access
                outputArray.Add("You can type 'echo' to print text to the screen."); //Just Print //Done
                outputArray.Add("You can type 'pwd' to print the current working directory."); //Just Print //Done
                outputArray.Add("You can type 'mkdir' to create a new directory."); //Do Something //Need Admin Access //Rem for imp //Done //TODO Admin Access
                outputArray.Add("You can type 'cp' to copy a file or directory."); //Do Something //Need Admin Access //TODO
                outputArray.Add("You can type 'rm' to remove a file."); //Do Something //Need Admin Access //Done //TODO Admin Access
                outputArray.Add("You can type 'rmdir' to remove a directory."); //Do Something //Need Admin Access //Done //TODO Admin Access
                outputArray.Add("You can type 'touch' to create a new file."); //Do Something //Need Admin Access //Done //TODO Admin Access
                outputArray.Add("You can type 'clear' to clear the screen."); //Do Something //Done
                outputArray.Add("You can type 'exit' to exit the terminal."); //Do Something //Done
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


            //+ Clear Command

            else if (inputArray[0] == "clear") //Done
            {
                ClearTerminal();
            }

            //+ Exit Command

            else if (inputArray[0] == "exit") //Done
            {
                outputArray.Clear();
                Application.Quit();
            }

            //+ rm Command

            else if (inputArray[0] == "rm" && inputArray.Count > 1) {
                outputArray.Clear();
                RemoveFile(inputArray[1]);
            }
            else if (inputArray[0] == "rm" && inputArray.Count <= 1) {
                outputArray.Clear();
                outputArray.Add("rm: missing operand");
            }

            //+ rmdir Command

            else if (inputArray[0] == "rmdir" && inputArray.Count > 1)
            {
                outputArray.Clear();
                RemoveDirectory(inputArray[1]);
            }
            else if (inputArray[0] == "rmdir" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("rmdir: missing operand");
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
