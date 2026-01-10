using System.Text;
using draft_ml.Db;
using draft_ml.Ingestion;
using draft_ml.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Npgsql;

namespace draft_ml;

public class Startup
{
    public IConfiguration Configuration { get; }
    public FeatureOptions FeatureOptions { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        FeatureOptions = new FeatureOptions();
        configuration.GetRequiredSection("Features").Bind(FeatureOptions);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        AddOptions(services);

        services.AddControllers().AddNewtonsoftJson();

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        services.AddDbContext<DietDbContext>(opt =>
            opt.UseNpgsql(
                Configuration["Database:ConnectionString"],
                opt =>
                {
                    opt.UseVector();
                }
            )
        );

        services.AddHttpClient();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddNLog();
        });

        services.AddSingleton<TokenService>();
        services.AddScoped<GoogleService>();

        if (bool.Parse(Configuration["EnableIngestion"] ?? "false"))
        {
            RecipeIngestion.ConfigureServices(services);
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://10.0.2.2:5180",
                    ValidAudience = "http://10.0.2.2:5180",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Authentication:SigningKey"]!)
                    ),
                };
            });
        services.AddAuthorization();
    }

    public void AddOptions(IServiceCollection services)
    {
        services
            .AddOptionsWithValidateOnStart<AuthOptions>()
            .Bind(Configuration.GetSection("Authentication"))
            .ValidateDataAnnotations();
    }

    public async void InitializeDb(IServiceProvider sp)
    {
        var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<DietDbContext>();

        await db.Database.MigrateAsync();

        if (!db.Meals.Any())
        {
            // Insert test data
            db.Meals.Add(
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Nutrients = new Vector(new ReadOnlyMemory<float>([10.5f, 5.6f, 6.7f])),
                    Name = "",
                    Description = "",
                    ThumbnailUrl = "",
                }
            );

            db.Meals.Add(
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Nutrients = new Vector(new ReadOnlyMemory<float>([7.2f, 9.1f, 2.7f])),
                    Name = "",
                    Description = "",
                    ThumbnailUrl = "",
                }
            );

            await db.SaveChangesAsync();
        }

        if (db.Meals.Count() < 3)
        {
            db.Meals.Add(
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Nutrients = new Vector(new ReadOnlyMemory<float>([1.2f, 0f, 0f])),
                    Name = "",
                    Description = "",
                    ThumbnailUrl = "",
                }
            );

            db.Meals.Add(
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Nutrients = new Vector(new ReadOnlyMemory<float>([0f, 1.2f, 0f])),
                    Name = "",
                    Description = "",
                    ThumbnailUrl = "",
                }
            );

            db.Meals.Add(
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Nutrients = new Vector(new ReadOnlyMemory<float>([0f, 0f, 1.2f])),
                    Name = "",
                    Description = "",
                    ThumbnailUrl = "",
                }
            );

            await db.SaveChangesAsync();
        }
    }
}

internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

    public BearerSecuritySchemeTransformer(
        IAuthenticationSchemeProvider authenticationSchemeProvider
    )
    {
        _authenticationSchemeProvider = authenticationSchemeProvider;
    }

    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        var authenticationSchemes = await _authenticationSchemeProvider.GetAllSchemesAsync();
        if (!authenticationSchemes.Any(a => a.Name == "Bearer"))
            return;

        var bearerScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme.",
        };

        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes.Add("Bearer", bearerScheme);

        var securityRequirement = new OpenApiSecurityRequirement
        {
            [
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme,
                    },
                }
            ] = [],
        };

        foreach (var path in document.Paths.Values)
        {
            foreach (var operation in path.Operations.Values)
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Security.Add(securityRequirement);
            }
        }
    }
}
