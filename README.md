This is a try to make a terminal emulator like thing. This was initially indented to work as an asset in Unity Asset Store, but this should work as a standalone app too. 



This has the following commands implemented:
help
man
pwd
cd
ls
mkdir
rmdir
touch
rm
cat
echo
clear
whoami
date
uptime
user (to change user. For admin access ans stuff. name it "Admin" for admin access. "user" for no admin access)
exit (Works only in the unity game, exits the game)

NOTE that (I forgot, I'll come back to it)

all the logic is in the file "Terminalv3.cs". The file Directory implements the "Directory.cs" system.
Most of the command logic is under the function Printer in "Terminalv3.cs"

Commands i am working on (Currently)
The filter system thing (What is the technical name for it?) like ls -l or ls - a.
file editing. (can only create the file with the text. Building a texteditor kind of layout)

Future plans
I/O Redirections
Piping
A custom programing language (Hopefully)

Long Term Idea
To build a working UI (Like an working OS) on top of this terminal system.

NOTE: Due to some error/problem with the .gitignore, I had ro delete the Terminalv1. everything imported to Terminalv2 now.

Images


![Screenshot 2024-07-09 002109](https://github.com/user-attachments/assets/e6273bc7-475f-4d52-bf64-e0d894733634)
![Screenshot 2024-07-09 002242](https://github.com/user-attachments/assets/3b94c075-7612-4662-8acf-ba597a55579c)
![Screenshot 2024-07-09 002410](https://github.com/user-attachments/assets/7d78de89-8f8a-4b1c-87fc-144d50259fde)
![Screenshot 2024-07-09 002519](https://github.com/user-attachments/assets/cb2f6c6a-8aa6-4359-aaa2-2a1b604e90b5)
![Screenshot 2024-07-09 002547](https://github.com/user-attachments/assets/45415889-9948-4720-b2eb-eb9ed307ebf0)
![Screenshot 2024-07-09 002651](https://github.com/user-attachments/assets/57461cf1-7cb1-400c-becb-9cf569e41afd)
![Screenshot 2024-07-09 002902](https://github.com/user-attachments/assets/3a1504ed-e4e4-4283-9883-53ce9162edd6)
![Screenshot 2024-07-09 003017](https://github.com/user-attachments/assets/f2c67b5f-9937-45fa-a997-597fa09b716c)
