using Microsoft.EntityFrameworkCore;
using MotoManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<MotoManagerContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MotoManagerContext")));

builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddCssBundle("/css/bundle.css", 
        "lib/bootstrap/dist/css/bootstrap.css",
        "css/all.min.css",
        "css/site.css");

    pipeline.AddJavaScriptBundle("/js/bundle.js", 
        "lib/jquery/dist/jquery.js", 
        "lib/bootstrap/dist/js/bootstrap.bundle.js", 
        "js/site.js");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseWebOptimizer();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();