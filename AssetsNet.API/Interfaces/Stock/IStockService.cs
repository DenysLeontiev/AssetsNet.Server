namespace AssetsNet.API.Interfaces.Stock;

public interface IStockService
{
    Task<Models.Stock.StockData> GetStockData(string stockName);
    Task<List<Models.Stock.HeaderStockData>> GetStockDataList(List<string> stockNames);
}