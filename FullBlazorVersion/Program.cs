using DiceRolls.Components;
using DiceRolls.Models;
using DiceRolls.Repositories.ThrowRepository;
using DiceRolls.Services.DataCleanup;
using DiceRolls.Services.ErrorMessage;
using DiceRolls.Services.JsDom;
using DiceRolls.Services.Randomize;
using DiceRolls.Services.SessionStorage;
using DiceRolls.Services.ThrowService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IRandomizeService, RandomizeService>();
builder.Services.AddScoped<SessionStorageService>();
builder.Services.AddScoped<JsDomService>();
builder.Services.AddScoped<IThrowRepository, ThrowRepository>();
builder.Services.AddScoped<IThrowService, ThrowService>();
builder.Services.AddSingleton<IErrorMessageService, ErrorMessageService>();

builder.Services.Configure<HostOptions>(x =>
{
    x.ServicesStartConcurrently = true;
});
builder.Services.AddHostedService<DataCleanupService>();

var dbPath = builder.Configuration.GetValue<string>("Database:Path");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();