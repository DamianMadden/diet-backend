using Microsoft.EntityFrameworkCore;

namespace draft_ml.Db;

public class DietDbContext(DbContextOptions<DietDbContext> opt) : DbContext(opt)
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<IngestionRecord> IngestionRecords { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<MealIngredient> MealIngredients { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanMeal> PlanMeals { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserExternalIdentity> UserExternalIdentities { get; set; }
    public DbSet<UserTagExclusion> UserTagExclusions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Extensions ---
        modelBuilder.HasPostgresExtension("vector");

        // --- Entities ---

        modelBuilder.Entity<Address>(e =>
        {
            e.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<IngestionRecord>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Ingredient>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Meal>(e =>
        {
            e.HasMany(e => e.Ingredients).WithMany().UsingEntity<MealIngredient>();
        });

        modelBuilder.Entity<MealIngredient>(e =>
        {
            e.HasKey(e => new { e.MealId, e.IngredientId });
        });

        modelBuilder.Entity<Plan>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasMany(e => e.Meals).WithMany().UsingEntity<PlanMeal>();
        });

        modelBuilder.Entity<PlanMeal>(e =>
        {
            e.HasKey(e => new { e.PlanId, e.MealId });
        });

        modelBuilder.Entity<Recipe>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Session>(e =>
        {
            e.HasKey(e => new { e.UserId, e.RefreshTokenHash });
        });

        modelBuilder.Entity<Tag>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasMany(e => e.Addresses).WithOne(e => e.User).HasForeignKey(e => e.UserId);

            e.HasMany(e => e.ExternalIdentities).WithOne(e => e.User).HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<UserExternalIdentity>(e =>
        {
            e.HasKey(e => new { e.UserId, e.IdentityProvider });
        });

        modelBuilder.Entity<UserPlan>(e =>
        {
            e.HasKey(e => new { e.UserId, e.PlanId });
        });

        modelBuilder.Entity<UserTagExclusion>(e =>
        {
            e.HasKey(e => new { e.UserId, e.TagId });

            e.HasOne(e => e.User).WithMany(e => e.Exclusions).HasForeignKey(e => e.UserId);

            e.HasOne(e => e.Tag).WithMany().HasForeignKey(e => e.TagId);
        });
    }
}
