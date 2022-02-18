using GoldPocket.Data;
using GoldPocket.Models;

namespace GoldPocket.Services.ModelServices
{
    public class ChartService : BaseService
    {
        protected ExpenseService _expenseService;
        protected ExpenseItemService _expenseItemService;
        public ChartService(ExpenseService expenseService, ExpenseItemService expenseItemService, GoldPocketContext context, UserService userService) : base(context, userService)
        {
            _expenseService = expenseService;
            _expenseItemService = expenseItemService;
        }


        public async Task<Chart<ChartData>> expenseCategory(int months = 5)
        {
            var numberOfMonths = months < 1 ? 1 : months;
            var expenses = await _expenseService.ToList();

            var categories = expenses.Select(e => e.Category).Distinct().ToList();
            Chart<ChartData> chart = new Chart<ChartData>();

            categories.ForEach(c =>
            {
                DateTime today = DateTime.Now.AddMonths((numberOfMonths - 1) * -1);
                ChartData modelData = new ChartData
                {
                    label = c,
                    data = new List<double>()
                };
                for (var i = numberOfMonths; i > 0; i--)
                {
                    var month = $"{today.ToString("MMM")} {today.ToString("yyyy")}";
                    if (!chart.labels.Any(e => e.Equals(month))) chart.labels.Add(month);
                    modelData.data.Add(expenses.Where(e =>
                            e.dateTime.Month == today.Month
                            && e.dateTime.Year == today.Year
                            && e.Category == c
                        )
                        .Sum(e => e.totalValue));
                    today = today.AddMonths(1);
                }

                chart.datasets.Add(modelData);
            });

            return chart;
        }


        public async Task<Chart<ChartData>> itemCategory(int months = 5)
        {
            var numberOfMonths = months < 1 ? 1 : months;
            List<expenseItem> expenseItems = await _expenseItemService.ToList();
            var categories = expenseItems.Select(e => e.itemCategory).Distinct().ToList();
            Chart<ChartData> chart = new Chart<ChartData>();

            categories.ForEach(c =>
            {
                DateTime today = DateTime.Now.AddMonths((numberOfMonths - 1) * -1);
                ChartData modelData = new ChartData
                {
                    label = c,
                    data = new List<double>()
                };
                for (var i = numberOfMonths; i > 0; i--)
                {
                    var month = $"{today.ToString("MMM")} {today.ToString("yyyy")}";
                    if (!chart.labels.Any(e => e.Equals(month))) chart.labels.Add(month);
                    modelData.data.Add(expenseItems.Where(e =>
                            e.dateTime.Month == today.Month
                            && e.dateTime.Year == today.Year
                            && e.itemCategory == c
                        )
                        .Select(e => e.unitPrice * e.Quantity)
                        .Sum(e => e));
                    today = today.AddMonths(1);
                }

                chart.datasets.Add(modelData);
            });

            return chart;
        }


        public async Task<Chart<double>> expenseCategoryDoughnut()
        {
            Chart<double> chart = new Chart<double>();
            var expenses = await _expenseService.ToList();
            var categories = expenses.Select(e => e.Category).Distinct().ToList();
            chart.labels = categories;
            var data = new List<double>();
            categories.ForEach(c =>
            {
                data.Add(expenses.Where(e =>
                        e.Category == c
                    )
                    .Sum(e => e.totalValue));
            });
            chart.datasets = data;

            return chart;
        }


        public async Task<Chart<double>> itemCategoryDoughnut()
        {
            Chart<double> chart = new Chart<double>();
            var expenseItems = await _expenseItemService.ToList();
            var categories = expenseItems.Select(e => e.itemCategory).Distinct().ToList();
            chart.labels = categories;
            var data = new List<double>();
            categories.ForEach(c =>
            {
                data.Add(expenseItems.Where(e =>
                        e.itemCategory == c
                    )
                    .Sum(e => e.total));
            });
            chart.datasets = data;

            return chart;
        }
    }
}