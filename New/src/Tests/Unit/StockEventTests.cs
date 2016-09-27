using System;
using AutoBuyer.Logic.Domain;
using Should;
using Xunit;

namespace Tests.Unit
{
    public class StockEventTests
    {
        [Fact]
        public void Parses_close_event()
        {
            string message = "Event: CLOSE;";

            StockEvent stockEvent = StockEvent.From(message);
            string serialized = stockEvent.ToString();

            stockEvent.Type.ShouldEqual(StockEventType.Close);
            serialized.ShouldEqual(message);
        }

        [Fact]
        public void Parses_price_event()
        {
            string message = "Event: PRICE; NumberInStock: 12; CurrentPrice: 34;";

            StockEvent stockEvent = StockEvent.From(message);
            string serialized = stockEvent.ToString();

            stockEvent.Type.ShouldEqual(StockEventType.Price);
            stockEvent.NumberInStock.ShouldEqual(12);
            stockEvent.CurrentPrice.ShouldEqual(34);
            serialized.ShouldEqual(message);
        }

        [Fact]
        public void Parses_purchase_event()
        {
            string message = "Event: PURCHASE; BuyerName: Buyer; NumberSold: 1;";

            StockEvent stockEvent = StockEvent.From(message);
            string serialized = stockEvent.ToString();

            stockEvent.Type.ShouldEqual(StockEventType.Purchase);
            stockEvent.BuyerName.ShouldEqual("Buyer");
            stockEvent.NumberSold.ShouldEqual(1);
            serialized.ShouldEqual(message);
        }

        [Fact]
        public void Does_not_parse_events_with_incorrect_format()
        {
            string message = "incorrect message";

            Action action = () => StockEvent.From(message);

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Does_not_parse_events_with_unknown_types()
        {
            string message = "Event: UNKNOWN;";

            Action action = () => StockEvent.From(message);

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Close_method_returns_a_close_event()
        {
            StockEvent stockEvent = StockEvent.Close();

            stockEvent.Type.ShouldEqual(StockEventType.Close);
            stockEvent.ToString().ShouldEqual("Event: CLOSE;");
        }

        [Fact]
        public void Price_method_returns_a_price_event()
        {
            StockEvent stockEvent = StockEvent.Price(10, 15);

            stockEvent.Type.ShouldEqual(StockEventType.Price);
            stockEvent.CurrentPrice.ShouldEqual(10);
            stockEvent.NumberInStock.ShouldEqual(15);
            stockEvent.ToString().ShouldEqual("Event: PRICE; CurrentPrice: 10; NumberInStock: 15;");
        }
        
        [Fact]
        public void Purchase_method_returns_a_purchase_event()
        {
            StockEvent stockEvent = StockEvent.Purchase("some user", 1);

            stockEvent.Type.ShouldEqual(StockEventType.Purchase);
            stockEvent.BuyerName.ShouldEqual("some user");
            stockEvent.NumberSold.ShouldEqual(1);
            stockEvent.ToString().ShouldEqual("Event: PURCHASE; BuyerName: some user; NumberSold: 1;");
        }
    }
}
