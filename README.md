
# MDTPDQSync

This is a simple tool to sync packages from PDQ Deploy to MDT Applications, that the user can directly specify the apps on the installation process. This application is based on .Net 6 as worker service


## Authors

- [@Niels Schuler](https://github.com/generalsle1n)


## Deployment

1. Clone this project
2. Build this project as published
3. Copy the files to the mdt server e.g. "C:\Program Files\Wehrle\MDTPDQSync"
4. If there is already a sync installed, remove it: ```sc delete MDTPDQSync```
5. Install an new Service: ```sc create MDTPDQSync binPath="C:\Program Files\Wehrle\MDTPDQSync\MDTPDQSync.exe" start=delayed-auto```
6. Open the Windows Service Panel and go to Logon and set the account to Networkservice
7. Check if the Networkservice has permissions to access the database

## Settings.json

To run this project, you will need to add the following config variables to your settings.json file

`dbPath`: This is the path where the pdq deploy database is stored

`syncTime`: This specifies the amount of time in minutes that are waited between the sync cycles

`powershellPath`: The path to the powershell assembly

`mdtPath`: The path to the mdt helper module

`mdtDeploymentShare`: This is the path where the deployment share is hosted

`mdtApplicationFolder`: This is the folder where the applications should be created

`pdqServer`: The FQND from the pdq server which is used for the install string

`domainName`: The FQDN of the ldap domain also used for the install string

`userName`: The username which triggers the deployment

`password`: Password for the user



## Features
This service sync all Packages from PDQ to MDT.
All Applications are synced that are in a folder.
If an Application should not be synced you can exclude the packages by defining in the package description the "__NOTINMDT" as prefix
