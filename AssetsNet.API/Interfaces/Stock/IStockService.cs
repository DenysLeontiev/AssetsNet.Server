namespace AssetsNet.API.Interfaces.Stock;

public interface IStockService
{
    Task<Models.Stock.Stock> GetStockData(string stockName);
}