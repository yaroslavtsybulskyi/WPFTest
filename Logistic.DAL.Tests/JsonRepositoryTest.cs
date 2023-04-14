using Logistic.Models;
using Newtonsoft.Json;
using Xunit;

namespace Logistic.DAL.Tests
{
    public class JsonRepositoryTests
    {
        [Fact]
        public void Create_WithValidEntities_CallsCorrectRepositoryMethod_Json()
        {
            // Arrange
            var warehouses = new List<Warehouse>()
            {
                new Warehouse(new List<Cargo>() { new Cargo(5, 100), new Cargo(10, 200) }),
                new Warehouse(new List<Cargo>() { new Cargo(20, 300), new Cargo(30, 400) })
            };

            string expectedFileName = $"Warehouse_{DateTime.Now:yyyyMMddHHmmss}.json";
            var jsonRepository = new JsonRepository<Warehouse>();

            // Act
            jsonRepository.Create(warehouses);

            // Assert
            Assert.True(File.Exists(expectedFileName));
        }

        [Fact]
        public void LoadReport_LoadsJsonDataFromFile()
        {
            // Arrange
            var jsonRepository = new JsonRepository<Warehouse>();
            var filePath = Path.Combine("Resources", "test.json");

            // Act
            var result = jsonRepository.Read(filePath);

            // Assert
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public void LoadReport_WithUnsupportedExtension_ThrowsException()
        {
            // Arrange
            var repository = new JsonRepository<Warehouse>();
            var filePath = Path.Combine("Resources", "test.txt");

            // Act
            var exception = Assert.Throws<JsonReaderException>(() => repository.Read(filePath));

            //Assert
            Assert.Contains("Unexpected character encountered while parsing value", exception.Message);
        }
    }
}