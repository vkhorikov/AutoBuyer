using System;

namespace AutoBuyer.Logic.Connections
{
    public interface IStockItemConnection
    {
        event Action<string> MessageReceived;
        void SendMessage(string message);
    }
}
