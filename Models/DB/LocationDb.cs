using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldPocket.Models.DB
{
    [Table("Location")]
    public class LocationDb : BaseDb
    {
        public string Name { get; set; }
        public virtual List<ExpenseDb> Expenses { get; set; }
    }
}
