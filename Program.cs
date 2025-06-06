using AppointmentScheduling.Data;
using AppointmentScheduling.Models;
using AppointmentScheduling.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<ApplicationUsers,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddTransient<IAppointmentService, AppointmentService>();
builder.Services.AddTransient<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options=> { 
    options.IdleTimeout= TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureApplicationCookie(options => options.AccessDeniedPath = new PathString("/Home/AccessDenied"));

var app = builder.Build();

 static async Task SeedAdmin(IServiceProvider serviceProvider)
{
    var roleManger = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManger = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var adminRoleExist = await roleManger.RoleExistsAsync("Admin");
    if (!adminRoleExist)
    {
        await roleManger.CreateAsync(new IdentityRole("Admin"));
    }
    var adminUsers = await userManger.GetUsersInRoleAsync("Admin");
    if (adminUsers == null|| !adminUsers.Any())
    {
        string adminEmail = "admin@gmal.com";
        string adminPassword = "Admin@12";
        string Name = "Main Admin";
        var adminUser=await userManger.FindByEmailAsync(adminEmail);
        if (adminUser==null)
        {
            var newAdmin = new IdentityUser
            {
                UserName = Name,
                Email=adminEmail,
                EmailConfirmed=true

            };
            var result = await userManger.CreateAsync(newAdmin, adminPassword);
            if (result.Succeeded)
            {
                await userManger.AddToRoleAsync(newAdmin, "Admin");
            }
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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
