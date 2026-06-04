namespace MyCookbook.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property — each dish has many steps
        public List<DishStep> Steps { get; set; } = new();
    }
}
