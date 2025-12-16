using System.Text;
using draft_ml.Db;
using draft_ml.Ingestion;
using draft_ml.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using Npgsql;

namespace draft_ml
{
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

            services.AddOpenApi();

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
                    }
                );

                db.Meals.Add(
                    new Meal
                    {
                        Id = Guid.NewGuid(),
                        Nutrients = new Vector(new ReadOnlyMemory<float>([7.2f, 9.1f, 2.7f])),
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
                    }
                );

                db.Meals.Add(
                    new Meal
                    {
                        Id = Guid.NewGuid(),
                        Nutrients = new Vector(new ReadOnlyMemory<float>([0f, 1.2f, 0f])),
                    }
                );

                db.Meals.Add(
                    new Meal
                    {
                        Id = Guid.NewGuid(),
                        Nutrients = new Vector(new ReadOnlyMemory<float>([0f, 0f, 1.2f])),
                    }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}
