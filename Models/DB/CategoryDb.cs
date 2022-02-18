using GoldPocket.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldPocket.Models.DB
{
    [Table("Category")]
    public class CategoryDb : BaseDb
    {        
        public string Name { get; set; }
        public CategoryTypes Type { get; set; }

        public List<ExpenseDb> Expenses { get; set; }
        public List<ItemDb> Items { get; set; }
    }
}
