using System;

namespace AssetsNet.API.Models.Stock
{
    public class HeaderStockData
    {
        //symbol passed
        public string symbol { get; set; } = "";
        //name of the instrument
        public string name { get; set; } = "";
        //exchange where instrument is traded
        public string exchange { get; set; } = "";
        //market identifier code (MIC) under ISO 10383 standard
        public string mic_code { get; set; } = "";
        //currency in which the equity is denominated
        public string currency { get; set; } = "";
        //unix timestamp of the last price
        public string timestamp { get; set; } = "";
        //	datetime in defined timezone referring to when the bar
        //	with specified interval was opened
        public string datetime { get; set; } = "";
        //price at the opening of current bar
        public string open { get; set; } = "";
        //lowest price which occurred during the current bar
        public string low { get; set; } = "";
        //highest price which occurred during the current bar
        public string high { get; set; } = "";
        //close price at the end of the bar
        public string close { get; set; } = "";
        //	trading volume during the bar
        public string volume { get; set; } = "";
        //close price at the end of the previous bar
        public string previous_close { get; set; } = "";
        //	close - previous_close
        public string change { get; set; } = "";
        //(close - previous_close) / previous_close * 100
        public string percent_change { get; set; } = "";
        //average volume of the specified period
        public string average_volume { get; set; } = "";
        //true if market is open; false if closed
        public bool is_market_open { get; set; }
    }
}
