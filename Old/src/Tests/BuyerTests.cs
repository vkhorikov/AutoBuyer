using AutoBuyer.Logic;
using Moq;
using Xunit;

namespace Tests
{
    public class BuyerTests
    {
        [Fact]
        public void Closes_when_item_closes()
        {
            var mock = new Mock<IBuyerListener>();
            var sut = new Buyer("ItemId", 10, 1, null);
            sut.AddBuyerListener(mock.Object);

            sut.ItemClosed();

            mock.Verify(x => x.BuyerStateChanged(new BuyerSnapshot("ItemId", 0, 0, 0, BuyerState.Closed)));
        }

        [Fact]
        public void Buyer_does_not_buy_when_price_event_with_too_high_price_arrives()
        {
            var mock = new Mock<IBuyerListener>();
            var sut = new Buyer("ItemId", 10, 1, null);
            sut.AddBuyerListener(mock.Object);

            sut.CurrentPrice(20, 5);

            mock.Verify(x => x.BuyerStateChanged(new BuyerSnapshot("ItemId", 20, 5, 0, BuyerState.Monitoring)));
        }

        [Fact]
        public void Buyer_buys_when_price_event_with_appropriate_price_arrives()
        {
            var listener = new Mock<IBuyerListener>();
            var stockItem = new Mock<IStockItem>();
            var sut = new Buyer("ItemId", 50, 1, stockItem.Object);
            sut.AddBuyerListener(listener.Object);

            sut.CurrentPrice(10, 5);

            listener.Verify(x => x.BuyerStateChanged(new BuyerSnapshot("ItemId", 10, 5, 0, BuyerState.Buying)));
            stockItem.Verify(x => x.Buy(10, 1));
        }

        [Fact]
        public void Buyer_attempts_to_buy_maximum_amount_available()
        {
            var stockItem = new Mock<IStockItem>();
            var sut = new Buyer("ItemId", 50, 10, stockItem.Object);

            sut.CurrentPrice(10, 5);

            stockItem.Verify(x => x.Buy(10, 5));
        }

        [Fact]
        public void Buyer_does_not_react_to_a_purchase_event_related_to_another_buyer()
        {
            var stockItem = new Mock<IStockItem>(MockBehavior.Strict);
            var sut = new Buyer("ItemId", 10, 1, stockItem.Object);
            sut.CurrentPrice(100, 5);

            sut.ItemPurchased(1, PurchaseSource.FromOtherBuyer);
        }

        [Fact]
        public void Buyer_updates_items_bought_so_far_when_purchase_event_with_the_same_user_name_arrives()
        {
            var listener = new Mock<IBuyerListener>();
            var sut = new Buyer("ItemId", 10, 10, null);
            sut.AddBuyerListener(listener.Object);
            sut.CurrentPrice(50, 10);

            sut.ItemPurchased(1, PurchaseSource.FromBuyer);

            listener.Verify(x => x.BuyerStateChanged(new BuyerSnapshot("ItemId", 50, 9, 1, BuyerState.Monitoring)));
        }

        [Fact]
        public void Buyer_closes_when_it_buys_enough_items()
        {
            var listener = new Mock<IBuyerListener>();
            var sut = new Buyer("ItemId", 10, 5, null);
            sut.AddBuyerListener(listener.Object);
            sut.CurrentPrice(50, 10);

            sut.ItemPurchased(5, PurchaseSource.FromBuyer);

            listener.Verify(x => x.BuyerStateChanged(new BuyerSnapshot("ItemId", 50, 5, 5, BuyerState.Closed)));
        }

        [Fact]
        public void Closed_buyer_does_not_react_to_further_messages_()
        {
            var listener = new Mock<IBuyerListener>(MockBehavior.Strict);
            listener.Setup(x => x.BuyerStateChanged(new BuyerSnapshot("ItemId", 0, 0, 0, BuyerState.Closed)));
            var stockItem = new Mock<IStockItem>(MockBehavior.Strict);
            var sut = new Buyer("ItemId", 10, 10, stockItem.Object);
            sut.AddBuyerListener(listener.Object);
            sut.ItemClosed();

            sut.CurrentPrice(10, 10);
        }
    }
}
