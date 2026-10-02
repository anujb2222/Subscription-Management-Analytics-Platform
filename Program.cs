using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Sublytic.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// Data Protection
// ---------------------------------------------------------

var dataDir = Path.Combine(
builder.Environment.ContentRootPath,
"App_Data"
);

Directory.CreateDirectory(dataDir);

var keysDir = Path.Combine(dataDir, "dpkeys");

Directory.CreateDirectory(keysDir);

builder.Services
.AddDataProtection()
.PersistKeysToFileSystem(new DirectoryInfo(keysDir))
.SetApplicationName("Sublytic");

// ---------------------------------------------------------
// MVC
// ---------------------------------------------------------

builder.Services
.AddControllersWithViews()
.AddNewtonsoftJson(options =>
{
options.SerializerSettings.ReferenceLoopHandling =
Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// ---------------------------------------------------------
// SQL Server
// ---------------------------------------------------------

builder.Services.AddDbContext<SublyticDbContext>(options =>
{
options.UseSqlServer(
builder.Configuration.GetConnectionString(
"SublyticDbContext"
)
);
});

// ---------------------------------------------------------
// ASP.NET Core Identity
// ---------------------------------------------------------

builder.Services
.AddIdentity<IdentityUser, IdentityRole>(options =>
{
options.SignIn.RequireConfirmedAccount = false;

    // Password: numbers only, minimum 6 digits
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<SublyticDbContext>()
.AddDefaultTokenProviders();


// ---------------------------------------------------------
// Cookie settings
// ---------------------------------------------------------

builder.Services.ConfigureApplicationCookie(options =>
{
options.LoginPath = "/Account/Login";
options.AccessDeniedPath = "/Account/AccessDenied";
});

// ---------------------------------------------------------
// Build application
// ---------------------------------------------------------

var app = builder.Build();

// ---------------------------------------------------------
// Middleware
// ---------------------------------------------------------

if (!app.Environment.IsDevelopment())
{
app.UseExceptionHandler("/Home/Error");
}
else
{
app.UseDeveloperExceptionPage();
}

app.UseStaticFiles(new StaticFileOptions
{
FileProvider = new PhysicalFileProvider(
Path.Combine(
builder.Environment.ContentRootPath,
"Content"
)
),
RequestPath = "/Content"
});

app.UseStaticFiles(new StaticFileOptions
{
FileProvider = new PhysicalFileProvider(
Path.Combine(
builder.Environment.ContentRootPath,
"Scripts"
)
),
RequestPath = "/Scripts"
});

app.UseRouting();

// ---------------------------------------------------------
// Authentication & Authorization
// ---------------------------------------------------------

app.UseAuthentication();
app.UseAuthorization();

// ---------------------------------------------------------
// MVC routes
// ---------------------------------------------------------

app.MapControllerRoute(
name: "default",
pattern: "{controller=Account}/{action=Login}/{id?}"
);

// ---------------------------------------------------------
// Port
// ---------------------------------------------------------

app.Urls.Add("http://localhost:5050");

// ---------------------------------------------------------
// Seed database
// ---------------------------------------------------------

using (var scope = app.Services.CreateScope())
{
var db = scope.ServiceProvider
.GetRequiredService<SublyticDbContext>();

SublyticInitializer.Seed(db);


}

// ---------------------------------------------------------
// Run
// ---------------------------------------------------------

app.Run();