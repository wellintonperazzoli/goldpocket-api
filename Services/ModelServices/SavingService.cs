using GoldPocket.Data;
using GoldPocket.Models;
using GoldPocket.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GoldPocket.Services.ModelServices
{
    public class SavingService : BaseService
    {
        private SavingDb saving;
        private List<SavingDb> savingList;
        private ILogger<SavingService> _logger;

        public SavingService(GoldPocketContext context, UserService userService, ILogger<SavingService> logger) : base(context, userService)
        {
            this._logger = logger;
        }

        public async Task<List<Saving>> ToList(string search = "", string sortby = "")
        {
            List<Saving> savings = new List<Saving>();
            savingList = await getAll();
            savingList.ForEach(l => savings.Add(getDetails(l)));

            search = search ?? "";
            var result = savings
                .Where(e =>
                    e.dateTime.ToString().ToUpper().Contains(search.ToUpper())
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

        public async Task<Saving> Get(int? id = default)
        {
            saving = await getSingle(id);
            return saving == null ?
                null :
                getDetails(saving);
        }

        public async Task<int> Create(Saving saving)
        {
            var newSaving = new SavingDb
            {
                dateTime = saving.dateTime,
                Value = saving.Value,
                appUser = await _userService.getCurrentUser()
            };
            _context.Add(newSaving);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Saving s)
        {
            var currentSaving = await Get(s.Id);

            if (currentSaving == null)
            {
                return 0;
            }

            saving.dateTime = s.dateTime;
            saving.Value = s.Value;
            _context.Update(saving);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int? id = default)
        {
            var savingDetail = await Get(id);

            if (savingDetail == null)
            {
                return 0;
            }

            _context.Remove(saving);
            return await _context.SaveChangesAsync();
        }

        private Saving getDetails(SavingDb l)
        {
            if(l == null) return null;

            var result = new Saving();
            result.Id = l.Id;
            result.Value = l.Value;
            result.dateTime = l.dateTime;

            return result;
        }

        private async Task<List<SavingDb>> getAll()
        {
            var query = await getIncluded();
            return await query.ToListAsync();
        }

        private async Task<SavingDb> getSingle(int? id)
        {
            var query = await getIncluded();
            return await query.Where(l => l.Id == id).FirstOrDefaultAsync();
        }

        private async Task<IQueryable<SavingDb>> getIncluded()
        {
            var currentUser = await _userService.getCurrentUser();
            return _context.Saving
            .Where(c => c.appUserId == currentUser.Id);

        }
    }
}
