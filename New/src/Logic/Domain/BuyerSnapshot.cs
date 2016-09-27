namespace AutoBuyer.Logic.Domain
{
    public struct BuyerSnapshot
    {
        public int CurrentPrice { get; }
        public int NumberInStock { get; }
        public int BoughtSoFar { get; }
        public BuyerState State { get; }

        public BuyerSnapshot(int currentPrice, int numberInStock,
            int boughtSoFar, BuyerState state)
        {
            CurrentPrice = currentPrice;
            NumberInStock = numberInStock;
            BoughtSoFar = boughtSoFar;
            State = state;
        }

        public static BuyerSnapshot Joining()
        {
            return new BuyerSnapshot(0, 0, 0, BuyerState.Joining);
        }

        public BuyerSnapshot Monitoring(int currentPrice, int numberInStock)
        {
            return new BuyerSnapshot(currentPrice, numberInStock, BoughtSoFar, BuyerState.Monitoring);
        }

        public BuyerSnapshot Bought(int numberBought)
        {
            return new BuyerSnapshot(CurrentPrice, NumberInStock - numberBought, BoughtSoFar + numberBought, State);
        }

        public BuyerSnapshot Closed()
        {
            return new BuyerSnapshot(CurrentPrice, NumberInStock, BoughtSoFar, BuyerState.Closed);
        }

        public BuyerSnapshot Buying(int currentPrice, int numberInStock)
        {
            return new BuyerSnapshot(currentPrice, numberInStock, BoughtSoFar, BuyerState.Buying);
        }
    }
}
