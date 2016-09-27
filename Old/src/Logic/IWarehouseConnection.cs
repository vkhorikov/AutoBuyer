namespace AutoBuyer.Logic
{
    public interface IWarehouseConnection
    {
        IStockItemConnection ConnectToItem(string itemId, string buyerName);
    }
}
