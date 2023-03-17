using CardGame.Controllers;
using CardGame.Controllers.MatchFolder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CardGame.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CardGameContextConnection") ?? throw new InvalidOperationException("Connection string 'CardGameContextConnection' not found.");

builder.Services.AddDbContext<IdentityDBContext>(options => options.UseSqlite(connectionString));



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
//add default identity maakt volledige implementatie van inlog en uitlog en beheer van accounts aan
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<IdentityDBContext>();
builder.Services.AddControllersWithViews();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<MatchHub>("/matchHub");
app.Run();
