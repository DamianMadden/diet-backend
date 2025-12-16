using System.Reflection;
using draft_ml.Db;

namespace draft_ml.Ingestion.Source.TheMealDb
{
    public class TheMealDb(DietDbContext dietDb, ITheMealDb theMealDbApi) : IIngestionSource
    {
        public async Task IngestAll(CancellationToken cancel)
        {
            // Prepare type information for iterating over ingredients and measures
            var ingredientProps = typeof(SearchResponse.MealResponse)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop =>
                    prop.Name.StartsWith("StrIngredient", StringComparison.OrdinalIgnoreCase)
                )
                .OrderByDescending(prop => prop.Name);

            var measureProps = typeof(SearchResponse.MealResponse)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop =>
                    prop.Name.StartsWith("StrMeasure", StringComparison.OrdinalIgnoreCase)
                )
                .OrderByDescending(prop => prop.Name);

            // Free key is limited to listing 100 items, but the api doesn't offer pagination
            // List meals sequentially by first letter until we have everything
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var response = await theMealDbApi.SearchMeal(
                    new SearchQueryParameters { FirstLetter = c.ToString() }
                );

                // Process response meals
                foreach (var meal in response.Meals)
                {
                    // Parse/lookup each ingredient
                    var ingredientCount = ingredientProps.Count();
                    for (int i = 0; i < ingredientCount; i++)
                    {
                        var ingredientName = (string?)ingredientProps.ElementAt(i).GetValue(meal);
                        if (string.IsNullOrEmpty(ingredientName)) { }
                    }

                    // TODO: FINISH THIS
                    Meal mealEntity = new()
                    {
                        Id = Guid.NewGuid(),
                        Nutrients = new Vector(new ReadOnlyMemory<float>([10.5f, 5.6f, 6.7f])),
                    };
                    dietDb.Meals.Add(mealEntity);
                }
            }
        }

        public Task IngestNew(DateTimeOffset lastRun, CancellationToken cancel)
        {
            // If the first run succeeded we don't need to rerun for now
            return Task.CompletedTask;
        }

        public string Key() => "themealdb";
    }
}
