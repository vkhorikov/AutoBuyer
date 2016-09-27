namespace AutoBuyer.Logic
{
    public class Warehouse : IWarehouse
    {
        private readonly IWarehouseConnection _connection;
        private readonly string _buyerName;

        public Warehouse(IWarehouseConnection connection, string buyerName)
        {
            _buyerName = buyerName;
            _connection = connection;
        }

        public IStockItem GetStockItemFor(string itemId)
        {
            return new StockItem(_connection.ConnectToItem(itemId, _buyerName));
        }
    }
}
