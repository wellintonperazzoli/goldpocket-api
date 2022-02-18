using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldPocket.Models.DB
{
    [Table("expenseItem")]
    public class expenseItemDb : BaseDb
    {
        public double Price { get; set; }
        public double unitPrice { get; set; }
        public DateTime dateTime { get; set; }
        public int itemId { get; set; }
        public ItemDb Item { get; set; }
        public int expenseId { get; set; }
        public ExpenseDb Expense { get; set; }
        public double itemQuantity { get; set; }
    }
}
