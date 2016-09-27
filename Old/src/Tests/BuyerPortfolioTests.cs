using AutoBuyer.Logic;
using Moq;
using Xunit;

namespace Tests
{
    public class BuyerPortfolioTests
    {
        [Fact]
        public void Notifies_listeners_of_new_buyers()
        {
            var sut = new BuyerPortfolio();
            var mock = new Mock<IPortfolioListener>();
            sut.AddPortfolioListener(mock.Object);
            var buyer = new Buyer("ItemId", 10, 1, null);

            sut.AddBuyer(buyer);

            mock.Verify(x => x.BuyerAdded(buyer));
        }
    }
}
