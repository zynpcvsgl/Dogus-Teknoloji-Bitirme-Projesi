using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;
using BlogApp.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using BlogApp.Data.Abstract;

var builder = WebApplication.CreateBuilder(args);

// DbContext yapılandırması
builder.Services.AddDbContext<BlogContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Sql_connection"))); // Sqlite bağlantısı

// Identity yapılandırması
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<BlogContext>()
    .AddDefaultTokenProviders();

// Repository'leri ekle
builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

// Authentication yapılandırması
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Users/AccessDenied";
        options.LoginPath = "/Users/Login";
        options.LogoutPath = "/Users/Logout";
    });

// Authorization yapılandırması
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Statik dosyaları servis et
app.UseStaticFiles();

// Middleware sıralaması
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Varsayılan yönlendirme
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");

// Veritabanı seed işlemleri
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BlogContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var seedData = new SeedData(context, userManager, roleManager);
    await seedData.SeedAsync();
}

app.Run();
