using Client.Components;
using Client.Middleware;
using Client.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://api");
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