var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.datntdev_Microservices_Identity_Web_Host>("svcs-identity-web-host");
builder.AddProject<Projects.datntdev_Microservices_Admin_Web_Host>("svcs-admin-web-host");

builder.Build().Run();
