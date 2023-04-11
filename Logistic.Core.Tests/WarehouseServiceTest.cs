using AutoFixture;
using FluentAssertions;
using Logistic.Core.Services;
using Logistic.DAL;
using Logistic.Models;
using Moq;
using NSubstitute;
using Xunit;

namespace Logistic.Core.Services.Tests
{

    public class WarehouseServiceTests
    {
        [Fact]
        public void Create_WhenValidWarehouse_ReturnsCreatedWarehouse()
        {
            // Arrange
            var repository = new InMemoryRepository<Warehouse>();
            var service = new WarehouseService(repository);
            var warehouse = new Warehouse();

            // Act
            var createdWarehouse = service.Create(warehouse);

            // Assert
            Assert.NotNull(createdWarehouse);
            Assert.Equal(warehouse.CargoList, createdWarehouse.CargoList);
        }

        [Fact]
        public void GetById_WhenWarehouseExists_ReturnsWarehouse()
        {
            // Arrange
            var warehouse = new Warehouse();
            warehouse.Id = 1;
            var repository = new Mock<InMemoryRepository<Warehouse>>();
            repository.Setup(x => x.ReadById(warehouse.Id)).Returns(warehouse);
            var service = new WarehouseService(repository.Object);

            // Act
            var result = service.GetById(warehouse.Id);

            // Assert
            Assert.Equal(warehouse, result);
        }

        [Fact]
        public void GetAll_ReturnsAllWarehouses()
        {
            // Arrange
            var warehouseList = new List<Warehouse>
            {
                new Warehouse(),
                new Warehouse(),
                new Warehouse()
            };
            var repositoryMock = new Mock<InMemoryRepository<Warehouse>>();
            repositoryMock.Setup(r => r.ReadAll()).Returns(warehouseList);
            var service = new WarehouseService(repositoryMock.Object);

            // Act
            var result = service.GetAll();

            // Assert
            Assert.Equal(warehouseList, result);
            Assert.Equal(warehouseList.Count, result.Count());
            Assert.Equal(warehouseList[0], result.ElementAt(0));
            Assert.Equal(warehouseList[1], result.ElementAt(1));
            Assert.Equal(warehouseList[2], result.ElementAt(2));
        }

        [Fact]
        public void Delete_ExistingWarehouse_DeletesWarehouse()
        {
            // Arrange
            int existingWarehouseId = 1;
            var mockRepo = new Mock<InMemoryRepository<Warehouse>>();
            var service = new WarehouseService(mockRepo.Object);

            var existingWarehouse = new Warehouse()
            {
                Id = existingWarehouseId,
                CargoList = new List<Cargo>()
            };
            mockRepo.Setup(r => r.ReadById(existingWarehouseId)).Returns(existingWarehouse);

            // Act
            service.Delete(existingWarehouseId);

            // Assert
            mockRepo.Verify(r => r.Delete(existingWarehouseId), Times.Once);
        }

        [Fact]
        public void LoadCargo_WhenCargoFitsInWarehouse_AddsCargoToList()
        {
            // Arrange
            var fixture = new Fixture();
            var warehouseId = 1;
            var existingCargoList = fixture.CreateMany<Cargo>(3).ToList();
            var warehouse = fixture.Build<Warehouse>()
                .With(w => w.Id, warehouseId)
                .With(w => w.CargoList, existingCargoList)
                .Create();
            var cargo = fixture.Create<Cargo>();

            var repositoryMock = new Mock<InMemoryRepository<Warehouse>>();
            repositoryMock.Setup(r => r.ReadById(warehouseId)).Returns(warehouse);

            var service = new WarehouseService(repositoryMock.Object);

            // Act
            service.LoadCargo(cargo, warehouseId);

            // Assert
            warehouse.CargoList.Should().Contain(cargo);
            repositoryMock.Verify(r => r.Update(warehouseId, warehouse), Times.Once);
        }

        [Fact]
        public void UnloadCargo_ValidCargoIdAndWarehouseId_RemovesCargoFromWarehouse()
        {
            // Arrange
            var warehouseId = 1;
            var cargoId = Guid.NewGuid();
            var cargos = new List<Cargo> { new Cargo { Id = cargoId, Volume = 10, Weight = 100 } };
            var warehouse = new Warehouse { Id = warehouseId, CargoList = cargos };
            var mockRepository = new Mock<InMemoryRepository<Warehouse>>();
            mockRepository.Setup(r => r.ReadById(warehouseId)).Returns(warehouse);
            var service = new WarehouseService(mockRepository.Object);

            // Act
            service.UnloadCargo(cargoId, warehouseId);

            // Assert
            mockRepository.Verify(r => r.Update(warehouseId, It.Is<Warehouse>(w => !w.CargoList.Any(c => c.Id == cargoId))), Times.Once);
        }
    }
}