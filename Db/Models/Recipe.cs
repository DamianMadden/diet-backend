namespace draft_ml.Db.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Ingredients = new List<string>();
            Measures = new List<string>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DrinkAlternate { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        public string Instructions { get; set; }
        public string MealThumb { get; set; }
        public string Tags { get; set; }
        public string Youtube { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Measures { get; set; }
        public string Source { get; set; }
        public string ImageSource { get; set; }
        public string CreativeCommonsConfirmed { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
