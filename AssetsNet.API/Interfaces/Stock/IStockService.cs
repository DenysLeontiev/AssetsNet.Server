using AssetsNet.API.Models.Stock;

namespace AssetsNet.API.Interfaces.Stock;

public interface IStockService
{
    Task<StockData> GetStockData(string stockName);
    Task<IEnumerable<HeaderStockData>> GetStockDataList(IEnumerable<string> stockNames);
    Task <IEnumerable<string>> GetStockNamesList();
}