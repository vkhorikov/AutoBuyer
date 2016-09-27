using AutoBuyer.Logic;

namespace AutoBuyer.UI
{
    public partial class App
    {
        private const string BuyerName = "Buyer";

        public App()
        {
            var connection = new WarehouseConnection();
            var warehouse = new Warehouse(connection, BuyerName);

            var portfolio = new BuyerPortfolio();
            var launcher = new BuyerLauncher(warehouse, portfolio);
            var mainViewModel = new MainViewModel(portfolio);
            mainViewModel.AddUserRequestListener(launcher);

            var window = new MainWindow
            {
                DataContext = mainViewModel
            };
            window.ShowDialog();
        }
    }
}
