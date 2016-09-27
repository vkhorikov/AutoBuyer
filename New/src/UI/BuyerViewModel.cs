using AutoBuyer.Logic.Connections;
using AutoBuyer.Logic.Database;
using AutoBuyer.Logic.Domain;

namespace AutoBuyer.UI
{
    public class BuyerViewModel : ViewModel
    {
        private readonly Buyer _buyer;
        private readonly IStockItemConnection _connection;
        private readonly BuyerRepository _repository;

        public string ItemId { get; }
        public string CurrentPrice => _buyer.Snapshot.CurrentPrice.ToString();
        public string NumberInStock => _buyer.Snapshot.NumberInStock.ToString();
        public string BoughtSoFar => _buyer.Snapshot.BoughtSoFar.ToString();
        public string State => _buyer.Snapshot.State.ToString();

        public BuyerViewModel(string itemId, int maximumPrice, int numberToBuy,
            string buyerName, IStockItemConnection connection, BuyerRepository repository)
        {
            ItemId = itemId;
            _buyer = new Buyer(buyerName, maximumPrice, numberToBuy);
            _connection = connection;
            _repository = repository;

            _connection.MessageReceived += StockMessageRecieved;
            _repository.Save(ItemId, _buyer);
        }

        private void StockMessageRecieved(string message)
        {
            StockEvent stockEvent = StockEvent.From(message);
            StockCommand stockCommand = _buyer.Process(stockEvent);
            if (stockCommand != StockCommand.None())
            {
                _connection.SendMessage(stockCommand.ToString());
            }

            _repository.Save(ItemId, _buyer);

            Notify(nameof(CurrentPrice));
            Notify(nameof(NumberInStock));
            Notify(nameof(BoughtSoFar));
            Notify(nameof(State));
        }
    }
}
