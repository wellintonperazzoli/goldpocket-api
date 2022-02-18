using GoldPocket.Enums;
using GoldPocket.Extensions;
using System.ComponentModel.DataAnnotations;

namespace GoldPocket.Models
{
    public class Expense : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must inform a location.")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "You must inform a category.")]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "You must inform at least one item.")]
        [Display(Name = "Items")]
        public List<expenseItem> expenseItems { get; set; } = new List<expenseItem>();

        [Required(ErrorMessage = "You must inform the date.")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime dateTime { get; set; }


        [Display(Name = "Description")]
        public string Description { get; set; } = "";

        [Display(Name = "Total Value")]
        public double totalValue { get; set; } = 0;

        [Display(Name = "Total")]
        public string totalValueText
        {
            get => totalValue.toCurrency();
        }


        public string Month { get => dateTime.ToString("MMM"); }
        public string Year { get => dateTime.ToString("yyyy"); }
    }
}
