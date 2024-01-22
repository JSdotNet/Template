var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Launcher>("launcher");

builder.Build().Run();
