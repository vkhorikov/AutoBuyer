using System;

namespace AutoBuyer.Logic.Connections
{
    public class StockItemConnection : IStockItemConnection
    {
        public event Action<string> MessageReceived;

        public void SendMessage(string message)
        {
            // Send the message through a 3rd party SDK
        }
    }
}
