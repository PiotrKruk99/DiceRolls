using DiceRolls.Components;
using DiceRolls.Models;
using DiceRolls.Repositories.Throw;
using DiceRolls.Services.Randomize;
using DiceRolls.Services.SessionStorage;
using DiceRolls.Services.Throw;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IRandomizeService, RandomizeService>();
builder.Services.AddScoped<SessionStorageService>();
builder.Services.AddScoped<IThrowRepository, ThrowRepository>();
builder.Services.AddScoped<IThrowService, ThrowService>();

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