using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicBookStore.Data;
using MusicBookStore.Models;
using MusicBookStore.Services;
using Stripe;

public class Programs
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        //===============SERVICE CONFIGURATION ===============
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFie($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Programs>();
        }

        // ===================== DATABASE CONFIGURATION ================
        var identityConnection = builder.Configuration.GetConnectionSrting("IdentityConnection");
        var storeConnection = builder.Configuration.GetConnectionString("StoreConnction");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(identityConnection));

        builder.Services.AddDbContext<StoreDbContext>(options =>
            options.UseSqlServer(storeConnection,
            sqlOptions => sqlOptions.EnableRetyOnFailure()));


        // ==================== IDENTITY CONFIGURATION ======================
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Passwoord.RequireDigit = true;
            options.Password.RequiredLowercase = true;
            options.Passwoord.RequireUppercase = true;
            options.User.RequireUniqueEmail = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
            .AddRoles<IdentityRoles>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        // ================== AUTHENTICATION SERVICES ===================
        builder.Services.AddAuthentication()
           .AddGoogle(options =>
           {
               options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
               options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
           })
           .AddFacebook(options =>
           {
               options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
               options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
           });

        // ===================== APPLICATION SERVICES ==================
        builder.Services.AddScoped<IProductServive, ProductService>();
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IPaymentService, StripePaymentService>();
        builder.Services.AddScoped<IRmailService, SendGridEmailService>();

        //  Configuration Stripe
        StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

        // =================== SESSION & CACHE =====================
        builder.Services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(30);
        });

        builder.services.AddDistributedMemoryCache();

        // ===================MVC & RAZOR PAGES ===================
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });
        
        builder.Services.AddRazorPages()
            .AddRazorRuntimeCompilation();

        // =================== HEALTH CACHES ===================
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>()
            .AddDbContextCheck<StoreDbContext>();

        // =================== BUILD APPLICATION ====================
        var app = builder.Build();

        // =================== MIDDLEWARE PIPELINE ===================
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();

            // Seed initial data
            awaitSeedDatabaseAsync(app);
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Security & Performance
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Authentication & Session
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();

        app.UseRequestLocalization("en-us");
        
        app.MapControllerRoute(
            name: "area",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();
        app.MapHealthChecks("/health");

        // ====================== DATABASE SEEDING ==================
        async Task SeedDatabaseAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<Ilogger<Program>>();

            try
            {
                var identityContext = services.GetRequiredService<ApplicationDbContext>();
                await identityContext.Database.MigrateAsync();

                var storeContext = services.GetRequiredService<StoreDbContext>();
                await storeContext.Database.MigrationAsync();

                var userManger = services.GetRequiredService<userManger<ApplicationUser>>();
                var roleManager = services.GetRequiredService<roleManager<ApplicationRole>>();

                await SeedData.InitializeAsync(identityContext, userManger, roleManager);
                await SeedDatabaseAsync().SeedProductsAsync(storeContext);


                logger.LogInformation("Database seeding complete successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while seeding the database");
            }
        }

        app.Run();
    }
}

     
    
