namespace AutoBuyer.Logic
{
    public class BuyerLauncher : IUserRequestListener
    {
        private readonly IWarehouse _warehouse;
        private readonly IBuyerPortfolio _portfolio;

        public BuyerLauncher(IWarehouse warehouse, IBuyerPortfolio portfolio)
        {
            _portfolio = portfolio;
            _warehouse = warehouse;
        }

        public void StartBuying(string newItemId, int newItemMaximumPrice, int numberToBuy)
        {
            IStockItem stockItem = _warehouse.GetStockItemFor(newItemId);
            var buyer = new Buyer(newItemId, newItemMaximumPrice, numberToBuy, stockItem);
            stockItem.AddStockEventListener(buyer);
            _portfolio.AddBuyer(buyer);
        }
    }
}
