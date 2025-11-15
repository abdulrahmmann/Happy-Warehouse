using System.Data;
using FluentAssertions;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.DeleteWarehouse;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.UOF;
using Moq;
using Serilog;

namespace HappyWarehouse.UnitTest.WarehouseTest;

public class DeleteWarehouseTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<IWarehouseRepository> _warehouseRepositoryMock;
    private readonly Mock<IDbTransaction> _transactionMock;
    
    public DeleteWarehouseTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger>();
        _warehouseRepositoryMock = new Mock<IWarehouseRepository>();
        _transactionMock = new Mock<IDbTransaction>();

        _unitOfWorkMock.Setup(u => u.GetWarehouseRepository).Returns(_warehouseRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(_transactionMock.Object);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnValidationError_WhenIdIsZero()
    {
        // Arrange
        var command = new SoftDeleteWarehouseCommand(0);
        var handler = new SoftDeleteWarehouseCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.HttpStatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Message.Should().Contain("Id must be greater than zero");
    }
}