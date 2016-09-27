using System;
using System.Collections.Generic;

namespace AutoBuyer.Logic
{
    public class Buyer : IStockEventListener
    {
        private readonly List<IBuyerListener> _listeners = new List<IBuyerListener>();
        private readonly int _maximumPrice;
        private readonly int _numberToBuy;
        private readonly IStockItem _stockItem;
        public BuyerSnapshot Snapshot { get; private set; }

        public Buyer(string itemId, int maximumPrice, int numberToBuy, IStockItem stockItem)
        {
            _numberToBuy = numberToBuy;
            _maximumPrice = maximumPrice;
            _stockItem = stockItem;
            Snapshot = BuyerSnapshot.Joining(itemId);
        }

        public void AddBuyerListener(IBuyerListener listener)
        {
            _listeners.Add(listener);
        }

        public void CurrentPrice(int price, int numberInStock)
        {
            if (Snapshot.State == BuyerState.Closed)
                return;

            if (price > _maximumPrice)
            {
                Snapshot = Snapshot.Monitoring(price, numberInStock);
            }
            else
            {
                int numberToBuy = Math.Min(numberInStock, _numberToBuy);
                _stockItem.Buy(price, numberToBuy);
                Snapshot = Snapshot.Buying(price, numberInStock);
            }
            NotifyChange();
        }

        public void ItemPurchased(int numberSold, PurchaseSource purchaseSource)
        {
            if (purchaseSource == PurchaseSource.FromBuyer)
            {
                Snapshot = Snapshot.Bought(numberSold);
                if (Snapshot.BoughtSoFar >= _numberToBuy)
                {
                    Snapshot = Snapshot.Closed();
                }
                NotifyChange();
            }
        }

        public void ItemClosed()
        {
            Snapshot = Snapshot.Closed();
            NotifyChange();
        }

        private void NotifyChange()
        {
            foreach (IBuyerListener listener in _listeners)
            {
                listener.BuyerStateChanged(Snapshot);
            }
        }
    }
}
