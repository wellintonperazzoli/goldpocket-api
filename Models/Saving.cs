using System.ComponentModel.DataAnnotations;

namespace GoldPocket.Models
{
    public class Saving : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must inform the date.")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime dateTime { get; set; }

        [Required(ErrorMessage = "Please inform a value for you saving.")]
        [Display(Name = "Value")]
        public double Value { get; set; }

    }
}
