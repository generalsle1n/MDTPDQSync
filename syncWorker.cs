using MDTPDQSync.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDTPDQSync
{
    internal class syncWorker : BackgroundService
    {
        private readonly ILogger<syncWorker> logger;
        const string configFileName = "settings.json";
        private IConfiguration config;
        private const long minimalFolderID = 12;
        private const string notInMDTPrefix = "__NotINMDT";

        private int syncTime;

        public syncWorker(ILogger<syncWorker> logger)
        {
            this.logger = logger;
            config = new ConfigurationBuilder()
                .AddJsonFile(configFileName, false)
                .AddEnvironmentVariables()
                .Build();

            syncTime = (config.GetValue<int>("syncTime")) * 60 * 1000;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                List<Package> allPackages = getAllPDQPackages();
                mdtController lol = new mdtController(config);
                logger.LogInformation("Start Application sync");
                lol.createApplication(allPackages);
                logger.LogInformation("Sync finished");
                await Task.Delay(30000, stoppingToken);
            }
        }

        private List<Package> getAllPDQPackages()
        {
            List<Package> packages = new List<Package>();

            using (DatabaseContext db = new DatabaseContext(config))
            {
                packages = db.Packages.Where((singlePackage) => singlePackage.FolderId >= minimalFolderID && !singlePackage.Description.StartsWith(notInMDTPrefix)).ToList();
            }

            return packages;
        }
    }
}
