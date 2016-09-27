using AutoBuyer.Logic.Connections;
using AutoBuyer.Logic.Database;
using AutoBuyer.Logic.Domain;
using AutoBuyer.UI;
using Moq;
using Should;
using Tests.Utils;
using Xunit;

namespace Tests.Integration
{
    public class BuyerViewModelTests : Tests
    {
        [Fact]
        public void Joining_a_sale()
        {
            var connectionMock = new Mock<IStockItemConnection>();

            new BuyerViewModel("ItemId", 10, 1, "Buyer",
                connectionMock.Object, new BuyerRepository(ConnectionString));

            BuyerDto buyer = DB.GetBuyer("ItemId");
            buyer.BuyerName.ShouldEqual("Buyer");
            buyer.MaximumPrice.ShouldEqual(10);
            buyer.NumberToBuy.ShouldEqual(1);
            buyer.State.ShouldEqual(BuyerState.Joining);
            buyer.BoughtSoFar.ShouldEqual(0);
            buyer.CurrentPrice.ShouldEqual(0);
            buyer.NumberInStock.ShouldEqual(0);
        }

        [Fact]
        public void Buying_an_item()
        {
            var connectionMock = new Mock<IStockItemConnection>();
            new BuyerViewModel("ItemId", 10, 1, "Buyer", connectionMock.Object,
                new BuyerRepository(ConnectionString));

            connectionMock.Raise(x => x.MessageReceived += null,
                "Event: PRICE; CurrentPrice: 5; NumberInStock: 5");

            BuyerDto buyer = DB.GetBuyer("ItemId");
            buyer.BuyerName.ShouldEqual("Buyer");
            buyer.MaximumPrice.ShouldEqual(10);
            buyer.NumberToBuy.ShouldEqual(1);
            buyer.State.ShouldEqual(BuyerState.Buying);
            buyer.BoughtSoFar.ShouldEqual(0);
            buyer.CurrentPrice.ShouldEqual(5);
            buyer.NumberInStock.ShouldEqual(5);
            connectionMock.Verify(x => x.SendMessage("Command: BUY; Price: 5; Number: 1"));
        }
    }
}
