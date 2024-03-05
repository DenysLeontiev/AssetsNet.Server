namespace AssetsNet.API.Models.Stock;

public class Stock
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public long MarketCap { get; set; }
    public decimal DollarChange { get; set; }
    public decimal PercentChange { get; set; }
}