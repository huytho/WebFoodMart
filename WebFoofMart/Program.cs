using Microsoft.EntityFrameworkCore;
using WebFoofMart.Models.DataModels;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FoodDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("sqlcon")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddSession(
        option =>
        {
            option.IdleTimeout = TimeSpan.FromMinutes(10);
            option.Cookie.HttpOnly = true;
            option.Cookie.Name = "FoodMart.session";
        }
        );

builder.Services.AddAuthentication("Foodschema").AddCookie("Foodschema", options => {
    //đường dẫn cấm truy cập
    //options.AccessDeniedPath = new PathString("/");
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = "FoodMart.cookie",
        Path = "/",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
    //phát sinh sự kiện
    //options.Events=new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    //{
    //    //OnSignedIn = context => { }
    //}

    //chỉ định đường dẫn login
    options.LoginPath = "/Admin/Home/Login";
    options.ReturnUrlParameter = "requestUrl";
    options.SlidingExpiration = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
