var builder = DistributedApplication.CreateBuilder(args);


var rabbit = builder.AddRabbitMQ("messaging");
var model = builder.AddPythonApp("ml-model", "../../Model", "./app/app.py")
    .WithReference(rabbit)
    .WaitFor(rabbit);

var api = builder.AddProject<Projects.Api>("api")
    .WithReference(model)
    .WithReference(rabbit)
    .WaitFor(model);

builder.AddProject<Projects.Client>("client")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();