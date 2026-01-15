using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using MusiBuy.Common.Interfaces.Marketing;
using MusiBuy.Common.Repositories.Marketing;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(prefix: "/")
    .Build();
// Add services to the container.
builder.Services.AddControllersWithViews();
Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.AddDbContext<MusiBuyDB_Connection>(option => option.UseSqlServer(config.GetConnectionString("MusiBuyDB_Connection")));
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IMarketingUsersLogin, MarketingUserLoginRepository>();
builder.Services.AddScoped<IRolePrivileges, RolePrivilegesRepository>();
builder.Services.AddScoped<ICommonSetting, CommonSettingRepository>();
builder.Services.AddScoped<IChangePassword, ChangePasswordRepository>();
builder.Services.AddScoped<IChangeProfile, ChangeProfileRepository>();
builder.Services.AddScoped<IRole, RoleRepository>();
builder.Services.AddScoped<IChangeProfile, ChangeProfileRepository>();
builder.Services.AddScoped<IRecipient, RecipientRepository>();
builder.Services.AddScoped<IContentManagement, ContentManagementRepository>();
builder.Services.AddScoped<ITemplate, TemplateRepository>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IMenu, MenuRepository>();
builder.Services.AddScoped<IEnum, EnumRepository>();
builder.Services.AddScoped<IClient, ClientRepository>();
builder.Services.AddScoped<ICreatores, CreatoresRepository>();
builder.Services.AddScoped<ICustomer, CustomerRepository>();
builder.Services.AddScoped<IPostManagement, PostManagementRepository>();
builder.Services.AddScoped<IEventManagement, EventManagementRepository>();
builder.Services.AddScoped<ICommentManagement, CommentManagementRepository>();
builder.Services.AddScoped<ITokenPlans, TokenPlansRepository>();
builder.Services.AddScoped<IStripeToken, StripeTokenRepository>();
builder.Services.AddScoped<IPaymentTransactionsRepository, PaymentTransactionsRepository>();
builder.Services.AddScoped<IGenres, GenresRepository>();
builder.Services.AddScoped<IDropdown, DropdownRepository>();
builder.Services.AddScoped<IInfluencerCategories, InfluencerCategoriesRepository>();
builder.Services.AddScoped<IProducerExpertise, ProducerExpertiseRepository>();
builder.Services.AddScoped<IPodcastGenres, PodcastGenresRepository>();
builder.Services.AddScoped<IMusicProducers, MusicProducersRepository>();

builder.Services.AddScoped<IMarketingResetPassword, MarketingResetPasswordRepository>();
builder.Services.AddScoped<IMarketingUser, MarketingUserRepository>();
builder.Services.AddScoped<IChangeMareketingProfile, ChangeMareketingProfileRepository>();
builder.Services.AddScoped<IMarketingChangePassword, ChangeMareketingPasswordRepository>();
builder.Services.AddKendo();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseStaticFiles();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
