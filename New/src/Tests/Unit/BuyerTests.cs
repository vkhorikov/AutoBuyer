using AutoBuyer.Logic.Domain;
using Should;
using Xunit;

namespace Tests.Unit
{
    public class BuyerTests
    {
        [Fact]
        public void New_buyer_is_in_joining_state()
        {
            var buyer = new Buyer("name", 34, 1);

            buyer.SnapshotShouldEqual(BuyerState.Joining, 0, 0, 0);
        }

        [Fact]
        public void Closes_when_item_closes()
        {
            var buyer = CreateJoiningBuyer();

            StockCommand command = buyer.Process(StockEvent.Close());

            command.ShouldEqual(StockCommand.None());
            buyer.SnapshotShouldEqual(BuyerState.Closed, 0, 0, 0);
        }

        [Fact]
        public void Buyer_does_not_buy_when_price_event_with_too_high_price_arrives()
        {
            var buyer = CreateJoiningBuyer(maximumPrice: 10);

            StockCommand command = buyer.Process(StockEvent.Price(20, 5));

            command.ShouldEqual(StockCommand.None());
            buyer.SnapshotShouldEqual(BuyerState.Monitoring, 20, 5, 0);
        }

        [Fact]
        public void Buyer_buys_when_price_event_with_appropriate_price_arrives()
        {
            Buyer buyer = CreateJoiningBuyer(maximumPrice: 50);

            StockCommand command = buyer.Process(StockEvent.Price(10, 5));

            command.ShouldEqual(StockCommand.Buy(10, 1));
            buyer.SnapshotShouldEqual(BuyerState.Buying, 10, 5, 0);
        }

        [Fact]
        public void Buyer_attempts_to_buy_maximum_amount_available()
        {
            Buyer buyer = CreateJoiningBuyer(maximumPrice: 50, numberToBuy: 10);

            StockCommand command = buyer.Process(StockEvent.Price(10, 5));

            command.ShouldEqual(StockCommand.Buy(10, 5));
            buyer.SnapshotShouldEqual(BuyerState.Buying, 10, 5, 0);
        }

        [Fact]
        public void Buyer_does_not_react_to_a_purchase_event_related_to_another_buyer()
        {
            Buyer buyer = CreateMonitoringBuyer("Buyer");

            StockCommand command = buyer.Process(StockEvent.Purchase("Some other buyer", 1));

            command.ShouldEqual(StockCommand.None());
            buyer.Snapshot.State.ShouldEqual(BuyerState.Monitoring);
            buyer.Snapshot.BoughtSoFar.ShouldEqual(0);
        }

        [Fact]
        public void Buyer_updates_items_bought_so_far_when_purchase_event_with_the_same_user_name_arrives()
        {
            Buyer buyer = CreateMonitoringBuyer("name", numberInStock: 10);

            StockCommand command = buyer.Process(StockEvent.Purchase("name", 1));

            command.ShouldEqual(StockCommand.None());
            buyer.Snapshot.State.ShouldEqual(BuyerState.Monitoring);
            buyer.Snapshot.BoughtSoFar.ShouldEqual(1);
            buyer.Snapshot.NumberInStock.ShouldEqual(9);
        }

        [Fact]
        public void Buyer_closes_when_it_buys_enough_items()
        {
            Buyer buyer = CreateMonitoringBuyer("Buyer", numberToBuy: 5);

            StockCommand command = buyer.Process(StockEvent.Purchase("Buyer", 5));

            command.ShouldEqual(StockCommand.None());
            buyer.Snapshot.State.ShouldEqual(BuyerState.Closed);
        }

        [Fact]
        public void Closed_buyer_does_not_react_to_further_messages()
        {
            Buyer buyer = CreateClosedBuyer(maximumPrice: 10);

            StockCommand command = buyer.Process(StockEvent.Price(10, 10));

            command.ShouldEqual(StockCommand.None());
            buyer.Snapshot.State.ShouldEqual(BuyerState.Closed);
        }

        private Buyer CreateClosedBuyer(int maximumPrice = 5)
        {
            var buyer = new Buyer("Buyer", maximumPrice, 1);
            buyer.Process(StockEvent.Close());
            return buyer;
        }

        private Buyer CreateMonitoringBuyer(string buyerName, int numberInStock = 100, int numberToBuy = 10)
        {
            var buyer = new Buyer(buyerName, 100, numberToBuy);
            buyer.Process(StockEvent.Price(200, numberInStock));
            return buyer;
        }

        private Buyer CreateJoiningBuyer(int maximumPrice = 100, int numberToBuy = 1)
        {
            return new Buyer("", maximumPrice, numberToBuy);
        }
    }


    internal static class BuyerExtentions
    {
        public static void SnapshotShouldEqual(this Buyer buyer, BuyerState state, int currentPrice,
            int numberInStock, int boughtSoFar)
        {
            buyer.Snapshot.State.ShouldEqual(state);
            buyer.Snapshot.CurrentPrice.ShouldEqual(currentPrice);
            buyer.Snapshot.NumberInStock.ShouldEqual(numberInStock);
            buyer.Snapshot.BoughtSoFar.ShouldEqual(boughtSoFar);
        }
    }
}
