using GoldPocket.Data;
using GoldPocket.Extensions;
using GoldPocket.Models;
using GoldPocket.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GoldPocket.Services.ModelServices
{
    public class CategoryService : BaseService
    {
        private CategoryDb category;
        private List<CategoryDb> categories;

        public CategoryService(GoldPocketContext context, UserService userService) : base(context, userService)
        { }

        public async Task<List<Category>> ToList(string search = "", string sortby = "")
        {
            List<Category> categorieDetails = new List<Category>();
            categories = await getAll();
            categories.ForEach(c => categorieDetails.Add(getDetails(c)));
            
            search = search ?? "";
            var result = categorieDetails
                .Where(e =>
                    e.Name.ToUpper().Contains(search.ToUpper())
                    || e.Type.ToString().ToUpper().Contains(search.ToUpper())
                    || e.Quantity.ToString().ToUpper().Contains(search.ToUpper())
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

        public async Task<Category> Get(int? id = default)
        {
            category = await getSingle(id);
            return category == null ?
                null :
                getDetails(category);
        }

        public async Task<int> Create(Category category)
        {
            if (_context.Category.Where(c => c.Name == category.Name && c.Type == category.Type).Any())
                throw new Exception("You already have this category");
            
            var newCategory = new CategoryDb
            {
                Name = category.Name.FirstCharToUpper(),
                Type = category.Type,
                appUser = await _userService.getCurrentUser()
            };
            _context.Add(newCategory);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Category categoryDetail)
        {
            var currentCategoryDetail = await Get(categoryDetail.Id);

            if (currentCategoryDetail == null)
            {
                return 0;
            }

            var query = await getIncluded();
            var sameName = await query.Where(c => c.Name == categoryDetail.Name && c.Id != categoryDetail.Id && c.Type == categoryDetail.Type).FirstOrDefaultAsync();

            if (sameName == null)
            {
                category.Name = categoryDetail.Name;
                if (!currentCategoryDetail.hasRelation)
                    category.Type = categoryDetail.Type;

                _context.Update(category);
            }
            else
            {
                category.Expenses.ForEach(c => sameName.Expenses.Add(c));
                category.Items.ForEach(i => sameName.Items.Add(i));
                _context.Update(sameName);
                _context.Remove(category);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int? id = default)
        {
            var categoryDetail = await Get(id);

            if (categoryDetail == null || categoryDetail.hasRelation)
            {
                return 0;
            }

            _context.Remove(category);
            return await _context.SaveChangesAsync();
        }

        public static Category getDetails(CategoryDb c)
        {
            if (c == null)
            {
                return null;
            }

            List<Item> itemDetails = new List<Item>();
            List<Expense> expenseDetails = new List<Expense>();

            c.Items.ForEach(item => itemDetails.Add(ItemService.getDetails(item)));
            c.Expenses.ForEach(expense => expenseDetails.Add(ExpenseService.getDetails(expense)));

            var categoryDetail = new Category
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type,
                itemDetails = itemDetails,
                expenseDetails = expenseDetails
            };

            return categoryDetail;
        }



        private async Task<List<CategoryDb>> getAll()
        {
            var query = await getIncluded();
            return await query.ToListAsync();
        }
        private async Task<CategoryDb> getSingle(int? id)  
        {
            var query = await getIncluded();
            return await query.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        private async Task<IQueryable<CategoryDb>> getIncluded()
        {
            var currentUser = await _userService.getCurrentUser();
            return _context.Category
                .Include("Items.expensesItems.Expense.Location")
                .Include("Expenses.Location")
                .Where(c => c.appUserId == currentUser.Id);
        }
    }
}
