using EmploymentManagementSystem.Data;
using EmploymentManagementSystem.Repositories;
using EmploymentManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// LOW COUPLING(GRASP) Supports Layered Architectural Design - DEPENDENCY INJECTION by Service Registration
builder.Services.AddScoped<IJobOpportunityRepository, JobOpportunityRepository>();
builder.Services.AddScoped<IJobOpportunityService, JobOpportunityService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=JobOpportunities}/{action=Index}/{id?}"
);

app.Run();
