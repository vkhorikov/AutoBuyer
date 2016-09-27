namespace AutoBuyer.Logic
{
    public class StockItem : IStockItem
    {
        private const string BuyCommandFormat = "Command: BUY; Price: {0}; Number: {1}";

        private readonly IStockItemConnection _connection;
        private readonly StockMessageTranslator _translator;

        public StockItem(IStockItemConnection connection)
        {
            _connection = connection;
            _translator = new StockMessageTranslator(connection.BuyerName);
            _connection.MessageReceived += TranslateMessage;
        }

        public void Buy(int price, int numberToBuy)
        {
            _connection.SendMessage(string.Format(BuyCommandFormat, price, numberToBuy));
        }

        private void TranslateMessage(string message)
        {
            _translator.ProcessMessage(message);
        }

        public void AddStockEventListener(IStockEventListener listener)
        {
            _translator.AddStockEventListener(listener);
        }
    }
}
