using GoldPocket.Enums;
using GoldPocket.Extensions;
using GoldPocket.Models.DB;
using System.ComponentModel.DataAnnotations;

namespace GoldPocket.Models
{
    public class expenseItem : BaseModel
    {
        public int Id { get; set; }
        public int itemId { get; set; }

        [Required(ErrorMessage = "You must inform a name.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must inform a category.")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "You must inform a measure type.")]
        [Display(Name = "Measure Type")]
        public MeasureTypes MeasureType { get; set; } = 0;

        [Required(ErrorMessage = "You must inform a quantity.")]
        [Display(Name = "Quantity")]
        public double Quantity { get; set; } = 0;

        [Required(ErrorMessage = "You must inform a price.")]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "unitPrice")]
        public double unitPrice { get; set; } = 0;

        [Display(Name = "Total Price")]
        public double total { get; set; } = 0;
    
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime dateTime { get; set; } = DateTime.Now;

        [Display(Name = "Location")]
        public string LocationName { get; set; } = "";

        public int expenseId { get; set; } = 0;


        [Display(Name = "Item Category")]
        public string itemCategory { get; set; } = "";



        
        #region displayOnlyData
        
        [Display(Name = "unitPrice")]
        public string PricePerUnitText { get => unitPrice.toCurrency() + " / " + MeasureType; }

        [Display(Name = "Total Price")]
        public string totalPriceText { get => total.toCurrency(); }

        #endregion
    }
}
