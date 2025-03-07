using Data.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

app.UseHsts();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Index}/{id?}")
    .WithStaticAssets();

//var AllowedConnections = "_allowedConnections";
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: AllowedConnections,
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:7003", "http://localhost:5173")
//                  .AllowAnyMethod()
//                  .AllowAnyHeader();
//        });
//});

app.Run();