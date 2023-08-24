var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configure cookie authentication for MVC
builder.Services.AddAuthentication("AccountCookie").AddCookie("AccountCookie", opt =>
{
    opt.Cookie.Name = "AccountCookie";
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
});

//Configure HTTP Client
builder.Services.AddHttpClient("api", (client) =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("API_URL")!);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseExceptionHandler("/Error/Index");

app.UseHsts();

app.UseStatusCodePagesWithReExecute("/Account/PageNotFound");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
