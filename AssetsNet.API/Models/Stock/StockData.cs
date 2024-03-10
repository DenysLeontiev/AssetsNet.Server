using Newtonsoft.Json;

namespace AssetsNet.API.Models.Stock;

public class StockData
{
    public double Close { get; set; }
    public int Dividends { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Open { get; set; }
    public int StockSplits { get; set; }
    public int Volume { get; set; }
}