using MDTPDQSync;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<syncWorker>();
    })
    .Build();

await host.RunAsync();
