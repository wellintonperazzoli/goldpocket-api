using GoldPocket.Data;
using GoldPocket.Enums;
using GoldPocket.Extensions;
using GoldPocket.Models;
using GoldPocket.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GoldPocket.Services.ModelServices
{
    public class ExpenseService : BaseService
    {
        private ExpenseDb expense;
        private List<ExpenseDb> expenses;
        private List<CategoryDb> newCategories;

        public ExpenseService(GoldPocketContext context, UserService userService) : base(context, userService)
        { 
            this.newCategories = new List<CategoryDb>();
        }

        public async Task<List<Expense>> ToList(string search = "", string sortby = "")
        {
            List<Expense> expenseDetails = new List<Expense>();
            expenses = await getAll();
            expenses.ForEach(i => expenseDetails.Add(getDetails(i)));

            search = search ?? "";
            var result = expenseDetails
                .Where(e =>
                    e.Location.ToUpper().Contains(search.ToUpper())
                    || e.Category.ToUpper().Contains(search.ToUpper())
                    || e.totalValueText.ToUpper().Contains(search.ToUpper())
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

        public async Task<Expense> Get(int? id = default)
        {
            expense = await getSingle(id);
            return expense == null ?
                null :
                getDetails(expense);
        }


        public async Task<int> Create(Expense expenseDetail)
        {
            expense = new ExpenseDb
            {
                dateTime = expenseDetail.dateTime,
                Description = expenseDetail.Description,
                Location = await setLocation(expenseDetail.Location),
                Category = await setCategory(expenseDetail.Category, CategoryTypes.Expense),
                appUser = await _userService.getCurrentUser()
            };

            expense.expensesItems = await setExpenseItems(expenseDetail.expenseItems);
            expense.Value = expense.expensesItems.Sum(ei => ei.itemQuantity * ei.unitPrice);

            _context.Add(expense);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Expense expenseDetail)
        {
            var currentExpenseDetail = await Get(expenseDetail.Id);
            if (currentExpenseDetail == null) return 0;

            expense.dateTime = expenseDetail.dateTime;
            expense.Description = expenseDetail.Description;

            removeRelations(expense);
            await _context.SaveChangesAsync();

            expense.Location = await setLocation(expenseDetail.Location);
            expense.Category = await setCategory(expenseDetail.Category, CategoryTypes.Expense);
            expense.expensesItems = await setExpenseItems(expenseDetail.expenseItems);
            expense.Value = expense.expensesItems.Sum(ei => ei.itemQuantity * ei.unitPrice);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int? id = default)
        {
            expense = await getSingle(id);
            if (expense == null) return 0;
            removeRelations(expense);
            _context.Remove(expense);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryDb>> getAllCategories() => await _context.Category.ToListAsync();

        public async Task<List<LocationDb>> getLocationList() => await _context.Location.ToListAsync();


        public Expense blankForm() =>
            new Expense
            {
                dateTime = DateTime.Now,
                expenseItems = {
                    new expenseItem {
                        Quantity = 1,
                        MeasureType = MeasureTypes.Unit
                    },
                },
            };

        public async Task<string> CurrentMonth() =>
                (await getAll()).Where(l => l.dateTime.Month == DateTime.Now.Month && l.dateTime.Year == DateTime.Now.Year)
                .Select(e => e.Value).Sum().toCurrency();



        public static Expense getDetails(ExpenseDb expense)
        {
            if (expense == null) return null;

            List<expenseItem> expenseItems = new List<expenseItem>();
            expense.expensesItems.ForEach(e =>
            {
                expenseItems.Add(ExpenseItemService.getDetails(e));
            });
            return new Expense
            {
                Id = expense.Id,
                Category = expense.Category.Name,
                dateTime = expense.dateTime,
                Description = expense.Description,
                expenseItems = expenseItems,
                Location = expense.Location.Name,
                totalValue = expense.Value
            };
        }

        #region Private Functions


        private async Task<List<ExpenseDb>> getAll()
        {
            var query = await getIncluded();
            return await query
                .OrderByDescending(e => e.dateTime).ToListAsync();
        }
        private async Task<ExpenseDb> getSingle(int? id)
        {
            var query = await getIncluded();
            return await query.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        private async Task<IQueryable<ExpenseDb>> getIncluded()
        {
            var currentUser = await _userService.getCurrentUser();
            return _context.Expense
            .Include("expensesItems.Item.Category")
            .Include("Location")
            .Include("Category")
            .Where(c => c.appUserId == currentUser.Id);
        }

        private void removeRelations(ExpenseDb expense)
        {
            var location = _context.Location.Where(l => l.Expenses.Count == 1 && l.Expenses.FirstOrDefault().Id == expense.Id).FirstOrDefault();
            if (location != null) _context.Remove(location);
            var items = _context.Item.Where(i => i.expensesItems.Count == 1 && i.expensesItems.FirstOrDefault().Expense.Id == expense.Id).ToList();
            foreach (var item in items) _context.Remove(item);
            foreach (var ei in expense.expensesItems)
            {
                _context.Remove(ei);
            }
        }
        private async Task<List<expenseItemDb>> setExpenseItems(List<expenseItem> expenseItems)
        {
            List<expenseItemDb> expenseItemsList = new List<expenseItemDb>();
            foreach (var ei in expenseItems)
            {
                expenseItemsList.Add(await setExpenseItem(ei));
            }
                
            return expenseItemsList;
        }

        private async Task<expenseItemDb> setExpenseItem(expenseItem ei)
        {
            expenseItemDb expenseItem = new expenseItemDb
            {
                Item = await setItem(ei),
                itemQuantity = Convert.ToDouble(ei.Quantity),
                Expense = expense,
                appUser = await _userService.getCurrentUser(),
                dateTime = expense.dateTime,
                unitPrice = ei.MeasureType == MeasureTypes.Unit ? Convert.ToDouble(ei.Price) : Convert.ToDouble(ei.Price) / Convert.ToDouble(ei.Quantity),
                Price = Convert.ToDouble(ei.Price),
            };

            _context.Add(expenseItem);
            return expenseItem;
        }

        private async Task<ItemDb> setItem(expenseItem expenseItemDetail)
        {
            var item = await _context.Item.Where(i => i.Name == expenseItemDetail.Name).FirstOrDefaultAsync();
            if (item == null)
            {
                item = new ItemDb
                {
                    Name = expenseItemDetail.Name.FirstCharToUpper(),
                    measureType = expenseItemDetail.MeasureType,
                    appUser = await _userService.getCurrentUser()
                };
                _context.Add(item);
            }

            item.Category = await setCategory(expenseItemDetail.CategoryName, CategoryTypes.Item);

            return item;
        }

        private async Task<CategoryDb> setCategory(string name, CategoryTypes ct)
        {

            var category = await _context.Category.Where(c => c.Name == name && c.Type == ct).FirstOrDefaultAsync();
            if(category == null) { 
                category = this.newCategories.Where(c =>c.Name == name && c.Type == ct).FirstOrDefault();
            }

            if (category == null)
            {
                category = new CategoryDb
                {
                    Name = name.FirstCharToUpper(),
                    Type = ct,
                    appUser = await _userService.getCurrentUser()
                };
                _context.Add(category);
                this.newCategories.Add(category);
            }
            return category;
        }

        private async Task<LocationDb> setLocation(string name)
        {
            var location = await _context.Location.Where(l => l.Name == name).FirstOrDefaultAsync();
            if (location == null)
            {
                location = new LocationDb
                {
                    Name = name.FirstCharToUpper(),
                    appUser = await _userService.getCurrentUser()
                };
                _context.Add(location);
            }
            return location;
        }

        #endregion
    }
}
