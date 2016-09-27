using System.Collections.Generic;

namespace AutoBuyer.Logic
{
    public class BuyerPortfolio : IBuyerPortfolio
    {
        private readonly List<IPortfolioListener> _listeners = new List<IPortfolioListener>();
        private readonly List<Buyer> _buyers = new List<Buyer>();

        public void AddBuyer(Buyer buyer)
        {
            _buyers.Add(buyer);
            foreach (IPortfolioListener listener in _listeners)
            {
                listener.BuyerAdded(buyer);
            }
        }

        public void AddPortfolioListener(IPortfolioListener listener)
        {
            _listeners.Add(listener);
        }
    }
}
