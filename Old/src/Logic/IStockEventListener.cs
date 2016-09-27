namespace AutoBuyer.Logic
{
    public interface IStockEventListener
    {
        void CurrentPrice(int price, int numberInStock);
        void ItemPurchased(int numberSold, PurchaseSource purchaseSource);
        void ItemClosed();
    }


    public enum PurchaseSource
    {
        FromBuyer,
        FromOtherBuyer
    }
}
