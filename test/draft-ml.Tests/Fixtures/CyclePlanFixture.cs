using draft_ml.Db;
using draft_ml.Db.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Testcontainers.PostgreSql;
using Xunit;

namespace draft_ml.UnitTests.Fixtures;

public class CyclePlanFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("pgvector/pgvector:0.8.1-pg18-trixie")
        .Build();

    public CyclePlan _cyclePlan;
    public DietDbContext _db;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var options = new DbContextOptionsBuilder<DietDbContext>()
            .UseNpgsql(_container.GetConnectionString(), opt => opt.UseVector())
            .Options;

        _db = new DietDbContext(options);
        await _db.Database.EnsureCreatedAsync();

        _db.Meals.Add(
            new Meal
            {
                Id = Guid.NewGuid(),
                Nutrients = new Vector(new ReadOnlyMemory<float>([7.2f, 9.1f, 2.7f])),
            }
        );
        await _db.SaveChangesAsync();

        _cyclePlan = new CyclePlan(_db);
    }

    public async Task DisposeAsync()
    {
        await _db.DisposeAsync();
        await _container.DisposeAsync();
    }
}
