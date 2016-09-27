using AutoBuyer.Logic;

namespace AutoBuyer.UI
{
    public class BuyerViewModel : ViewModel
    {
        public string ItemId { get; }
        public string CurrentPrice { get; private set; }
        public string NumberInStock { get; private set; }
        public string BoughtSoFar { get; private set; }
        public string State { get; private set; }

        public BuyerViewModel(string itemId, BuyerSnapshot snapshot)
        {
            ItemId = itemId;
            UpdateState(snapshot);
        }

        public void UpdateState(BuyerSnapshot snapshot)
        {
            CurrentPrice = snapshot.CurrentPrice.ToString();
            NumberInStock = snapshot.NumberInStock.ToString();
            BoughtSoFar = snapshot.BoughtSoFar.ToString();
            State = snapshot.State.ToString();

            Notify(nameof(CurrentPrice));
            Notify(nameof(NumberInStock));
            Notify(nameof(BoughtSoFar));
            Notify(nameof(State));
        }
    }
}
