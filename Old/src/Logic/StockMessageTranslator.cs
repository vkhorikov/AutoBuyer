using System;
using System.Collections.Generic;

namespace AutoBuyer.Logic
{
    public class StockMessageTranslator
    {
        private readonly List<IStockEventListener> _listeners = new List<IStockEventListener>();
        private readonly string _buyerName;

        public StockMessageTranslator(string buyerName)
        {
            _buyerName = buyerName;
        }

        public void AddStockEventListener(IStockEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void ProcessMessage(string message)
        {
            StockEvent stockEvent = StockEvent.From(message);
            switch (stockEvent.Type)
            {
                case StockEventType.Price:
                    Notify(x => x.CurrentPrice(stockEvent.CurrentPrice, stockEvent.NumberInStock));
                    break;

                case StockEventType.Purchase:
                    Notify(x => x.ItemPurchased(stockEvent.NumberSold, GetEventSource(stockEvent.BuyerName)));
                    break;

                case StockEventType.Close:
                    Notify(x => x.ItemClosed());
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private PurchaseSource GetEventSource(string buyerName)
        {
            return buyerName == _buyerName ? PurchaseSource.FromBuyer : PurchaseSource.FromOtherBuyer;
        }

        private void Notify(Action<IStockEventListener> action)
        {
            foreach (IStockEventListener listener in _listeners)
            {
                action(listener);
            }
        }
    }
}
