using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusiBuy.Common.DB;
using MusiBuy.Common.AppConfig;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Repositories;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(prefix: "/")
    .Build();
#region Get value from appsettings.json file
var appSettingsSection = config.GetSection("AppSettings");
builder.Services.Configure<AppConfig>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppConfig>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

var authSection = config.GetSection("AuthDetail");
builder.Services.Configure<AuthDetailConfig>(authSection);
#endregion

// Add services to the container.
builder.Services.AddDbContext<MusiBuyDB_Connection>(option => option.UseSqlServer(config.GetConnectionString("MusiBuyDB_Connection"), sqlServerOptions => sqlServerOptions.CommandTimeout(120)));
builder.Services.AddScoped<IAuthenticate, AuthenticationRepository>();
builder.Services.AddScoped<IFrontUser, FrontUserRepository>();
builder.Services.AddScoped<ITemplate, TemplateRepository>();
builder.Services.AddScoped<ICommonSetting, CommonSettingRepository>();
builder.Services.AddScoped<IDropdown, DropdownRepository>();
builder.Services.AddScoped<ICreatores, CreatoresRepository>();
builder.Services.AddScoped<IPostManagement, PostManagementRepository>();
builder.Services.AddScoped<IEventManagement, EventManagementRepository>();
builder.Services.AddScoped<IContentManagement, ContentManagementRepository>();
builder.Services.AddScoped<ISocialEngagement, SocialEngagementRepository>();
builder.Services.AddScoped<IBank, BankRepository>();
builder.Services.AddScoped<ITransaction, TransactionRepsitory>();
builder.Services.AddScoped<IContactUs, ContactUsRepository>();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "MyAuthServer",
            ValidAudience = "MyWebAPI",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("keyabcdefghijklmnopkeyabcdefghijklmnop"))
        };
    });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference(opt =>
{
    opt.WithTitle("MusiBuy APIs");
    opt.WithTheme(ScalarTheme.BluePlanet);
    opt.Servers = new[]
    {
        new ScalarServer("/")
    };
});
app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar", permanent: false);
    return Task.CompletedTask;
});
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapControllers();

app.Run();
