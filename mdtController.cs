using MDTPDQSync.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDTPDQSync
{
    internal class mdtController
    {
        private IConfiguration config;
        private const string mdtDriveName = "mdtShare";
        public mdtController(IConfiguration config)
        {
            this.config = config;
        }

        public void createApplication(List<Package> packages)
        {
            Process powershell = new Process();
            powershell.StartInfo.FileName = config.GetValue<string>("powershellPath");
            powershell.StartInfo.CreateNoWindow = true;
            powershell.StartInfo.RedirectStandardOutput = false;

            string powershellScript = createAddAppliactionString(packages);
            string tempPath = Path.GetTempFileName().Replace(".tmp", ".ps1");

            File.WriteAllText(tempPath, powershellScript);

            powershell.StartInfo.Arguments = ("-File " + $"{tempPath}");

            powershell.Start();
            powershell.WaitForExit();

            File.Delete(tempPath);
        }

        private string createAddAppliactionString(List<Package> applications)
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine($"Import-Module '{config.GetValue<string>("mdtPath")}'");
            script.AppendLine($"New-PSDrive -Name '{mdtDriveName}' -PSProvider MDTProvider -Root {config.GetValue<string>("mdtDeploymentShare")}");

            foreach (Package app in applications)
            {
                script.AppendLine(createSingleApplicationImportString(app.Name));
            }

            script.AppendLine($"Remove-PSDrive -Name {mdtDriveName}");
            return script.ToString();
        }

        private string createSingleApplicationImportString(string applicationName)
        {
            StringBuilder application = new StringBuilder();
            StringBuilder applicationInstall = new StringBuilder();

            application.Append("Import-MDTApplication ");
            application.Append($@"-Path '{mdtDriveName}:\Applications\{config.GetValue<string>("mdtApplicationFolder")}' ");
            application.Append("-Enable $true ");
            application.Append($"-Name '{applicationName}' ");
            application.Append($"-ShortName '{applicationName}' ");
            application.Append("-Reboot $false ");
            application.Append("-Hide $false ");

            applicationInstall.Append("-CommandLine ");
            applicationInstall.Append($"'cmd /c ");
            applicationInstall.Append(@"%SCRIPTROOT%\psexec.exe ");
            applicationInstall.Append($@"\\{config.GetValue<string>("pdqSever")} ");
            applicationInstall.Append($@"-u {config.GetValue<string>("domainName")}\{config.GetValue<string>("userName")} ");
            applicationInstall.Append($"-p {config.GetValue<string>("password")} ");
            applicationInstall.Append("-h -accepteula ");
            applicationInstall.Append('"');
            applicationInstall.Append(@"C:\Program Files (x86)\Admin Arsenal\PDQ Deploy\pdqdeploy.exe");
            applicationInstall.Append('"');
            applicationInstall.Append(" Deploy -Package ");
            applicationInstall.Append('"');
            applicationInstall.Append(applicationName);
            applicationInstall.Append('"');
            applicationInstall.Append(" -Targets %Computername%");
            applicationInstall.Append("'");

            application.Append(applicationInstall);
            application.Append(" -Nosource");

            return application.ToString();
        }
    }
}
