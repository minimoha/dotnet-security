using Microsoft.AspNetCore.Authorization;
using WebApp.Pages.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.Cookie.Name = "MyCookieAuth";
    options.ExpireTimeSpan = TimeSpan.FromSeconds(60);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HumanResource",
        policy => policy.RequireClaim("Department", "HR")
        .RequireClaim("Manager")
        //.RequireClaim("EmployementDate", "2023-09-09")
        .Requirements.Add(new HRManagerProbationRequirement(3))
        );
});

builder.Services.AddSingleton <IAuthorizationHandler, HRManagerProbationRequirementHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

