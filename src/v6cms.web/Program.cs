
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using v6cms.entities.db_set;
using v6cms.models.config;
using v6cms.services;
using v6cms.utils;
using v6cms.web;
using V6UEditor.Core;

var builder = WebApplication.CreateBuilder(args);

// 配置模版视图路径
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationExpanders.Add(new TemplateViewLocationExpander(builder.Configuration.GetSection("site_config:template_dir").Value));
});

// 注册生成静态页面
builder.Services.AddTransient<ViewResultExecutor>();

// 基于文件系统的密钥存储库（持久性保持密钥）
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("login-keys"));

// 解决中文被编码（valu添加）
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});

// 添加编辑器
builder.Services.AddUEditorService(configFileRelativePath: "../ueditor.json", basePath: Path.Combine("wwwroot"));

//添加options
builder.Services.AddOptions();
builder.Services.Configure<site_config_model>(builder.Configuration.GetSection("site_config"));

// EF数据库（valu添加）
var sqlConnection = builder.Configuration.GetConnectionString("SqlServerConnection");
builder.Services.AddDbContext<db_context>(option => option.UseSqlServer(sqlConnection));

// SSCMS外挂
var sqlConnection_sscms = builder.Configuration.GetConnectionString("SqlServerConnection_sscms");
builder.Services.AddDbContext<sscms_db_context>(option => option.UseSqlServer(sqlConnection_sscms));

// 跨域服务注册
builder.Services.AddCors(m => m.AddPolicy("any", a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// 注册Cookie认证服务
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/v6admin/login";
    options.LogoutPath = "/v6admin/loginout";
});

//注册自定义cookie服务
builder.Services.AddTransient<ICookie, cookie_util>();

//获取客户端ip
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//注册数据服务
builder.Services.AddTransient<IDataService, data_service>();

//使用动态路由
builder.Services.AddScoped<SlugRouteValueTransformer>();

//添加页面缓存
builder.Services.AddResponseCaching();

// 禁止Views生成dll(valu添加，注意：nuget引用Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation)
// <RazorCompileOnBuild>false</RazorCompileOnBuild>
// <RazorCompileOnPublish>false</RazorCompileOnPublish>
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(options =>
{
    // 忽略属性的循环引用
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
});

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

// 添加页面缓存
app.UseResponseCaching();

// 添加认证授权（顺序不可以颠倒）
app.UseAuthentication();

app.UseAuthorization();

// 跨域
app.UseCors();

// 区域路由
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=home}/{action=index}/{id?}"
);

// 使用动态路由
app.MapDynamicControllerRoute<SlugRouteValueTransformer>("/{**slug}");

// 默认路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");

app.Run();
