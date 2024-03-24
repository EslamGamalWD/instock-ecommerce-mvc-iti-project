using Hangfire;
using InStockWebAppBLL;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppBLL.Helpers.Account;
using InStockWebAppPL.Resources;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using InStockWebAppPL.Hubs;
using InStockWebAppPL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddControllersWithViews()
.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
.AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(SharedResources));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.LoginPath = new PathString("/Account/Login");
            options.AccessDeniedPath = new PathString("/Error/Error");
        });


builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);


// Password and user name configuration
builder.Services.AddBusinessLogicLayerDependencies();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Default Password settings.
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
}).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();


//External Logins
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "794925340425-7ikn9cakv7tsef37g3ghqfbr9hc3do7l.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
{
    options.ClientId = "f2b08cb4-707b-418a-9f66-cabd363fee14";
    options.ClientSecret = "L2C8Q~Xnf2pJcX-z7EJRZox.qvhq0P9N6Pd9-adw";
});

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "425624396646148";
    options.AppSecret = "ed20e058d11c948bdc0c90eb635f4570";
    
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();


builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
   

}
// Configure supported cultures
var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("ar")
};
// Add localization middleware
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
                {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider()
                }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//var requestlocalizationOptions = app.Services.GetServices<IOptions<RequestLocalizationOptions>>();
app.UseResponseCaching();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.UseHangfireDashboard("/HangFire");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/Hubs/ChatHub");
app.MapHub<UserCount>("/Hubs/UserCount");

app.Run();