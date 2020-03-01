# Chess Api


## Configuration
configuration is done in env.json file start by copying the appsettings and update it to suit your needs

```
cp appsettings.json env.json
```

## Running on ubuntu server
Install dotnet tools from 
https://docs.microsoft.com/en-us/dotnet/core/install/linux-package-manager-ubuntu-1904

go to the **$(SolutionDir)/ChessApi** Folder and install publish the package

```
dotnet publish --configuration Release
```

it should generate a executable for you in a folder that looks like **$(SolutionDir)/ChessApi/bin/Release/netcoreapp3.1/publish/ChessApi**


you can run it and it should listen on port 5000 and 5001 
``` 
cd ChessApi/bin/Release/netcoreapp3.1/publish/
./ChessApi
```

> :warning: **You have to start the application in the same folder otherwise it cannot find the settings**

### Start service
install supervisor to automatically start the dotnet service [Based on guide](https://www.digitalocean.com/community/tutorials/how-to-install-and-manage-supervisor-on-ubuntu-and-debian-vps)
```
apt-get install supervisor
```
create a file at location **/etc/supervisor/conf.d/chessApi.conf**
```
[program:ChessApi]
directory=$(SolutionDir)/ChessApi/bin/Release/netcoreapp3.1/publish
command=./ChessApi
autostart=true
autorestart=true
stderr_logfile=/var/log/chessApi.err.log
stdout_logfile=/var/log/chessApi.out.log
```
reload the supervisor configuration
```
sudo service supervisord reload
```

