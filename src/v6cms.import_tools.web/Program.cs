using Microsoft.EntityFrameworkCore;
using v6cms.entities.db_set;
using v6cms.mysql_entities.db_set;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<db_context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("v6cms_mssql_conn")));
builder.Services.AddDbContext<mysql_db_context>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("dedecms_mysql_conn")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");

app.Run();
