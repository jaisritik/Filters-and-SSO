using Filters;
using Filters.ExceptionsMiddleware;
using Filters.Service;
using Filters.Service.FiltersService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add SSO Services
#region SSO Service
var directory = Path.Combine(Directory.GetCurrentDirectory(), "KeysToFile", "KeysFile");
if (!Directory.Exists(directory))
{
    Directory.CreateDirectory(directory);
}
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(directory))
    .SetApplicationName("sso");

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(c =>
    {
        c.Cookie.Name = "abcAuth";
        c.LoginPath = "/WeatherForecast/Index";
        c.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        c.SlidingExpiration = true;
        c.Cookie.Domain = "localhost";
    });

#endregion

// this Filter apply as a Globle lavel
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new MySampleActionFilter());
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();

app.MapControllers();

app.Run();
