namespace AssetsNet.API.Models.Stock;

public class Stock
{
    public Stock(
        string name,
        float price,
        double marketCap,
        float dollarChange,
        float percentChange)
    {
        Name = name;
        Price = price;
        MarketCap = marketCap;
        DollarChange = dollarChange;
        PercentChange = percentChange;
    }

    public string Name { get; set; }
    public float Price { get; set; }
    public double MarketCap { get; set; }
    public float DollarChange { get; set; }
    public float PercentChange { get; set; }
}