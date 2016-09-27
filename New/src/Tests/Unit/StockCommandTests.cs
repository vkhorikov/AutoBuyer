using AutoBuyer.Logic.Domain;
using Should;
using Xunit;

namespace Tests.Unit
{
    public class StockCommandTests
    {
        [Fact]
        public void Buy_command_is_of_appropriate_content()
        {
            StockCommand command = StockCommand.Buy(123, 10);

            command.ToString().ShouldEqual("Command: BUY; Price: 123; Number: 10");
        }

        [Fact]
        public void Two_commands_are_equal_if_their_contents_match()
        {
            StockCommand command1 = StockCommand.Buy(123, 10);
            StockCommand command2 = StockCommand.Buy(123, 10);

            bool areEqual1 = command1.Equals(command2);
            bool areEqual2 = command1 == command2;
            bool areEqual3 = command1.GetHashCode() == command2.GetHashCode();

            areEqual1.ShouldBeTrue();
            areEqual2.ShouldBeTrue();
            areEqual3.ShouldBeTrue();
        }
    }
}
