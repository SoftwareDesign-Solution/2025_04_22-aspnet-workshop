using InventoryManagement.Mvc.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

/*
// Variante 1
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
*/

// Variante 2
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();


    var adminRoleCheck = await roleManager.RoleExistsAsync("Admin");

    if (!adminRoleCheck)
    { 
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    var userRoleCheck = await roleManager.RoleExistsAsync("User");

    if (!userRoleCheck)
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }


    IdentityUser? admin = await userManager.FindByEmailAsync("admin@admin.de");

    if (admin == null)
    {
        admin = new IdentityUser
        {
            UserName = "admin@admin.de",
            Email = "admin@admin.de",
            EmailConfirmed = true,
        };

        var createAdminResult = await userManager.CreateAsync(admin, "Admin123!");

        if (createAdminResult.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
            //await userManager.AddToRolesAsync(admin, new List<string>{"User", "Admin"});
        }

    }

    IdentityUser? user = await userManager.FindByEmailAsync("user@user.de");

    if (user == null)
    {
        user = new IdentityUser
        {
            UserName = "user@user.de",
            Email = "user@user.de",
            EmailConfirmed = true,
        };

        var createUserResult = await userManager.CreateAsync(user, "User123!");

        if (createUserResult.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "User");
        }
    }
}

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

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Items}/{action=Overview}/{id?}");

app.Run();
