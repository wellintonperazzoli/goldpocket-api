namespace GoldPocket.Extensions
{
    public static class CurrencyExtensions
    {
        public static string toCurrency(this double input) => $"{input:0.00} €";
    }
}
