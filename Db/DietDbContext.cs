using draft_ml.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace draft_ml.Db
{
    public class DietDbContext : DbContext
    {
        public DietDbContext(DbContextOptions<DietDbContext> opt) : base(opt)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealIngredient> MealIngredients { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanMeal> PlanMeals { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTagExclusion> UserTagExclusions { get; set; }


        public DbSet<IngestionRecord> IngestionRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Map entity relationships

            modelBuilder.HasPostgresExtension("vector");
            modelBuilder.Entity<Meal>()
                .HasIndex(i => i.Nutrients);
                //.HasMethod("hnsw")
                //.HasOperators("vector_l2_ops")
                //.HasStorageParameter("m", 16)
                //.HasStorageParameter("ef_construction", 64);

                //modelBuilder.Entity<Snack>()
                //    .HasIndex(i => i.Nutrients);

            modelBuilder.Entity<IngestionRecord>();
        }
    }
}
