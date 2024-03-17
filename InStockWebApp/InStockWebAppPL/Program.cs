using Hangfire;
using InStockWebAppBLL;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppBLL.Helpers.Account;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using InStockWebAppPL.Hubs;
using InStockWebAppPL.Settings;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubcategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IItemRepository,ItemRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddControllersWithViews();
//builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)));

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
builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
{
    options.ClientId = "f2b08cb4-707b-418a-9f66-cabd363fee14";
    options.ClientSecret = "L2C8Q~Xnf2pJcX-z7EJRZox.qvhq0P9N6Pd9-adw";
});

//builder.Services.AddAuthentication().AddFacebook(options =>
//{
//    options.AppId = "764210835665367";
//    options.AppSecret = "2f6a335d944293a56ba30909ecc82c8a";
//});

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "1839893246447698";
    options.AppSecret = "37d901b716c469a8862d6a11d922fcce";
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
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.UseHangfireDashboard("/HangFire");
app.MapControllerRoute(
    name: "default",

    pattern: "{controller=Home}/{action=Index}/{id?}");


    //pattern: "{controller=FilterProduct}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/Hubs/ChatHub");

app.Run();