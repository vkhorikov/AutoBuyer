using System;

namespace AutoBuyer.Logic
{
    public interface IStockItemConnection
    {
        event Action<string> MessageReceived;
        string BuyerName { get; }
        void SendMessage(string message);
    }
}
