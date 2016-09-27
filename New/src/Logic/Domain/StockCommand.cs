namespace AutoBuyer.Logic.Domain
{
    public struct StockCommand
    {
        private readonly string _content;

        public StockCommand(string content)
        {
            _content = content;
        }

        public override string ToString()
        {
            return _content;
        }

        public static StockCommand Buy(int price, int number)
        {
            return new StockCommand($"Command: BUY; Price: {price}; Number: {number}");
        }

        public static StockCommand None()
        {
            return new StockCommand(string.Empty);
        }

        public static bool operator ==(StockCommand left, StockCommand right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StockCommand left, StockCommand right)
        {
            return !(left == right);
        }
    }
}
