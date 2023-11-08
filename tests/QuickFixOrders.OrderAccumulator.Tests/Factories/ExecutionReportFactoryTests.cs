using FluentAssertions;
using QuickFix.Fields;
using QuickFix.FIX44;
using QuickFixOrders.Core.Constants;

namespace QuickFixOrders.OrderAccumulator.Tests.Factories
{
    public class ExecutionReportFactoryTests
    {
        [Theory]
        [InlineData(true, ExecType.FILL, OrdStatus.FILLED)]
        [InlineData(false, ExecType.REJECTED, OrdStatus.REJECTED)]

        public void OnCreateOrder44_GivenValidInputs_ShouldCreateReportsRespectingRules(bool executed, char execType, char ordStatus)
        {
            //Arrange            
            var orders = Enumerable.Range(1, 3).Select(x => CreateOrderSingle(x.ToString())).ToList();

            //Act
            var reports = orders.Select(x => 
                    ExecutionReportFactory.CreateExecutionReport44(x,executed))
                .ToList();

            //Assert
            reports.Should().AllSatisfy(x =>
            {
                x.ExecType.getValue().Should().Be(execType);
                x.OrdStatus.getValue().Should().Be(ordStatus);
            });
        }

        private NewOrderSingle CreateOrderSingle(string id)
        {
            var idDecimal = Convert.ToDecimal(id);
            return new NewOrderSingle(
               new ClOrdID(id),
               new Symbol(Symbols.VALE3),
               new Side(Side.BUY),
               new TransactTime(DateTime.Now),
               new OrdType(OrdType.MARKET))
            {
                Price = new Price(idDecimal),
                OrderQty = new OrderQty(idDecimal)
            };
        }
    }
}
