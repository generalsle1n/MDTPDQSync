using MDTPDQSync;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<syncWorker>();
    })
    .UseWindowsService(options =>
    {
        options.ServiceName = "WehrleMDTPDQSync";
    })
    .Build();

await host.RunAsync();
