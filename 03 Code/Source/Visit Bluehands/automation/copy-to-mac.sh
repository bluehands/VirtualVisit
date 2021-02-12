cd ../bin/iOS
scp -r * $RemoteMacUser@192.168.1.136:"/Users/Shared/Visit\\ Bluehands/bin"
ssh $RemoteMacUser@192.168.1.136 "cd /Users/Shared/Visit\ Bluehands/bin; ls *.sh | xargs chmod +x"