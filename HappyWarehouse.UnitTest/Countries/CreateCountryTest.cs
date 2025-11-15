using System.Data;
using System.Linq.Expressions;
using System.Net;
using FluentAssertions;
using HappyWarehouse.Application.Features.CountryFeature.Commands.CreateCountry;
using HappyWarehouse.Application.Features.CountryFeature.DTOs;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.UOF;
using Moq;
using Serilog;

namespace HappyWarehouse.UnitTest.Countries;

public class CreateCountryTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<ICountryRepository> _countryRepositoryMock;

    public CreateCountryTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger>();
        _countryRepositoryMock = new Mock<ICountryRepository>();
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenIsExist()
    {
        // Arrange
        var command = new CreateCountryCommand(new CreateCountryDto("Spain", "Seeder"));
        var handler = new CreateCountryCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        
        var existingCountry = Country.Create("Spain", "Seeder");
        
        var transactionMock = new Mock<IDbTransaction>();
        
        var genericRepoMock = new Mock<IGenericRepository<Country>>();
        
        _unitOfWorkMock
            .Setup(u => u.BeginTransaction())
            .Returns(transactionMock.Object);
        
        _unitOfWorkMock
            .Setup(u => u.GetCountryRepository)
            .Returns(_countryRepositoryMock.Object);

        _countryRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Country, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCountry);
        
        _unitOfWorkMock
            .Setup(u => u.GetRepository<Country>())
            .Returns(genericRepoMock.Object);
        
        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.HttpStatusCode.Should().Be(HttpStatusCode.Conflict);

    }
}