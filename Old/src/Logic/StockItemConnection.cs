using System;

namespace AutoBuyer.Logic
{
    public class StockItemConnection : IStockItemConnection
    {
        public event Action<string> MessageReceived;

        public string BuyerName { get; }

        public StockItemConnection(string buyerName)
        {
            BuyerName = buyerName;
        }

        public void SendMessage(string message)
        {
            // Send the message through a 3rd party SDK
        }
    }
}
