using Refit;

namespace draft_ml.Ingestion.Source.TheMealDb
{
    public class SearchQueryParameters
    {
        [AliasAs("s")]
        public string? MealName { get; set; }

        [AliasAs("f")]
        public string? FirstLetter { get; set; }
    }

    public class FilterQueryParameters
    {
        [AliasAs("i")]
        public string? Ingredient { get; set; }

        [AliasAs("c")]
        public string? Category { get; set; }

        [AliasAs("a")]
        public string? Area { get; set; }
    }

    public class SearchResponse
    {
        public MealResponse[] Meals { get; set; }

        public class MealResponse
        {
            public DateTime? DateModified { get; set; }
            public string IdMeal { get; set; }
            public string StrArea { get; set; }
            public string StrCategory { get; set; }
            public string StrCreativeCommonsConfirmed { get; set; }
            public string StrImageSource { get; set; }
            public string? StrIngredient1 { get; set; }
            public string? StrIngredient2 { get; set; }
            public string? StrIngredient3 { get; set; }
            public string? StrIngredient4 { get; set; }
            public string? StrIngredient5 { get; set; }
            public string? StrIngredient6 { get; set; }
            public string? StrIngredient7 { get; set; }
            public string? StrIngredient8 { get; set; }
            public string? StrIngredient9 { get; set; }
            public string? StrIngredient10 { get; set; }
            public string? StrIngredient11 { get; set; }
            public string? StrIngredient12 { get; set; }
            public string? StrIngredient13 { get; set; }
            public string? StrIngredient14 { get; set; }
            public string? StrIngredient15 { get; set; }
            public string? StrIngredient16 { get; set; }
            public string? StrIngredient17 { get; set; }
            public string? StrIngredient18 { get; set; }
            public string? StrIngredient19 { get; set; }
            public string? StrIngredient20 { get; set; }
            public string? StrInstructions { get; set; }
            public string StrMeal { get; set; }
            public string StrMealAlternate { get; set; }
            public string StrMealThumb { get; set; }
            public string? StrMeasure1 { get; set; }
            public string? StrMeasure2 { get; set; }
            public string? StrMeasure3 { get; set; }
            public string? StrMeasure4 { get; set; }
            public string? StrMeasure5 { get; set; }
            public string? StrMeasure6 { get; set; }
            public string? StrMeasure7 { get; set; }
            public string? StrMeasure8 { get; set; }
            public string? StrMeasure9 { get; set; }
            public string? StrMeasure10 { get; set; }
            public string? StrMeasure11 { get; set; }
            public string? StrMeasure12 { get; set; }
            public string? StrMeasure13 { get; set; }
            public string? StrMeasure14 { get; set; }
            public string? StrMeasure15 { get; set; }
            public string? StrMeasure16 { get; set; }
            public string? StrMeasure17 { get; set; }
            public string? StrMeasure18 { get; set; }
            public string? StrMeasure19 { get; set; }
            public string? StrMeasure20 { get; set; }
            public string StrSource { get; set; }
            public string StrTags { get; set; }
            public string StrYoutube { get; set; }
        }
    }

    public class FilterResponse
    {
        public MealResponse[] Meals { get; set; }

        public class MealResponse
        {
            public string IdMeal { get; set; }
            public string StrMeal { get; set; }
            public string StrMealThumb { get; set; }
        }
    }

    public class ListResponse
    {
        public MealResponse[] Meals { get; set; }

        public class MealResponse
        {
            public string StrArea { get; set; }
            public string StrCategory { get; set; }
            public string StrIngredient { get; set; }
        }
    }
    
    public interface ITheMealDb
    {
        [Get("search.php")]
        public Task<SearchResponse> SearchMeal([Query] SearchQueryParameters parameters);

        [Get("lookup.php")]
        public Task<SearchResponse> LookupMeal([Query][AliasAs("i")] string id);

        [Get("filter.php")]
        public Task<FilterResponse> FilterMeal([Query] FilterQueryParameters parameters);


        [Get("list.php?c=list")]
        public Task<ListResponse> ListCategories();

        [Get("list.php?a=list")]
        public Task<ListResponse> ListAreas();

        [Get("list.php?i=list")]
        public Task<ListResponse> ListIngredients();


        // Latest - Premium only
        [Get("latest.php")]
        public Task<SearchResponse> LatestMeals();
    }
}
