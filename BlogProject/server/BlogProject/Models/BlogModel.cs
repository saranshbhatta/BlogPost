namespace BlogProject.Models
{
    public class BlogModel
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogDescription { get; set; }
        public string PrepTime { get; set; }
        public string CookTime { get; set; }
        public bool IsVegan { get; set; }
        public bool IsVeg { get; set; }
        public int MinimumServingSize { get; set; }
        public string Instruction { get; set; }
        public string PostedDate { get; set; }
        public string EditedDate { get; set; }
        public string EntryBy { get; set; }
        public List<CategoryModel> CategoriesList { get; set; }
        public List<IngredientsModel> IngredientsList { get; set; }
    }
}
