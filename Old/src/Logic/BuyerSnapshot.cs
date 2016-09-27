namespace AutoBuyer.Logic
{
    public struct BuyerSnapshot
    {
        public string ItemId { get; }
        public int CurrentPrice { get; }
        public int NumberInStock { get; }
        public int BoughtSoFar { get; }
        public BuyerState State { get; }

        public BuyerSnapshot(string itemId, int currentPrice, int numberInStock,
            int boughtSoFar, BuyerState state)
        {
            ItemId = itemId;
            CurrentPrice = currentPrice;
            NumberInStock = numberInStock;
            BoughtSoFar = boughtSoFar;
            State = state;
        }

        public static BuyerSnapshot Joining(string itemId)
        {
            return new BuyerSnapshot(itemId, 0, 0, 0, BuyerState.Joining);
        }

        public BuyerSnapshot Monitoring(int currentPrice, int numberInStock)
        {
            return new BuyerSnapshot(ItemId, currentPrice, numberInStock, BoughtSoFar, BuyerState.Monitoring);
        }

        public BuyerSnapshot Bought(int numberBought)
        {
            return new BuyerSnapshot(ItemId, CurrentPrice, NumberInStock - numberBought, BoughtSoFar + numberBought, State);
        }

        public BuyerSnapshot Closed()
        {
            return new BuyerSnapshot(ItemId, CurrentPrice, NumberInStock, BoughtSoFar, BuyerState.Closed);
        }

        public BuyerSnapshot Buying(int currentPrice, int numberInStock)
        {
            return new BuyerSnapshot(ItemId, currentPrice, numberInStock, BoughtSoFar, BuyerState.Buying);
        }
    }
}
