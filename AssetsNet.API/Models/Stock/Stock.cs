namespace AssetsNet.API.Models.Stock;

public class Stock
{
    public Stock(
        string close,
        float high,
        double marketCap,
        float dollarChange,
        float percentChange)
    {
        Close = close;
        High = high;
        Low = marketCap;
        Volume = dollarChange;
        PercentChange = percentChange;
    }

    public string Close { get; set; }
    public float High { get; set; }
    public double Low { get; set; }
    public float Volume { get; set; }
    public float PercentChange { get; set; }
}