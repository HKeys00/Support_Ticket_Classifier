using Client.Components;
using Client.Middleware;
using Client.Services;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient(ApiEndpoints.Client, client =>
{
    client.BaseAddress = new Uri("https://api");
});

builder.Services.AddHttpClient(ApiEndpoints.LongRunningClient, client =>
{
    client.BaseAddress = new Uri("https://api");
    client.Timeout = Timeout.InfiniteTimeSpan;

}).AddStandardResilienceHandler(options =>
{
    options.AttemptTimeout.Timeout = TimeSpan.FromMinutes(2);
    options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(6);
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(4);
});

builder.Services.AddSingleton<ToastService>();
builder.Services.AddScoped<CurrentTicketService>();
builder.Services.AddScoped<TicketDragService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<ModelService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseRequestResponseLogging();

app.Run();