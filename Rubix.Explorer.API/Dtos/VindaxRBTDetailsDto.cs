using System.Collections.Generic;

namespace Rubix.Explorer.API.Dtos
{
    public class VindaxRBTDetailsDto
    {
        public double askPrice { get; set; }
        public double askQty { get; set; }
        public double bidPrice { get; set; }
        public double bidQty { get; set; }
        public string firstId { get; set; }
        public double? highPrice { get; set; }
        public string lastId { get; set; }
        public double? lastQty { get; set; }
        public double? lowPrice { get; set; }
        public double? openPrice { get; set; }
        public double? prevClosePrice { get; set; }
        public double? priceChange { get; set; }
        public double? priceChangePercent { get; set; }
        public double? quoteVolume { get; set; }
        public string symbol { get; set; }
        public double? volume { get; set; }
        public double? weightedAvgPrice { get; set; }
        public double? lastPrice { get; set; }
        public string openTime { get; set; }
        public string closeTime { get; set; }
    }


    public class Datum
    {
        public string symbol { get; set; }
        public Ticker ticker { get; set; }
        public long timestamp { get; set; }
    }

    public class LBANKRBTDetailsDto
    {
        public string result { get; set; }
        public List<Datum> data { get; set; }
        public int error_code { get; set; }
        public long ts { get; set; }
    }

    public class Ticker
    {
        public string high { get; set; }
        public string vol { get; set; }
        public string low { get; set; }
        public string change { get; set; }
        public string turnover { get; set; }
        public string latest { get; set; }
    }


    public class Result
    {
        public string open { get; set; }
        public string bid { get; set; }
        public string ask { get; set; }
        public string low { get; set; }
        public string high { get; set; }
        public string last { get; set; }
        public string volume { get; set; }
        public string deal { get; set; }
        public string change { get; set; }
    }

    public class WhiteBITRBTResponse
    {
        public bool success { get; set; }
        public object message { get; set; }
        public Result result { get; set; }
    }

}
