using System;
using System.Collections.Generic;

namespace AutoBuyer.Logic.Domain
{
    public class Buyer
    {
        public string BuyerName { get; }
        public int MaximumPrice { get; }
        public int NumberToBuy { get; }
        public BuyerSnapshot Snapshot { get; private set; }

        public Buyer(string buyerName, int maximumPrice, int numberToBuy)
        {
            BuyerName = buyerName;
            NumberToBuy = numberToBuy;
            MaximumPrice = maximumPrice;
            Snapshot = BuyerSnapshot.Joining();
        }

        public StockCommand Process(StockEvent stockEvent)
        {
            if (Snapshot.State == BuyerState.Closed)
                return StockCommand.None();

            switch (stockEvent.Type)
            {
                case StockEventType.Price:
                    return ProcessPriceEvent(stockEvent.CurrentPrice, stockEvent.NumberInStock);

                case StockEventType.Purchase:
                    return ProcessPurchaseEvent(stockEvent.BuyerName, stockEvent.NumberSold);

                case StockEventType.Close:
                    return ProcessCloseEvent();

                default:
                    throw new InvalidOperationException();
            }
        }

        private StockCommand ProcessPurchaseEvent(string buyerName, int numberSold)
        {
            if (BuyerName == buyerName)
            {
                Snapshot = Snapshot.Bought(numberSold);
                if (Snapshot.BoughtSoFar >= NumberToBuy)
                {
                    Snapshot = Snapshot.Closed();
                }
            }
            return StockCommand.None();
        }

        private StockCommand ProcessPriceEvent(int currentPrice, int numberInStock)
        {
            if (currentPrice > MaximumPrice)
            {
                Snapshot = Snapshot.Monitoring(currentPrice, numberInStock);
                return StockCommand.None();
            }

            Snapshot = Snapshot.Buying(currentPrice, numberInStock);

            int numberToBuy = Math.Min(numberInStock, NumberToBuy);
            return StockCommand.Buy(currentPrice, numberToBuy);
        }

        private StockCommand ProcessCloseEvent()
        {
            Snapshot = Snapshot.Closed();
            return StockCommand.None();
        }
    }
}
