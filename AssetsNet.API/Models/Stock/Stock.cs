namespace AssetsNet.API.Models.Stock;

public class Stock
{
    public Stock(
        string name,
        string price,
        string marketCap,
        string dollarChange,
        string percentChange)
    {
        Name = name;
        Price = price;
        MarketCap = marketCap;
        DollarChange = dollarChange;
        PercentChange = percentChange;
    }

    public string Name { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string MarketCap { get; set; } = string.Empty;
    public string DollarChange { get; set; } = string.Empty;
    public string PercentChange { get; set; } = string.Empty;
}