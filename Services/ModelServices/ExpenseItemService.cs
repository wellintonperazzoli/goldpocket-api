using GoldPocket.Data;
using GoldPocket.Models;
using GoldPocket.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GoldPocket.Services.ModelServices
{
    public class ExpenseItemService : BaseService
    {
        private List<expenseItemDb> expenseItems;
        public ExpenseItemService(GoldPocketContext context, UserService userService) : base(context, userService)
        { }

        public async Task<List<expenseItem>> ToList()
        {
            List<expenseItem> expenseItemsDetails = new List<expenseItem>();
            expenseItems = await getAll();
            expenseItems.ForEach(i => expenseItemsDetails.Add(getDetails(i)));
            return expenseItemsDetails;
        }

        public static expenseItem getDetails(expenseItemDb expenseItem)
        {
            if (expenseItem == null)
            {
                return null;
            }


            return new expenseItem
            {
                Id = expenseItem.Id,
                itemId = expenseItem.Item.Id,
                Name = expenseItem.Item.Name,
                CategoryName = expenseItem.Item.Category.Name,
                MeasureType = expenseItem.Item.measureType,
                Quantity = expenseItem.itemQuantity,
                Price = expenseItem.Price,
                unitPrice = expenseItem.unitPrice,
                total = expenseItem.unitPrice * expenseItem.itemQuantity,
                dateTime = expenseItem.Expense.dateTime,
                LocationName = expenseItem.Expense.Location.Name,
                expenseId = expenseItem.expenseId,
                itemCategory = expenseItem.Item.Category.Name
            };
        }

        #region Private Functions


        private async Task<List<expenseItemDb>> getAll()
        {
            var query = await getIncluded();
            return await query.OrderByDescending(e => e.dateTime).ToListAsync();
        }
        private async Task<expenseItemDb> getSingle(int? id)
        {
            var query = await getIncluded();
            return await query.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        private async Task<IQueryable<expenseItemDb>> getIncluded()
        {
            var currentUser = await _userService.getCurrentUser();
            return _context.expenseItem
            .Include("Item.Category")
            .Include("Expense.Location")
            .Where(c => c.appUserId == currentUser.Id);
        }


        #endregion
    }
}
