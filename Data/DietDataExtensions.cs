using draft_ml.Db;
using Pgvector.EntityFrameworkCore;

namespace draft_ml.Data
{
    public static class DietDataExtensions
    {
        public static async Task<Meal> GetMealAsync(this DietDbContext db, Vector vec)
        {
            return db.Meals.OrderBy(x => x.Nutrients.L2Distance(vec)).Take(1).Single();
        }

        public static async Task<Snack> GetSnackAsync(this DietDbContext db, Vector vec)
        {
            return default; /* db.Snacks
                .OrderBy(x => x.Nutrients.L2Distance(vec))
                .Take(1)
                .Single();*/
        }
    }
}
