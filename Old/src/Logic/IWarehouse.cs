namespace AutoBuyer.Logic
{
    public interface IWarehouse
    {
        IStockItem GetStockItemFor(string itemId);
    }
}
