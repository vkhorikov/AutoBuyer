using System;
using AutoBuyer.Logic;
using Moq;
using Should;
using Xunit;

namespace Tests
{
    public class StockMessageTranslatorTests
    {
        [Fact]
        public void Notifies_stock_closes_when_close_message_received()
        {
            var sut = new StockMessageTranslator("Buyer");
            var mock = new Mock<IStockEventListener>();
            sut.AddStockEventListener(mock.Object);

            sut.ProcessMessage("Event: CLOSE;");

            mock.Verify(x => x.ItemClosed());
        }

        [Fact]
        public void Notifies_current_price_when_price_message_received()
        {
            var sut = new StockMessageTranslator("Buyer");
            var mock = new Mock<IStockEventListener>();
            sut.AddStockEventListener(mock.Object);

            sut.ProcessMessage("Event: PRICE; CurrentPrice: 12; NumberInStock: 34");

            mock.Verify(x => x.CurrentPrice(12, 34));
        }

        [Fact]
        public void Notifies_item_purchased_by_the_buyer_when_purchase_message_received()
        {
            var sut = new StockMessageTranslator("Buyer");
            var mock = new Mock<IStockEventListener>();
            sut.AddStockEventListener(mock.Object);

            sut.ProcessMessage("Event: PURCHASE; BuyerName: Buyer; NumberSold: 1");

            mock.Verify(x => x.ItemPurchased(1, PurchaseSource.FromBuyer));
        }

        [Fact]
        public void Notifies_item_purchased_by_other_buyer_when_purchase_message_received()
        {
            var sut = new StockMessageTranslator("Buyer");
            var mock = new Mock<IStockEventListener>();
            sut.AddStockEventListener(mock.Object);

            sut.ProcessMessage("Event: PURCHASE; BuyerName: OtherBuyer; NumberSold: 1");

            mock.Verify(x => x.ItemPurchased(1, PurchaseSource.FromOtherBuyer));
        }

        [Fact]
        public void Throws_when_incorrect_message_received()
        {
            var sut = new StockMessageTranslator("Buyer");
            var mock = new Mock<IStockEventListener>(MockBehavior.Strict);
            sut.AddStockEventListener(mock.Object);

            Action action = () => sut.ProcessMessage("incorrect message");

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Throws_when_message_of_unknown_type_received()
        {
            var sut = new StockMessageTranslator("Buyer");
            var mock = new Mock<IStockEventListener>(MockBehavior.Strict);
            sut.AddStockEventListener(mock.Object);

            Action action = () => sut.ProcessMessage("Event: UNKNOWN;");

            action.ShouldThrow<ArgumentException>();
        }
    }
}
