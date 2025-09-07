using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Client.Pages;
using BlazorApp.Components;
using BlazorApp.Components.Account;
using BlazorApp.Data;
using BlazorApp.Infrastructure;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddControllers();
builder.Services.AddScoped(sp => 
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient 
    { 
        BaseAddress = new Uri(navigationManager.BaseUri) 
    };
});

builder.Services.AddHttpClient();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
builder.Services.AddAuthorization();

// --- START: DATABASE CONFIGURATION CHANGES ---

// connection strings from appsettings.json
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var applicationConnectionString = builder.Configuration.GetConnectionString("ApplicationConnection") 
    ?? throw new InvalidOperationException("Connection string 'ApplicationConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(applicationConnectionString));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(defaultConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(BlazorApp.Application.Products.Queries.GetProductsQuery).Assembly));


// --- END: DATABASE CONFIGURATION CHANGES ---

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp.Client._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();
app.MapControllers();

// --- START: AUTOMATIC MIGRATIONS ---

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await appDbContext.Database.MigrateAsync();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await applicationDbContext.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// --- END: AUTOMATIC MIGRATIONS ---


app.Run();