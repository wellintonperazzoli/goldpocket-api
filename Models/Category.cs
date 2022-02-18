using GoldPocket.Enums;
using System.ComponentModel.DataAnnotations;

namespace GoldPocket.Models
{
    public class Category : BaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You must inform a name.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must inform a type.")]
        [Display(Name = "Type")]
        public CategoryTypes Type { get; set; }

        [Display(Name = "Type")]
        public string TypeName { get => Type.ToString(); }

        public List<Item> itemDetails {get; set;} = new List<Item>();
        public List<Expense> expenseDetails {get; set;} = new List<Expense>();
        

        [Display(Name = "Times Used")]
        public int Quantity { get => itemDetails.Count() + expenseDetails.Count(); }

        public bool hasRelation { get => Quantity > 0; }
    }
}
