namespace AutoBuyer.Logic
{
    public interface IBuyerListener
    {
        void BuyerStateChanged(BuyerSnapshot snapshot);
    }
}
