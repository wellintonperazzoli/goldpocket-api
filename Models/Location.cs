using GoldPocket.Models.DB;
using System.ComponentModel.DataAnnotations;

namespace GoldPocket.Models
{
    public class Location : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must inform a name.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();

        [Display(Name = "Expenses")]
        public int expenseCount { get; set; } = 0;

        [Display(Name = "Items")]
        public int itemCount { get => Items.Count; }
        public bool hasExpense { get => expenseCount > 0; }
        public bool hasItem { get => itemCount > 0; }
        public bool hasRelation { get => hasExpense || hasItem; }
    }
}
