namespace Rubix.Explorer.API.Dtos
{
    public class VindaxRBTDetailsDto
    {
        public double askPrice { get; set; }
        public double askQty { get; set; }
        public double bidPrice { get; set; }
        public double bidQty { get; set; }
        public string firstId { get; set; }
        public int highPrice { get; set; }
        public string lastId { get; set; }
        public double lastQty { get; set; }
        public double lowPrice { get; set; }
        public double openPrice { get; set; }
        public int prevClosePrice { get; set; }
        public double priceChange { get; set; }
        public double priceChangePercent { get; set; }
        public double quoteVolume { get; set; }
        public string symbol { get; set; }
        public double volume { get; set; }
        public int weightedAvgPrice { get; set; }
        public double lastPrice { get; set; }
        public string openTime { get; set; }
        public string closeTime { get; set; }
    }
}
