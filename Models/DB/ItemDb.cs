using GoldPocket.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldPocket.Models.DB
{

    [Table("Item")]
    public class ItemDb : BaseDb
    {
        public string Name { get; set; }
        public int categoryId { get; set; }
        public CategoryDb Category { get; set; }
        public MeasureTypes measureType { get; set; } = 0;
        public List<expenseItemDb> expensesItems { get; set; } = new List<expenseItemDb>();
    }
}
