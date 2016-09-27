using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoBuyer.Logic
{
    public class StockEvent
    {
        public StockEventType Type { get; }
        private readonly Dictionary<string, string> _fields;

        public int CurrentPrice => GetInt("CurrentPrice");
        public int NumberInStock => GetInt("NumberInStock");
        public int NumberSold => GetInt("NumberSold");
        public string BuyerName => Get("BuyerName");

        private StockEvent(StockEventType type, Dictionary<string, string> fields)
        {
            Type = type;
            _fields = fields;
        }

        private int GetInt(string fieldName)
        {
            return int.Parse(Get(fieldName));
        }

        private string Get(string fieldName)
        {
            string value = _fields[fieldName];

            if (value == null)
                throw new InvalidOperationException(fieldName);

            return value;
        }

        public override string ToString()
        {
            return string.Join(" ", _fields.Select(x => x.Key + ": " + x.Value + ";"));
        }

        public static StockEvent From(string message)
        {
            if (!message.Contains(":") || !message.Contains(";"))
                throw new ArgumentException();

            Dictionary<string, string> fields = GetFields(message);
            StockEventType eventType = GetEventType(fields);

            return new StockEvent(eventType, fields);
        }

        private static StockEventType GetEventType(Dictionary<string, string> fields)
        {
            if (!fields.ContainsKey("Event"))
                throw new ArgumentException();

            StockEventType type;
            if (!Enum.TryParse(fields["Event"], true, out type))
                throw new ArgumentException();

            return type;
        }

        private static Dictionary<string, string> GetFields(string message)
        {
            var fields = new Dictionary<string, string>();

            string[] pairs = message.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pair in pairs)
            {
                string[] data = pair.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                fields.Add(data[0].Trim(), data[1].Trim());
            }
            
            return fields;
        }
    }


    public enum StockEventType
    {
        Price,
        Purchase,
        Close
    }
}
