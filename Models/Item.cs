using GoldPocket.Enums;
using GoldPocket.Extensions;
using GoldPocket.Models.DB;
using System.ComponentModel.DataAnnotations;

namespace GoldPocket.Models
{
    public class Item : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must inform a name.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must inform a category.")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "You must inform a measure type.")]
        [Display(Name = "Measure Type")]
        public MeasureTypes MeasureType { get; set; } = 0;

        public string MeasureTypeName { get => MeasureType.ToString(); }


        [Display(Name = "Recent Price Value")]
        public double recentPriceValue { get; set; } = 0;

        [Display(Name = "Recent Price Location")]
        public string recentPriceLocation { get; set; } = "";

        [Display(Name = "Average Price")]
        public double averagePriceValue { get; set; } = 0;

        public List<expenseItem> expenseItems { get; set; } = new List<expenseItem>();




        
        #region displayOnlyData

        [Display(Name = "Recent Price")]
        public string recentPriceText
        {
            get => $"{recentPriceLocation} {recentPriceValue.toCurrency()} / {MeasureType}";
        }

        
        [Display(Name = "Average Price")]
        public string averagePriceText
        {
            get => $"{averagePriceValue.toCurrency()} / {MeasureType}";
        }
        
        #endregion
    }
}
