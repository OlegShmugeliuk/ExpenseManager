using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Data;
using ExpenseManager.Areas.Identity.Data;
using Service;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString)
);


//var connectionForData = builder.Configuration.GetConnectionString("DataContext") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");
//builder.Services.AddDbContext<AuthDbContext>(options =>
//    options.UseSqlServer(connectionForData)
//);


builder.Services.AddRazorPages();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});


builder.Services.AddDbContext<ExpenseManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AuthDbContext>();



var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Move UseAuthentication here

app.UseAuthorization();   // UseAuthorization should go between UseRouting() and UseEndpoints()

app.MapControllers();

app.MapRazorPages();

app.Run();