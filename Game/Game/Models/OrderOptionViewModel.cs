namespace Game.Models
{
    public class OrderOptionViewModel
    {
        public string Account { get; set; } = null!;
        public decimal TotalGameMoney { get; set; }
        public List<OrderOptionItem> OrderOptions { get; set; }
    }

    public class OrderOptionItem
    {
        public string OrderType { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal GameMoney { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
