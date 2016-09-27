using AutoBuyer.Logic.Connections;
using AutoBuyer.Logic.Database;

namespace AutoBuyer.UI
{
    public partial class App
    {
        private const string BuyerName = "Buyer";
        private const string ConnectionString = @"Server=VLADIMIR-PC\SQL2014;Database=AutoBuyer;Trusted_Connection=true;";

        public App()
        {
            var connection = new WarehouseConnection();
            var mainViewModel = new MainViewModel(BuyerName, connection, new BuyerRepository(ConnectionString));

            var window = new MainWindow
            {
                DataContext = mainViewModel
            };
            window.ShowDialog();
        }
    }
}
