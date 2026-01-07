using draft_ml.Db;
using draft_ml.Exceptions;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace draft_ml.Data;

public static class DietDataExtensions
{
    public static async Task<Meal[]> GetMealAsync(
        this DietDbContext db,
        Vector vec,
        int count = 1,
        int page = 0
    )
    {
        return db.Meals.OrderBy(x => x.Nutrients.L2Distance(vec)).Take(count).ToArray();
    }

    public static async Task<Meal[]> GetSnackAsync(
        this DietDbContext db,
        Vector vec,
        int count = 1,
        int page = 0
    )
    {
        return await db
            .Meals.OrderByDescending(x => x.Nutrients.CosineDistance(vec))
            .Skip(page * count)
            .Take(count)
            .ToArrayAsync();
    }
}
