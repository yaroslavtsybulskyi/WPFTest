using System.Reflection;
using AutoMapper;
using FluentAssertions;
using Logistic.Models;
using Moq;
using Xunit;

namespace Logistic.DAL.Tests
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public void Create_CreatesNewEntityInMemory()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Warehouse>(It.IsAny<Warehouse>()))
                      .Returns<Warehouse>(x => x);

            var entity = new Warehouse(new List<Cargo>() { new Cargo(1, 10), new Cargo(2, 20) });

            var repository = new InMemoryRepository<Warehouse>();

            // Act
            var createdEntity = repository.Create(entity);
            var retrievedEntity = repository.ReadById(createdEntity.Id);

            // Assert
            retrievedEntity.Should().NotBeNull();
            retrievedEntity.Id.Should().Be(createdEntity.Id);
            retrievedEntity.CargoList.Should().BeEquivalentTo(createdEntity.CargoList);
        }

        [Fact]
        public void ReadAll_ReturnsAllEntitiesFromMemory()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<Warehouse>>(It.IsAny<IEnumerable<Warehouse>>()))
                      .Returns<IEnumerable<Warehouse>>(x => x);

            var entities = new List<Warehouse>
            {
            new Warehouse(new List<Cargo> { new Cargo(1, 10), new Cargo(2, 20) }),
            new Warehouse(new List<Cargo> { new Cargo(3, 30), new Cargo(4, 40) })
            };

            var repository = new InMemoryRepository<Warehouse>();
            var field = repository.GetType().GetField("_entities", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(repository, entities);

            // Act
            var result = repository.ReadAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveSameCount(entities);
            result.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public void ReadById_ReturnsEntityById()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Warehouse>(It.IsAny<Warehouse>()))
                      .Returns<Warehouse>(x => x);

            var entity1 = new Warehouse(new List<Cargo> { new Cargo(1, 10), new Cargo(2, 20) });
            var entity2 = new Warehouse(new List<Cargo> { new Cargo(3, 30), new Cargo(4, 40) });

            var repository = new InMemoryRepository<Warehouse>();
            repository.Create(entity1);
            repository.Create(entity2);

            // Act
            var result1 = repository.ReadById(entity1.Id);
            var result2 = repository.ReadById(entity2.Id);
            var result3 = repository.ReadById(-1);

            // Assert
            result1.Should().NotBeNull();
            result1.Should().BeEquivalentTo(entity1);

            result2.Should().NotBeNull();
            result2.Should().BeEquivalentTo(entity2);

            result3.Should().BeNull();
        }

        [Fact]
        public void Update_UpdatesExistingEntity()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Warehouse>(It.IsAny<Warehouse>()))
                      .Returns<Warehouse>(x => x);

            var entity1 = new Warehouse(new List<Cargo> { new Cargo(1, 10), new Cargo(2, 20) });
            var entity2 = new Warehouse(new List<Cargo> { new Cargo(1, 10), new Cargo(2, 20) });

            var repository = new InMemoryRepository<Warehouse>();
            repository.Create(entity1);

            // Act
            entity1.CargoList[0].Volume = 100;
            repository.Update(entity1.Id, entity1);

            var result = repository.ReadById(entity1.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(entity1);
            result.Should().NotBeEquivalentTo(entity2);
        }

        [Fact]
        public void Delete_DeletesExistingEntity()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Warehouse>(It.IsAny<Warehouse>()))
                      .Returns<Warehouse>(x => x);

            var entity1 = new Warehouse(new List<Cargo> { new Cargo(1, 10), new Cargo(2, 20) });
            var entity2 = new Warehouse(new List<Cargo> { new Cargo(3, 30), new Cargo(4, 40) });

            var repository = new InMemoryRepository<Warehouse>();
            repository.Create(entity1);
            repository.Create(entity2);

            // Act
            repository.Delete(entity1.Id);

            var result1 = repository.ReadById(entity1.Id);
            var result2 = repository.ReadById(entity2.Id);

            // Assert
            result1.Should().BeNull();
            result2.Should().NotBeNull();
            result2.Should().BeEquivalentTo(entity2);
        }
    }
}