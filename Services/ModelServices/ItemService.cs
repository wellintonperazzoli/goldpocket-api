using GoldPocket.Data;
using GoldPocket.Enums;
using GoldPocket.Models;
using GoldPocket.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GoldPocket.Services.ModelServices
{
    public class ItemService : BaseService
    {
        private ItemDb item;
        private List<ItemDb> items;

        public ItemService(GoldPocketContext context, UserService userService) : base(context, userService)
        { }

        public async Task<List<Item>> ToList(string search = "", string sortby = "")
        {
            List<Item> itemDetails = new List<Item>();
            items = await getAll();
            items.ForEach(i => itemDetails.Add(getDetails(i)));
            
            search = search ?? "";
            var result = itemDetails
                .Where(e =>
                    e.Name.ToUpper().Contains(search.ToUpper())
                    || e.CategoryName.ToUpper().Contains(search.ToUpper())
                    || e.averagePriceText.ToUpper().Contains(search.ToUpper())
                    || e.recentPriceText.ToUpper().Contains(search.ToUpper())
                );

            try
            {
                var order = sortby.Substring(sortby.Length - 1);
                var attribute = sortby.Remove(sortby.Length - 2);
                if (order == "a")
                    return result.OrderBy(e => e[attribute]).ToList();
                return result.OrderByDescending(e => e[attribute]).ToList();
            }
            catch
            {
                return result.ToList();
            }
        }

        public async Task<Item> Get(int? id = default)
        {
            item = await getSingle(id);
            return item == null ?
                null :
                getDetails(item);
        }


        public async Task<int> Create(ItemDb item)
        {
            _context.Add(item);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Item itemDetail)
        {
            var currentItemDetail = await Get(itemDetail.Id);
            if (currentItemDetail == null) return 0;

            if (_context.Item.Where(i => i.Id != itemDetail.Id && i.Name == itemDetail.Name).Any())
                throw new Exception("You already have an item with this name");

            item.Name = itemDetail.Name;
            var itemCategory = await _context.Category.Where(c => c.Name == itemDetail.CategoryName).ToListAsync();
            if (itemCategory != null)
                item.Category = new CategoryDb { Name = itemDetail.CategoryName, Type = CategoryTypes.Item };

            _context.Update(item);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int? id = default)
        {
            var item = await getSingle(id);

            if (item == null)
            {
                return 0;
            }

            _context.Remove(item);
            return await _context.SaveChangesAsync();
        }


        public async Task<List<CategoryDb>> getCategoryList()
        {
            var currentUser = await _userService.getCurrentUser();
            return await _context.Category.Where(c => c.Type == CategoryTypes.Item && c.appUserId == currentUser.Id).ToListAsync();
        }

        public static Item getDetails(ItemDb item)
        {
            if (item == null)
            {
                return new Item();
            }

            var orderedExpenseItem = item.expensesItems
                .OrderByDescending(ei => ei.Expense.dateTime);

            List<expenseItem> expenseItems = new List<expenseItem>();
            orderedExpenseItem.ToList().ForEach(ei =>
            {
                expenseItems.Add(ExpenseItemService.getDetails(ei));
            });

            var returnItem = new Item();
            returnItem.Id = item.Id;
            returnItem.Name = item.Name;
            returnItem.CategoryName = item.Category.Name;
            returnItem.MeasureType = item.measureType;
            returnItem.recentPriceValue = orderedExpenseItem.FirstOrDefault().unitPrice;
            returnItem.recentPriceLocation = orderedExpenseItem.FirstOrDefault().Expense.Location.Name;
            returnItem.averagePriceValue = item.expensesItems.Average(ip => ip.unitPrice);
            returnItem.expenseItems = expenseItems;
            return returnItem;
        }

        private async Task<List<ItemDb>> getAll()
        {
            var query = await getIncluded();
            return await query.ToListAsync();
        }
        private async Task<ItemDb> getSingle(int? id)
        {
            var query = await getIncluded();
            return await query.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        private async Task<IQueryable<ItemDb>> getIncluded()
        {
            var currentUser = await _userService.getCurrentUser();
            return _context.Item
                .Include("Category")
                .Include("expensesItems.Expense.Location")
                .Where(c => c.appUserId == currentUser.Id);
        }
    }
}
