using System.Data;
using System.Linq.Expressions;
using System.Net;
using FluentAssertions;
using FluentValidation;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.CreateWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.UOF;
using Moq;
using Serilog;

namespace HappyWarehouse.UnitTest.WarehouseTest;

public class CreateWarehouseTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<IValidator<CreateWarehouseDto>>  _validatorMock;
    private readonly Mock<IWarehouseRepository>  _warehouseRepositoryMock;
    private readonly Mock<IDbTransaction> _dbTransactionMock;
    private readonly Mock<IGenericRepository<Warehouse>>  _genericRepositoryMock;

    public CreateWarehouseTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger>();
        _warehouseRepositoryMock = new Mock<IWarehouseRepository>();
        _validatorMock = new Mock<IValidator<CreateWarehouseDto>>();
        _dbTransactionMock =  new Mock<IDbTransaction>();
        _genericRepositoryMock = new Mock<IGenericRepository<Warehouse>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenIsNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(_dbTransactionMock.Object);
        _dbTransactionMock.Setup(t => t.Commit());
        _dbTransactionMock.Setup(t => t.Rollback());

        // Generic repository
        _genericRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Warehouse>())).Returns(Task.CompletedTask);
        
        // UnitOfWork
        _unitOfWorkMock.Setup(u => u.GetRepository<Warehouse>()).Returns(_genericRepositoryMock.Object);
        
        // Warehouse repository
        _unitOfWorkMock.Setup(u => u.GetWarehouseRepository).Returns(_warehouseRepositoryMock.Object);
        
        _warehouseRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Warehouse, bool>>>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync((Warehouse?)null);
        
        // Validator
        var validationResult = new FluentValidation.Results.ValidationResult(); 
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateWarehouseDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        // SaveChangesAsync
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Command
        var command = new CreateWarehouseCommand(new CreateWarehouseDto("NewWarehouse", "Address", "City", 2, 2, "Seeder"));
        var handler = new CreateWarehouseCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _validatorMock.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.HttpStatusCode.Should().Be(HttpStatusCode.Created);
    }

}