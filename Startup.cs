using draft_ml.Db;
using draft_ml.Ingestion;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "The API", Version = "v1" });
            });

            services.AddDbContext<DietDbContext>(opt => 
                opt.UseNpgsql("Server=127.0.0.1;Port=5432;Database=mealdb;User Id=postgres;Password=dheartj;", opt => {
                    opt.UseVector();
                })
            );

            if (bool.Parse(Configuration["EnableIngestion"] ?? "false"))
            {
                RecipeIngestion.ConfigureServices(services);
            }
        }

        public async void InitializeDb(IServiceProvider sp)
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<DietDbContext>();

            if (!db.Meals.Any())
            {
                // Insert test data
                db.Meals.Add(new Db.Models.Meal
                {
                    Id = 1,
                    Nutrients = new Vector(new ReadOnlyMemory<float>([10.5f, 5.6f, 6.7f]))
                });

                db.Meals.Add(new Db.Models.Meal
                {
                    Id = 2,
                    Nutrients = new Vector(new ReadOnlyMemory<float>([7.2f, 9.1f, 2.7f]))
                });

                await db.SaveChangesAsync();
            }

            if (db.Meals.Count() < 3)
            {
                db.Meals.Add(new Db.Models.Meal
                {
                    Id = 3,
                    Nutrients = new Vector(new ReadOnlyMemory<float>([1.2f, 0f, 0f]))
                });

                db.Meals.Add(new Db.Models.Meal
                {
                    Id = 4,
                    Nutrients = new Vector(new ReadOnlyMemory<float>([0f, 1.2f, 0f]))
                });

                db.Meals.Add(new Db.Models.Meal
                {
                    Id = 5,
                    Nutrients = new Vector(new ReadOnlyMemory<float>([0f, 0f, 1.2f]))
                });

                await db.SaveChangesAsync();
            }
        }
    }
}