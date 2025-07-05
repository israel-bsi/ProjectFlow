var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ProjectFlow_ApiService>("apiservice");

builder.AddProject<Projects.ProjectFlow_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
