using System.Collections.Generic;

namespace AutoBuyer.Logic
{
    public class WarehouseConnection : IWarehouseConnection
    {
        private readonly List<StockItemConnection> _connections = new List<StockItemConnection>();

        public IStockItemConnection ConnectToItem(string itemId, string buyerName)
        {
            var connection = new StockItemConnection(buyerName);
            _connections.Add(connection);

            return connection;
        }
    }
}
