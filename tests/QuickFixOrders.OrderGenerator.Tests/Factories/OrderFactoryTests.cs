using FluentAssertions;
using QuickFix.Fields;
using QuickFixOrders.Core.Constants;
using QuickFixOrders.OrderGenerator.Factories;

namespace QuickFixOrders.OrderGenerator.Tests.Factories
{
    public class OrderFactoryTests
    {
        private readonly char[] _sides = { Side.SELL, Side.BUY };

        [Fact]
        public void OnCreateOrder44_GivenValidSymbol_ShouldCreateOrdersRespectingRules()
        {
            //Act
            var orders = Enumerable.Range(1, 50)
                .Select(x => OrderFactory.CreateOrder44(Symbols.VALE3));

            //Assert
            orders.Should().AllSatisfy(x =>
            {
                x.Side.getValue().Should().BeOneOf(_sides);
                x.Price.getValue().Should().BeInRange((decimal)0.1, 10000);
                x.OrderQty.getValue().Should().BeInRange(1,1000001);
            });
        }
    }
}
