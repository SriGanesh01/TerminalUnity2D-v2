// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using System.Linq;
// using UnityEngine.UI;

// public class Terminalv3 : MonoBehaviour
// {
//     [SerializeField] GameObject valueUserInput;
//     [SerializeField] GameObject valueUserOutput;
//     [SerializeField] GameObject valueUserError;
//     [SerializeField] TMP_InputField tmpInputField;
//     [SerializeField] Transform parentPanel;
//     [SerializeField] RectTransform targetRectTransform;
//     [SerializeField] RectTransform toAdjustRectTransform;
//     public  List<string> inputArray;
//     public  List<string> outputArray = new List<string>();
//     public List<string> lsValuesHome = new List<string> { "A", "B", "C", "D", "E" };
//     public List<string> lsValuesA = new List<string> { "A1", "B1", "C1", "D1", "E1" };
//     public List<string> lsValuesB = new List<string> { "A2", "B2", "C2", "D2", "E2" };
//     public List<string> lsValuesC = new List<string> { "A3", "B3", "C3", "D3", "E3" };
//     public List<string> lsValuesD = new List<string> { "A4", "B4", "C4", "D4", "E4" };
//     public List<string> lsValuesE = new List<string> { "A5", "B5", "C5", "D5", "E5" };
//     public List<string> lsDirs = new List<string> { "lsValuesHome", "lsValuesA", "lsValuesB", "lsValuesC", "lsValuesD", "lsValuesE" };


//     [SerializeField] float minHeight = 2160f;
//     [SerializeField] int i = 1;
//     public new string name = "Cat";
//     public string dir1 = "";
//     public string dir2 = "";
//     public string dir3 = "";
    

//     void Start()
//     {
//         // inputArray = new List<string>();
//         // outputArray = new List<string>();

        

//         tmpInputField.onEndEdit.AddListener(HandleInputEndEdit);
//         tmpInputField.Select();
//         tmpInputField.ActivateInputField();

//         VerticalLayoutGroup verticalLayoutGroup = parentPanel.GetComponent<VerticalLayoutGroup>();
//         if (verticalLayoutGroup != null)
//         {
//             LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
//         }
//     }

//     void Update()
//     {
//         string username = name;
//         string Directory1 = dir1;
//         string Directory2 = dir2;
//         string Directory3 = dir3;

//         TextMeshProUGUI[] textMeshPro = valueUserInput.GetComponentsInChildren<TextMeshProUGUI>();
//         textMeshPro[0].text = $"TS {username}";
//         textMeshPro[1].text = $"{Directory1}";
//         textMeshPro[2].text = $"{Directory2}";
//         textMeshPro[3].text = $"{Directory3}~$";
//         //i = parentPanel.childCount;
//         if (targetRectTransform != null && toAdjustRectTransform != null)
//         {
//             float targetY = targetRectTransform.anchoredPosition.y;
//             float newHeight = Mathf.Max(minHeight, -targetY + (60f * i));
//             toAdjustRectTransform.sizeDelta = new Vector2(toAdjustRectTransform.sizeDelta.x, newHeight);
//         }
//         else
//         {
//             Debug.LogError("Target RectTransforms are not assigned!");
//         }
//     }

//     private void FixedUpdate()
//     {
//         tmpInputField.Select();
//         tmpInputField.ActivateInputField();
//     }

//     private void HandleInputEndEdit(string userInput)
//     {
//         if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
//         {
//             ExecuteCommand(userInput);
//         }
//     }

//     private void ExecuteCommand(string userInput)
//     {
//         Printer(userInput);
//     }

//     private void Printer(string userInput)
//     {
//         string username = name;
//         string Directory1 = dir1;
//         string Directory2 = dir2;
//         string Directory3 = dir3;
//         GameObject inputPanel = Instantiate(valueUserInput, parentPanel);
//         inputPanel.tag = "InstantiatedPanel";
//         TextMeshProUGUI[] InputComponents = inputPanel.GetComponentsInChildren<TextMeshProUGUI>();
//         InputComponents[0].text = $"TS {username}";
//         InputComponents[1].text = $"{Directory1}";
//         InputComponents[2].text = $"{Directory2}";
//         InputComponents[3].text = $"{Directory3}~$";
//         InputComponents[4].text = userInput;
//         tmpInputField.text = "";

//         inputArray = new List<string>(userInput.Split());

//         if (inputArray.Count > 0)
//         {
//             if (inputArray[0] == "Help" || inputArray[0] == "help")
//             {
//                 outputArray.Clear();
//                 outputArray.Add("You can type 'help' to get a list of commands."); //Just Print //Done
//                 outputArray.Add("You can type 'cd' to change directories."); //Do Something
//                 outputArray.Add("You can type 'ls' to list the contents of a directory."); //Just Print //Done  //Rem for imp
//                 outputArray.Add("You can type 'cat' to read the contents of a file."); //Do Something //Some need Admin Access
//                 outputArray.Add("You can type 'echo' to print text to the screen."); //Just Print //Done
//                 outputArray.Add("You can type 'pwd' to print the current working directory."); //Just Print
//                 outputArray.Add("You can type 'mkdir' to create a new directory."); //Do Something //Done //Need Admin Access //Rem for imp
//                 outputArray.Add("You can type 'cp' to copy a file or directory."); //Do Something //Need Admin Access
//                 outputArray.Add("You can type 'rm' to remove a file or directory."); //Do Something //Done //Need Admin Access
//                 outputArray.Add("You can type 'rmdir' to remove a directory."); //Do Something //Need Admin Access
//                 outputArray.Add("You can type 'touch' to create a new file."); //Do Something //Need Admin Access
//                 outputArray.Add("You can type 'clear' to clear the screen."); //Do Something //Done
//                 outputArray.Add("You can type 'exit' to exit the terminal."); //Do Something
//             }
//             else if (inputArray[0] == "ls")
//             {
//                 if (Directory1 == "")
//                 {
//                     outputArray.Clear();
//                     foreach (string word in lsValuesHome)
//                     {
//                         outputArray.Add(word);
//                     }
//                 }
//                 else if (Directory1 == lsValuesHome[0]){
//                     outputArray.Clear();
//                     foreach (string word in lsValuesA)
//                     {
//                         outputArray.Add(word);
//                     }
//                 }
//                 else if (Directory1 == lsValuesHome[1])
//                 {
//                     outputArray.Clear();
//                     foreach (string word in lsValuesB)
//                     {
//                         outputArray.Add(word);
//                     }
//                 }
//                 else if (Directory1 == lsValuesHome[2])
//                 {
//                     outputArray.Clear();
//                     foreach (string word in lsValuesC)
//                     {
//                         outputArray.Add(word);
//                     }
//                 }
//                 else if (Directory1 == lsValuesHome[3])
//                 {
//                     outputArray.Clear();
//                     foreach (string word in lsValuesD)
//                     {
//                         outputArray.Add(word);
//                     }
//                 }
//                 else if (Directory1 == lsValuesHome[4])
//                 {
//                     outputArray.Clear();
//                     foreach (string word in lsValuesE)
//                     {
//                         outputArray.Add(word);
//                     }
//                 }
                
//             }
//             else if (inputArray[0] == "echo" && inputArray.Count > 1)
//             {
//                 outputArray.Clear();
//                 outputArray.Add(inputArray[1]);
//             }
//             else if (inputArray[0] == "echo" && inputArray.Count <= 1)
//             {
//                 outputArray.Clear();
//                 outputArray.Add("echo: missing operand");
//             }
//             else if (inputArray[0] == "mkdir" && inputArray.Count > 1)
//             {
//                 if (Directory1 == "")
//                 {
//                     if (username == "Admin")
//                     {
//                         outputArray.Clear();
//                         lsValuesHome.Add(inputArray[1]);
//                         outputArray.Add($"Directory {inputArray[1]} created");
//                     }
//                     else
//                     {
//                         outputArray.Clear();
//                         outputArray.Add("Admin Access Denied");
//                     }
//                 }
//                 else
//                 {
//                     foreach (string word1 in lsValuesHome)
//                     {
//                         if (Directory1 == word1)
//                         {
//                             if (username == "Admin")
//                             {
//                                 outputArray.Clear();
//                                 lsValuesA.Add(inputArray[1]);
//                                 outputArray.Add($"Directory {inputArray[1]} created");
//                             }
//                             else
//                             {
//                                 outputArray.Clear();
//                                 outputArray.Add("Admin Access Denied");
//                             }
//                         }
//                     }
//                 }  
//             }
//             else if (inputArray[0] == "mkdir" && inputArray.Count <= 1)
//             {
//                 outputArray.Clear();
//                 outputArray.Add("mkdir: missing operand");
//             }

//             else if (inputArray[0] == "rm" && inputArray.Count > 1)
//             {
//                 if (Directory1 == "")
//                 {
//                     if (username == "Admin")
//                     {
//                         RemoveCharacterFromList(inputArray[1], lsValuesHome);
//                     }
//                     else
//                     {
//                         outputArray.Clear();
//                         outputArray.Add("Admin Access Denied");
//                     }
//                 }
//                 else
//                 {
//                     foreach (string word1 in lsValuesHome)
//                     {
//                         if (Directory1 == word1)
//                         {
//                             if (username == "Admin")
//                             {
//                                 RemoveCharacterFromList(inputArray[1], lsValuesA);
//                             }
//                             else
//                             {
//                                 outputArray.Clear();
//                                 outputArray.Add("Admin Access Denied");
//                             }
//                         }
                        
//                     }
//                 }
//             } 
//             else if (inputArray[0] == "clear")
//             {
//                 ClearTerminal();
//             }
//             else
//             {
//                 outputArray.Clear();
//                 GameObject errorPanel = Instantiate(valueUserError, parentPanel);
//                 errorPanel.tag = "InstantiatedPanel";
//             }
            
//         }

//         foreach (string word in outputArray)
//         {
//             GameObject outputPanel = Instantiate(valueUserOutput, parentPanel);
//             outputPanel.tag = "InstantiatedPanel";
//             TextMeshProUGUI[] OutputComponents = outputPanel.GetComponentsInChildren<TextMeshProUGUI>();
//             OutputComponents[0].text = word;
//         }

//         AdjustHeight();
//     }

//     private void ClearTerminal()
//     {
//         // Destroy only the instantiated input and output panels
//         foreach (Transform child in parentPanel.transform)
//         {
//             if (child.CompareTag("InstantiatedPanel"))
//             {
//                 Destroy(child.gameObject);
//             }
//         }

//         // Reset the input and output arrays
//         inputArray.Clear();
//         outputArray.Clear();

//         // Adjust the height after clearing
//         AdjustHeight();
//     }


//     public void RemoveCharacterFromList(string characterToRemove, List<string> lsValues)
//     {
//         // Check if the character exists in the list before removing
//         if (lsValues.Contains(characterToRemove))
//         {
//             // Remove the character
//             lsValues.Remove(characterToRemove);
//             outputArray.Clear();
//             outputArray.Add($"File {characterToRemove} removed");
//         }
//         else
//         {
//             outputArray.Clear();
//             outputArray.Add("Directory does not exist");
//         }
//     }

    



//     private void AdjustHeight()
//     {
//         float targetY = targetRectTransform.anchoredPosition.y;
//         float newHeight = Mathf.Max(minHeight, -targetY + 60f);
//         toAdjustRectTransform.anchoredPosition = new Vector2(toAdjustRectTransform.anchoredPosition.x, (newHeight + 60) / 2);

//         valueUserInput.transform.SetAsLastSibling();

//         VerticalLayoutGroup verticalLayoutGroup = parentPanel.GetComponent<VerticalLayoutGroup>();
//         if (verticalLayoutGroup != null)
//         {
//             LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
//         }
//     }
// }
