using System.ComponentModel.DataAnnotations;

namespace GoldPocket.Models.DB
{
    public class BaseDb
    {
        [Display(Name = "#")]
        public int Id { get; set; }

        public string? appUserId { get; set; }
        public appUser? appUser { get; set; }
    }
}
