using GoldPocket.Enums;
using System.ComponentModel.DataAnnotations;

namespace GoldPocket.Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
