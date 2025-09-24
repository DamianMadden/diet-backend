using System.ComponentModel.DataAnnotations.Schema;

namespace draft_ml.Db.Models
{
    public class Meal
    {
        public required Guid Id { get; set; }

        [Column(TypeName = "vector(3)")]
        public required Vector Nutrients { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }

    public class Snack : Meal
    {
        public Snack(Meal m)
        {
            Id = m.Id;
            Nutrients = m.Nutrients;
        }
        
        public float Quantity { get; set; }

        public Vector AppliedNutrients { get => Nutrients.Multiply(Quantity); }
    }
}
