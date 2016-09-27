using System.Data.SqlClient;
using AutoBuyer.Logic.Domain;

namespace AutoBuyer.Logic.Database
{
    public class BuyerRepository
    {
        private readonly string _connectionString;

        public BuyerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Save(string itemId, Buyer buyer)
        {
            string text = @"
                IF EXISTS (SELECT TOP 1 1 FROM dbo.[Buyer] WHERE [ItemID] = @ItemID)
                BEGIN
                    UPDATE dbo.[Buyer]
                    SET [BuyerName] = @BuyerName,
                        [MaximumPrice] = @MaximumPrice,
                        [NumberToBuy] = @NumberToBuy,
                        [CurrentPrice] = @CurrentPrice,
                        [NumberInStock] = @NumberInStock,
                        [BoughtSoFar] = @BoughtSoFar,
                        [State] = @State
                    WHERE ItemID = @ItemID
                END
                ELSE
                BEGIN
                    INSERT dbo.[Buyer] (ItemID, BuyerName, MaximumPrice, NumberToBuy,
                        CurrentPrice, NumberInStock, BoughtSoFar, State)
                    VALUES (@ItemID, @BuyerName, @MaximumPrice, @NumberToBuy,
                        @CurrentPrice, @NumberInStock, @BoughtSoFar, @State)
                END";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = text;
                command.Parameters.AddWithValue("@ItemID", itemId);
                command.Parameters.AddWithValue("@BuyerName", buyer.BuyerName);
                command.Parameters.AddWithValue("@MaximumPrice", buyer.MaximumPrice);
                command.Parameters.AddWithValue("@NumberToBuy", buyer.NumberToBuy);
                command.Parameters.AddWithValue("@CurrentPrice", buyer.Snapshot.CurrentPrice);
                command.Parameters.AddWithValue("@NumberInStock", buyer.Snapshot.NumberInStock);
                command.Parameters.AddWithValue("@BoughtSoFar", buyer.Snapshot.BoughtSoFar);
                command.Parameters.AddWithValue("@State", (int)buyer.Snapshot.State);

                command.ExecuteNonQuery();
            }
        }
    }
}
