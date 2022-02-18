using Microsoft.AspNetCore.Identity;

namespace GoldPocket.Models.DB
{
    public class appUser : IdentityUser
    {
        public List<ExpenseDb> Expenses { get; set; }
        public List<LocationDb> Locations { get; set; }
        public List<ItemDb> Items { get; set; }
    }
}
