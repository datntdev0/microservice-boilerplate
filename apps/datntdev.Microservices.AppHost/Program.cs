var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.datntdev_Microservice_Identity_Web_Host>("svcs-identity-web-host");

builder.Build().Run();
