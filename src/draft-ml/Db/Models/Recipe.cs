namespace draft_ml.Db.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DrinkAlternate { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string MealThumb { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string Youtube { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new List<string>();
        public List<string> Measures { get; set; } = new List<string>();
        public string Source { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public string CreativeCommonsConfirmed { get; set; } = string.Empty;
        public DateTime? DateModified { get; set; }
    }
}
