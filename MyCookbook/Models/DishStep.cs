namespace MyCookbook.API.Models
{
    public class DishStep
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int StepNumber { get; set; }
        public string Instruction { get; set; } = string.Empty;
    }
}
