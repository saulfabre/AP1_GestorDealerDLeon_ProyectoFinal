using AP1_GestorDealer_ProyectoFinal.Components;
using DLeon.Data.Context;
using DLeon.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<VehiculosServices>();
builder.Services.AddScoped<ModelosServices>();

builder.Services.AddDbContextFactory<Contexto>(
    o => o.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlConStr")));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value?.EndsWith(".avif", StringComparison.OrdinalIgnoreCase) == true)
    {
        context.Response.OnStarting(() =>
        {
            if (context.Response.StatusCode == StatusCodes.Status200OK)
            {
                context.Response.ContentType = "image/avif";
            }

            return Task.CompletedTask;
        });
    }

    await next();
});
app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
