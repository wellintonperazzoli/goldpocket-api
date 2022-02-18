using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldPocket.Models.DB
{

    [Table("Expense")]
    public class ExpenseDb : BaseDb
    {
        public int? locationId { get; set; }
        public LocationDb Location { get; set; }
        public int categoryId { get; set; }
        public CategoryDb Category { get; set; }
        public DateTime dateTime { get; set; }
        public string? Description { get; set; }
        public double Value { get; set; }
        public List<expenseItemDb> expensesItems { get; set; } = new List<expenseItemDb>();
    }
}
