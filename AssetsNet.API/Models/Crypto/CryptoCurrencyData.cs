namespace AssetsNet.API.Models.Crypto;

public class CryptoCurrencyData
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public double DollarChange { get; set; }
    public double PercentChange { get; set; }
    public double DayLow { get; set; }
    public double DayHigh { get; set; }
    public double YearLow { get; set; }
    public double YearHigh { get; set; }
    public double MarketCap { get; set; }
    public long Volume { get; set; }
    public DateTime DataCollected { get; set; }
}