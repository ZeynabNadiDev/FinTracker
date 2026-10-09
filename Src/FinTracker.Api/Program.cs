using Budget.Composition;
using Budget.Infrastructure.Persistence;
using Budget.Infrastructure.Persistence.DBcontext;
using Category.Composition;
using Category.Infrastructure.Persistence;
using Category.Infrastructure.Persistence.DBcontext;
using Identity.Composition;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.DBcontext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Notification.Composition;
using Notification.Infrastructure.Persistence.DBcontext;
using Report.Composition;
using System.Text;
using Transaction.Composition;
using Transaction.Infrastructure.Persistence;
using Transaction.Infrastructure.Persistence.DBcontext;
using Wallet.Composition;
using Wallet.Infrastructure.Persistence;
using Wallet.Infrastructure.Persistence.DBcontext;

var builder = WebApplication.CreateBuilder(args);

// Module Registrations
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddCategoryModule(builder.Configuration);
builder.Services.AddWalletModule(builder.Configuration);
builder.Services.AddTransactionModule(builder.Configuration);
builder.Services.AddBudgetModule(builder.Configuration);
builder.Services.AddReportModule(builder.Configuration);
builder.Services.AddNotificationModule(builder.Configuration);

// Authentication Setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
    };
});

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Identity.Presentation.ModuleReference).Assembly)
    .AddApplicationPart(typeof(Category.Presentation.ModuleReference).Assembly)
    .AddApplicationPart(typeof(Wallet.Presentation.ModuleReference).Assembly)
    .AddApplicationPart(typeof(Transaction.Presentation.Controller.TransactionsController).Assembly)
    .AddApplicationPart(typeof(Budget.Presentation.Controllers.BudgetsController).Assembly)
    .AddApplicationPart(typeof(Report.Presentation.ModuleReference).Assembly)
    .AddApplicationPart(typeof(Notification.Presentation.ModuleReference).Assembly);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FinTracker API", Version = "v1" });
    options.EnableAnnotations();
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.CustomSchemaIds(type => type.FullName);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Bearer token"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

// Apply pending migrations for all modules
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    void MigrateContext<TContext>(string moduleName) where TContext : DbContext
    {
        try
        {
            var db = services.GetRequiredService<TContext>();
            var pendingMigrations = db.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Any())
            {
                logger.LogInformation("Applying {Count} pending migrations for {Module}...", pendingMigrations.Count, moduleName);
                db.Database.Migrate();
                logger.LogInformation("Successfully migrated {Module}.", moduleName);
            }
            else
            {
                logger.LogInformation("No pending migrations for {Module}.", moduleName);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to apply migrations for {Module}.", moduleName);
            throw;
        }
    }

    try
    {
        MigrateContext<IdentityDbContext>("Identity");
        MigrateContext<CategoryDbContext>("Category");
        MigrateContext<WalletDbContext>("Wallet");
        MigrateContext<TransactionDbContext>("Transaction");
        MigrateContext<BudgetDbContext>("Budget");
        MigrateContext<NotificationDbContext>("Notification");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Fatal error during database initialization.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinTracker API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
