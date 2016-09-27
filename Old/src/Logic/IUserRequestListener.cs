namespace AutoBuyer.Logic
{
    public interface IUserRequestListener
    {
        void StartBuying(string newItemId, int newItemMaximumPrice, int numberToBuy);
    }
}
