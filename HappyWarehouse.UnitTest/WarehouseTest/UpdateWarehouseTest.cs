using System.Data;
using System.Linq.Expressions;
using System.Net;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.UpdateWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.UOF;
using Moq;
using Serilog;

namespace HappyWarehouse.UnitTest.WarehouseTest;

public class UpdateWarehouseTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<UpdateWarehouseDto>> _validatorMock;
    private readonly Mock<IWarehouseRepository> _warehouseRepositoryMock;
    private readonly Mock<IDbTransaction> _dbTransactionMock;
    private readonly Mock<ILogger> _loggerMock;
    
    public UpdateWarehouseTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<UpdateWarehouseDto>>();
        _warehouseRepositoryMock = new Mock<IWarehouseRepository>();
        _dbTransactionMock = new Mock<IDbTransaction>();
        _loggerMock = new Mock<ILogger>();
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenWarehouseIsUpdated()
    {
        // Arrange
        var existingWarehouse = Warehouse.Create("OldName", "OldAddress", "OldCity", 1, 1, "Seeder");
        existingWarehouse.Id = 10;
        
        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(_dbTransactionMock.Object);
        _unitOfWorkMock.Setup(u => u.GetWarehouseRepository).Returns(_warehouseRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateWarehouseDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        _warehouseRepositoryMock
            .SetupSequence(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Warehouse, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingWarehouse)!  
            .ReturnsAsync((Warehouse?)null); 

        _warehouseRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Warehouse>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        
        
        var dto = new UpdateWarehouseDto("NewName", "NewAddress", "NewCity", 2, "Seeder");

        var command = new UpdateWarehouseCommand(10, dto);

        var handler = new UpdateWarehouseCommandHandler(
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _validatorMock.Object
        );

        // Act
        var result = await handler.HandleAsync(command);
        
        // Assert
        result.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Message.Should().Contain("Warehouse updated successfully.");
        existingWarehouse.Name.Should().Be("NewName");
    }
}