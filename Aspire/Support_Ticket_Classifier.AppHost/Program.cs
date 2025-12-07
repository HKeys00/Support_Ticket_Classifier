var builder = DistributedApplication.CreateBuilder(args);

var model = builder.AddPythonApp("model", "../../Model", "./app/app.py");

var api = builder.AddProject<Projects.Api>("api")
    .WithReference(model)
    .WaitFor(model);

var client = builder.AddProject<Projects.Client>("client")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();