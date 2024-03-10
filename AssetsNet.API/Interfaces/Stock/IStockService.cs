namespace AssetsNet.API.Interfaces.Stock;

public interface IStockService
{
    Task<Models.Stock.StockData> GetStockData(string stockName);
}