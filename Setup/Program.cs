using CardGame.Controllers;
using CardGame.Controllers.MatchFolder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CardGame.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using CardGame.Areas.Identity.Data;

namespace CardGame
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("CardGameContextConnection");
            builder.Services.AddDbContext<CardGameContext>(options =>
    options.UseSqlServer(
       connectionString,
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CardGameContext>().AddRoles<IdentityRole>();





            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            //add default identity maakt volledige implementatie van inlog en uitlog en beheer van accounts aan;
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();

            builder.Services.AddHsts(options =>
                {
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(365);
                });
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;


            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });










            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHub<MatchHub>("/matchHub");

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "User" };
                foreach (var r in roles)
                {
                    if (!await roleManager.RoleExistsAsync(r))
                    {
                        await roleManager.CreateAsync(new IdentityRole(r));
                    }

                }

                var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string adminEmail = "admin@admin.com";
                string adminPassword = "Pasword@1234";
                if (await usermanager.FindByEmailAsync(adminEmail) == null)
                {
                    var user = new IdentityUser();
                    user.Email = adminEmail;
                    user.EmailConfirmed = true;
                    await usermanager.CreateAsync(user, adminPassword);

                   await usermanager.AddToRoleAsync(user, "Admin");
                }

            }
            app.Run();
        }
    }
}