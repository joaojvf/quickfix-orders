using AutoFixture;
using FluentAssertions;
using Moq;
using QuickFix.Fields;
using QuickFix.FIX44;
using QuickFixOrders.Core.Constants;
using QuickFixOrders.Core.Data;
using QuickFixOrders.Core.Entities;
using QuickFixOrders.Core.UseCases.ExecuteOrder;

namespace QuickFixOrders.Core.Tests.UseCases
{
    public class ExecuteOrderHandlerTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IStockRepository> _repositoryMock = new();
        private readonly ExecuteOrderHandler _handler;

        public ExecuteOrderHandlerTests()
        {
            _handler = new ExecuteOrderHandler(_repositoryMock.Object);
        }

        [Fact]
        public void OnHandler_GivenANonExistOrder_ShouldReturnNotExecutable()
        {
            //Arrange
            var order = _fixture.Create<NewOrderSingle>();
            var request = _fixture.Build<ExecuteOrderRequest>()
                .With(x => x.OrderSingle, order)
                .Create();

            _repositoryMock.Setup(x =>
                x.GetStockBySymbol(It.IsAny<string>()))
                .Returns(() => null)
                .Verifiable();

            //Act
            var res = _handler.Handle(request);

            //Assert
            res.Should().BeOfType<ExecuteOrderResponse>();
            res.Executed.Should().BeFalse();
            _repositoryMock.Verify();
        }

        [Theory]
        [InlineData(Side.BUY, 1000, 10, 9000, 0)]
        [InlineData(Side.BUY, 1, 10, 1000, 1000)]
        [InlineData(Side.SELL, 1000, 10, -1000, 1000)]
        [InlineData(Side.SELL, 1000, 10, 1000, 0)]
        [InlineData(Side.CROSS, 0, 0, 0, 0)]
        public void OnHandler_GivenAValidRequest_ShouldReturnNotExecutable(char orderSide, decimal orderPrice, int orderQuantity, decimal financialExpositionLimit, decimal financialExposition)
        {
            //Arrange
            var order = _fixture.Build<NewOrderSingle>()
                .With(x => x.Symbol, new Symbol(Symbols.VALE3))
                .With(x => x.Side, new Side(orderSide))
                .With(x => x.Price, new Price(orderPrice))
                .With(x => x.OrderQty, new OrderQty(orderQuantity))
                .Create();

            var request = _fixture.Build<ExecuteOrderRequest>()
                .With(x => x.OrderSingle, order)
                .Create();

            var stock = _fixture.Build<Stock>()
                .With(x => x.FinancialExpositionBuyLimit, financialExpositionLimit)
                .With(x => x.FinancialExpositionSellLimit, financialExpositionLimit)
                .With(x => x.FinancialExposition, financialExposition)
                .Create();

            _repositoryMock.Setup(x =>
                x.GetStockBySymbol(It.IsAny<string>()))
                .Returns(stock)
                .Verifiable();

            //Act
            var res = _handler.Handle(request);

            //Assert
            res.Should().BeOfType<ExecuteOrderResponse>();
            res.Executed.Should().BeFalse();
            _repositoryMock.Verify();
        }

        [Theory]
        [InlineData(Side.BUY, 1000, 10, 100000, 1000)]
        [InlineData(Side.BUY, 1000, 10, 10000, -1000)]
        [InlineData(Side.BUY, 1000, 10, 10000, 0)]
        [InlineData(Side.SELL, 1000, 10, -100000, 1000)]
        [InlineData(Side.SELL, 1000, 10, -10000, 1000)]
        [InlineData(Side.SELL, 1000, 10, -10000, 0)]
        public void OnHandler_GivenAValidRequest_ShouldReturnExecutable(char orderSide, decimal orderPrice, int orderQuantity, decimal financialExpositionLimit, decimal financialExposition)
        {
            //Arrange
            var order = _fixture.Build<NewOrderSingle>()
                .With(x => x.Symbol, new Symbol(Symbols.VALE3))
                .With(x => x.Side, new Side(orderSide))
                .With(x => x.Price, new Price(orderPrice))
                .With(x => x.OrderQty, new OrderQty(orderQuantity))
                .Create();

            var request = _fixture.Build<ExecuteOrderRequest>()
                .With(x => x.OrderSingle, order)
                .Create();

            var stock = _fixture.Build<Stock>()
                .With(x => x.FinancialExpositionBuyLimit, financialExpositionLimit)
                .With(x => x.FinancialExpositionSellLimit, financialExpositionLimit)
                .With(x => x.FinancialExposition, financialExposition)
                .Create();

            _repositoryMock.Setup(x =>
                x.GetStockBySymbol(It.IsAny<string>()))
                .Returns(stock);


            //Act
            var res = _handler.Handle(request);

            //Assert
            res.Should().BeOfType<ExecuteOrderResponse>();
            res.Executed.Should().BeTrue();
            _repositoryMock.Verify(x => x.GetStockBySymbol(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }

    }
}
