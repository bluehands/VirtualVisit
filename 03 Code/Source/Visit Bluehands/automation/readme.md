How to use the copy-to-mac script:<br>
These scripts are there to simplify the process of building for ios with unity on windows, and then moving the project to the remote mac to build and run with xcode there.<br>
To use, please specify in windows the environment variable 'RemoteMacUser' to the user name on the remote Mac mini.<br>
To simplify and not having to log in, create an ssh key with ssh-keygen in the git bash. add the public key to the remote mac.<br>
Then just add the location of the git-bash.exe to your path, or add a shortcut to it to a folder that is in the path (and include .lnk in the pathext variable).<br>
now run the copy-to-mac.bat by double-clicking, after building with unity
