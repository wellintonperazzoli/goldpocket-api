namespace GoldPocket.Models{
    public class Chart<T>{
        public List<string> labels { get; set; } = new();
        public List<T> datasets { get; set; } = new();
    }

    public class ChartData{
        public string label { get; set; } = "";
        public List<double> data { get; set; } = new();
    }
}