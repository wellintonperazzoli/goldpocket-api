using GoldPocket.Data;
using GoldPocket.Models;
using GoldPocket.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GoldPocket.Services.ModelServices
{
    public class LocationService : BaseService
    {
        private LocationDb location;
        private List<LocationDb> locations;

        public LocationService(GoldPocketContext context, UserService userService) : base(context, userService)
        { }

        public async Task<List<Location>> ToList(string search = "", string sortby = "")
        {
            List<Location> locationDetail = new List<Location>();
            locations = await getAll();
            locations.ForEach(l => locationDetail.Add(getDetails(l)));

            search = search ?? "";
            var result = locationDetail
                .Where(e =>
                    e.Name.ToUpper().Contains(search.ToUpper())
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

        public async Task<Location> Get(int? id = default)
        {
            location = await getSingle(id);
            return location == null ?
                null :
                getDetails(location);
        }

        public async Task<int> Create(Location location)
        {
            if (_context.Location.Where(l => l.Name == location.Name).Any())
                throw new Exception("You already have a location with this name");

            var newLocation = new CategoryDb
            {
                Name = location.Name,
                appUser = await _userService.getCurrentUser()
            };
            _context.Add(newLocation);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Location locationDetail)
        {
            var currentCategoryDetail = await Get(locationDetail.Id);

            if (currentCategoryDetail == null)
            {
                return 0;
            }

            var query = await getIncluded();
            LocationDb sameName = await query.Where(l => l.Name == locationDetail.Name && l.Id != locationDetail.Id).FirstOrDefaultAsync();

            if (sameName == null)
            {
                location.Name = locationDetail.Name;
                _context.Update(location);
            }
            else
            {
                location.Expenses.ForEach(c => sameName.Expenses.Add(c));
                _context.Update(sameName);
                _context.Remove(location);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int? id = default)
        {
            var locationDetail = await Get(id);

            if (locationDetail == null || locationDetail.hasRelation)
            {
                return 0;
            }

            _context.Remove(location);
            return await _context.SaveChangesAsync();
        }

        

        public static Location getDetails(LocationDb l)
        {
            if (l == null)
            {
                return null;
            }

            List<Item> items = new List<Item>();
            l.Expenses.OrderByDescending(e => e.dateTime).ToList().ForEach(e =>
            {
                e.expensesItems.ForEach(ei =>
                {
                    if (!items.Where(i => i.Id == ei.Item.Id).Any())
                    {
                        items.Add(ItemService.getDetails(ei.Item));
                    }
                });
            });

            return new Location
            {
                Id = l.Id,
                Name = l.Name,
                expenseCount = l.Expenses.Count(),
                Items = items,
            };;
        }



        private async Task<List<LocationDb>> getAll()
        {
            var query = await getIncluded();
            return await query.ToListAsync();
        }

        private async Task<LocationDb> getSingle(int? id)
        {
            var query = await getIncluded();
            return await query.Where(l => l.Id == id).FirstOrDefaultAsync();
        }

        private async Task<IQueryable<LocationDb>> getIncluded()
        {
            var currentUser = await _userService.getCurrentUser();
            return _context.Location
            .Include("Expenses.expensesItems.Item.Category")
            .Include("Expenses.expensesItems.Item.expensesItems.Expense.Location")
            .Where(c => c.appUserId == currentUser.Id);

        }
    }
}
