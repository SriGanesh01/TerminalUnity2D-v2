// if (inputArray.Count > 0)
//         {
//             //+ Help Command

//             if (inputArray[0] == "Help" || inputArray[0] == "help") //Done
//             {
//                 outputArray.Clear();
//                 outputArray.Add("You can type 'help' to get a list of commands."); //Just Print //Done
//                 outputArray.Add("You can type 'cd' to change directories."); //Do Something //Done
//                 outputArray.Add("You can type 'ls' to list the contents of a directory."); //Just Print //Rem for imp //Done
//                 outputArray.Add("You can type 'cat' to read the contents of a file."); //Do Something //Some need Admin Access //Done //TODO Admin Access
//                 outputArray.Add("You can type 'echo' to print text to the screen."); //Just Print //Done
//                 outputArray.Add("You can type 'pwd' to print the current working directory."); //Just Print //Done
//                 outputArray.Add("You can type 'mkdir' to create a new directory."); //Do Something //Need Admin Access //Rem for imp //Done //TODO Admin Access
//                 outputArray.Add("You can type 'cp' to copy a file or directory."); //Do Something //Need Admin Access //TODO
//                 outputArray.Add("You can type 'rm' to remove a file."); //Do Something //Need Admin Access //Done //TODO Admin Access
//                 outputArray.Add("You can type 'rmdir' to remove a directory."); //Do Something //Need Admin Access //Done //TODO Admin Access
//                 outputArray.Add("You can type 'touch' to create a new file."); //Do Something //Need Admin Access //Done //TODO Admin Access
//                 outputArray.Add("You can type 'clear' to clear the screen."); //Do Something //Done
//                 outputArray.Add("You can type 'exit' to exit the terminal."); //Do Something //Done
//             }

//             //+ Echo Command

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

//             //+ LS Command

//             else if (inputArray[0] == "ls")
//             {
//                 outputArray.Clear();
//                 ListDirectory();
//             }

//             //+ Mkdir Command

//             else if (inputArray[0] == "mkdir" && inputArray.Count > 1)
//             {
//                 outputArray.Clear();
//                 CreateDirectory(inputArray[1]);
//             }
//             else if (inputArray[0] == "mkdir" && inputArray.Count <= 1)
//             {
//                 outputArray.Clear();
//                 outputArray.Add("mkdir: missing operand");
//             }

//             //+ Cd Command

//             else if (inputArray[0] == "cd" && inputArray.Count > 1) {
//                 outputArray.Clear();
//                 ChangeDirectory(inputArray[1]);
//             }
//             else if (inputArray[0] == "cd" && inputArray.Count <= 1) {
//                 outputArray.Clear();
//                 outputArray.Add("cd: missing operand");
//             }

//             //+ Pwd Command

//             else if (inputArray[0] == "pwd") {
//                 outputArray.Clear();
//                 PrintWorkingDirectory();
//             }

//             //+ Cat Command

//             else if (inputArray[0] == "cat" && inputArray.Count > 1) {
//                 outputArray.Clear();
//                 OpenFile(inputArray[1]);
//             }
//             else if (inputArray[0] == "cat" && inputArray.Count <= 1) {
//                 outputArray.Clear();
//                 outputArray.Add("cat: missing operand");
//             }

//             //+ Touch Command

//             else if (inputArray[0] == "touch" && inputArray.Count > 2) {
//                 outputArray.Clear();
//                 string fileContent = string.Join(" ", inputArray.GetRange(2, inputArray.Count - 2));
//                 currentDirectory.Files[inputArray[1]] = new Files(inputArray[1], fileContent);
//             }
//             else if (inputArray[0] == "touch" && inputArray.Count <= 1) {
//                 outputArray.Clear();
//                 outputArray.Add("touch: missing operand");
//             }
//             else if (inputArray[0] == "touch" && inputArray.Count <= 2) {
//                 outputArray.Clear();
//                 outputArray.Add("touch: missing file content");
//             }


//             //+ Clear Command

//             else if (inputArray[0] == "clear") //Done
//             {
//                 ClearTerminal();
//             }

//             //+ Exit Command

//             else if (inputArray[0] == "exit") //Done
//             {
//                 outputArray.Clear();
//                 Application.Quit();
//             }

//             //+ rm Command

//             else if (inputArray[0] == "rm" && inputArray.Count > 1) {
//                 outputArray.Clear();
//                 RemoveFile(inputArray[1]);
//             }
//             else if (inputArray[0] == "rm" && inputArray.Count <= 1) {
//                 outputArray.Clear();
//                 outputArray.Add("rm: missing operand");
//             }

//             //+ rmdir Command

//             else if (inputArray[0] == "rmdir" && inputArray.Count > 1)
//             {
//                 outputArray.Clear();
//                 RemoveDirectory(inputArray[1]);
//             }
//             else if (inputArray[0] == "rmdir" && inputArray.Count <= 1)
//             {
//                 outputArray.Clear();
//                 outputArray.Add("rmdir: missing operand");
//             }

//             //+ else
            
//             else
//             {
//                 outputArray.Clear();
//                 GameObject errorPanel = Instantiate(valueUserError, parentPanel);
//                 errorPanel.tag = "InstantiatedPanel";
//             }

//         }