using Logistic.Models;
using Xunit;

namespace Logistic.DAL.Tests
{
    public class XmlRepositoryTests
    {
        [Fact]
        public void Create_WithValidEntities_CallsCorrectRepositoryMethod()
        {
            // Arrange
            var warehouses = new List<Warehouse>()
            {
                new Warehouse(new List<Cargo>() { new Cargo(5, 100), new Cargo(10, 200) }),
                new Warehouse(new List<Cargo>() { new Cargo(20, 300), new Cargo(30, 400) })
            };

            string expectedFileName = $"Warehouse_{DateTime.Now:yyyyMMddHHmmss}.xml";
            var repository = new XmlRepository<Warehouse>();

            // Act
            repository.Create(warehouses);

            // Assert
            Assert.True(File.Exists(expectedFileName));
        }

        [Fact]
        public void LoadReport_LoadsXmlDataFromFile()
        {
            // Arrange
            var xmlRepository = new XmlRepository<Warehouse>();
            var filePath = Path.Combine("Resources", "test.xml");

            // Act
            var result = xmlRepository.Read(filePath);


            // Assert
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public void LoadReport_WithUnsupportedExtension_ThrowsException()
        {
            // Arrange
            var repository = new XmlRepository<Warehouse>();
            var filePath = Path.Combine("Resources", "test.txt");

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => repository.Read(filePath));

            //Assert
            Assert.Contains("There is an error in XML document", exception.Message);
        }
    }
}