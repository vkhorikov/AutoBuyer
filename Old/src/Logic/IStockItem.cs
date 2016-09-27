namespace AutoBuyer.Logic
{
    public interface IStockItem
    {
        void Buy(int price, int numberToBuy);
        void AddStockEventListener(IStockEventListener listener);
    }
}
