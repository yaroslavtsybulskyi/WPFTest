using FluentAssertions;
using Logistic.DAL;
using Logistic.Models;
using Moq;
using NSubstitute;
using Xunit;

namespace Logistic.Core.Services.Tests
{
    public class VehicleServiceTests
    {
        private readonly VehicleService _service;
        private readonly IRepository<Vehicle> _repository;

        public VehicleServiceTests()
        {
            _repository = Substitute.For<IRepository<Vehicle>>();
            _service = new VehicleService(_repository);
        }

        [Fact]
        public void Create_WhenVehicleIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Vehicle vehicle = null;

            // Act
            Action action = () => _service.Create(vehicle);

            // Assert
            action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("vehicle");
        }

        [Fact]
        public void Create_WhenVehicleIsValid_CreatesAndReturnsVehicle()
        {
            // Arrange
            var repositoryMock = new Mock<InMemoryRepository<Vehicle>>();
            var service = new VehicleService(repositoryMock.Object);
            var vehicle = new Vehicle(VehicleType.Car, 40, 45);

            repositoryMock.Setup(r => r.Create(vehicle)).Returns(vehicle);

            // Act
            var result = service.Create(vehicle);

            // Assert
            repositoryMock.Verify(r => r.Create(vehicle), Times.Once());
            result.Should().BeEquivalentTo(vehicle);
        }

        [Fact]
        public void GetById_WhenVehicleExists_ReturnsVehicle()
        {
            // Arrange
            var vehicleId = 1;
            var vehicle = new Vehicle(VehicleType.Car, 40, 45) { Id = vehicleId };
            var repository = new InMemoryRepository<Vehicle>();
            repository.Create(vehicle);
            var service = new VehicleService(repository);

            // Act
            var result = service.GetById(vehicleId);

            // Assert
            Assert.Equal(vehicle.Id, result.Id);
        }

        [Fact]
        public void GetById_WhenVehicleDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var repository = new Mock<InMemoryRepository<Vehicle>>();
            var service = new VehicleService(repository.Object);
            var vehicleId = 123;

            repository.Setup(r => r.ReadById(vehicleId)).Returns((Vehicle)null);

            // Act and assert
            Assert.Throws<ArgumentException>(() => service.GetById(vehicleId));
        }


        [Fact]
        public void GetAll_ReturnsAllVehicles()
        {
            // Arrange
            var vehicles = new List<Vehicle>
            {
                new Vehicle(VehicleType.Car, 40, 45),
                new Vehicle(VehicleType.Plane, 50, 60),
                new Vehicle(VehicleType.Train, 30, 35),
            };

            var repository = new InMemoryRepository<Vehicle>();
            repository.Create(vehicles[0]);
            repository.Create(vehicles[1]);
            repository.Create(vehicles[2]);
            var service = new VehicleService(repository);

            // Act
            var result = service.GetAll();

            // Assert
            result.Should().BeEquivalentTo(vehicles);
        }

        [Fact]
        public void Delete_WhenVehicleExists_DeletesVehicle()
        {
            // Arrange
            var repository = Substitute.For<InMemoryRepository<Vehicle>>();
            var service = new VehicleService(repository);
            var vehicleId = 1;
            var vehicle = new Vehicle(VehicleType.Car, 40, 45) { Id = vehicleId };
            repository.ReadById(vehicleId).Returns(vehicle);

            // Act
            service.Delete(vehicleId);

            // Assert
            repository.Received(1).Delete(vehicle);
        }

        [Fact]
        public void LoadCargo_WhenCargoExceedsWeightCapacity_ThrowsArgumentException()
        {
            // Arrange
            var vehicle = new Vehicle(VehicleType.Car, 1000, 50);
            var cargo = new Cargo(25, 1200);
            var repositoryMock = new Mock<InMemoryRepository<Vehicle>>();
            var service = new VehicleService(repositoryMock.Object);
            repositoryMock.Setup(x => x.ReadById(It.IsAny<int>())).Returns(vehicle);

            // Act
            Action act = () => service.LoadCargo(cargo, vehicle.Id);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage($"Cargo with weight {cargo.Weight} cannot be loaded into vehicle with id {vehicle.Id}. The vehicle is already at full capacity");
        }

        [Fact]
        public void LoadCargo_WhenCargoOverVolume_ThrowsArgumentException()
        {
            // Arrange
            var vehicle = new Vehicle(VehicleType.Car, 500, 10);
            var repository = new InMemoryRepository<Vehicle>();
            repository.Create(vehicle);
            var service = new VehicleService(repository);
            var cargo = new Cargo(15, 100); // volume exceeds vehicle capacity
            var vehicleId = vehicle.Id;

            // Act and assert
            Assert.Throws<ArgumentException>(() => service.LoadCargo(cargo, vehicleId));
        }

        [Fact]
        public void LoadCargo_WhenCargoFitsInVehicle_AddsCargoToVehicle()
        {
            // Arrange
            var vehicle = new Vehicle(VehicleType.Car, 1000, 5.5);
            var cargo = new Cargo(1.0, 500);
            var repository = new InMemoryRepository<Vehicle>();
            repository.Create(vehicle);
            var service = new VehicleService(repository);

            // Act
            service.LoadCargo(cargo, vehicle.Id);

            // Assert
            var loadedVehicle = service.GetById(vehicle.Id);
            Assert.Contains(cargo, loadedVehicle.Cargos);
        }

        [Fact]
        public void UnloadCargo_WhenCargoExistsInVehicle_RemovesCargoFromVehicle()
        {
            // Arrange
            var cargoId = Guid.NewGuid();
            var cargo = new Cargo(volume: 100, weight: 50, code: "ABC") { Id = cargoId };
            var vehicle = new Vehicle(VehicleType.Car, 500, 10);
            vehicle.Cargos.Add(cargo);

            var repository = new InMemoryRepository<Vehicle>();
            repository.Create(vehicle);

            var service = new VehicleService(repository);

            // Act
            service.UnloadCargo(cargoId, vehicle.Id);

            // Assert
            var updatedVehicle = service.GetById(vehicle.Id);
            Assert.DoesNotContain(cargo, updatedVehicle.Cargos);
        }
    }
}