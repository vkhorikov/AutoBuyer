using System.Collections.Generic;

namespace AutoBuyer.Logic.Connections
{
    public class WarehouseConnection : IWarehouseConnection
    {
        private readonly List<StockItemConnection> _connections = new List<StockItemConnection>();

        public IStockItemConnection ConnectToItem(string itemId)
        {
            var connection = new StockItemConnection();
            _connections.Add(connection);

            return connection;
        }
    }
}
