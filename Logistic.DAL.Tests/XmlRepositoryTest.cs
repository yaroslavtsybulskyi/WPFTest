using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FluentAssertions;
using Logistic.Core.Services;
using Logistic.DAL;
using Logistic.Models;
using Moq;
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

            var xmlRepositoryMock = new Mock<XmlRepository<Warehouse>>();
            var jsonRepositoryMock = new Mock<JsonRepository<Warehouse>>();
            var reportService = new ReportService<Warehouse>(jsonRepositoryMock.Object, xmlRepositoryMock.Object);

            // Act
            reportService.CreateReport(warehouses, ReportType.Xml);

            // Assert
            xmlRepositoryMock.Verify(x => x.Create(warehouses), Times.Once);
        }

        [Fact]
        public void LoadReport_LoadsXmlDataFromFile()
        {
            // Arrange
            var expectedData = new List<Warehouse>()
            {
                new Warehouse(new List<Cargo>() { new Cargo(1, 10), new Cargo(2, 20) }),
                new Warehouse(new List<Cargo>() { new Cargo(3, 30), new Cargo(4, 40) })
            };

            var fileName = "test_report.xml";

            var serializer = new XmlSerializer(typeof(List<Warehouse>));
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, expectedData);
            }

            var xmlRepository = new XmlRepository<Warehouse>();
            var mockFileSystem = new Mock<IFileSystemService>();
            mockFileSystem.Setup(x => x.FileExists(fileName)).Returns(true);
            mockFileSystem.Setup(x => x.ReadFile(fileName)).Returns(File.ReadAllText(fileName));

            var reportService = new ReportService<Warehouse>(null, xmlRepository);

            // Act
            var result = reportService.LoadReport(fileName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedData.Count, result.Count);

            for (int i = 0; i < expectedData.Count; i++)
            {
                Assert.NotNull(result[i]);
                Assert.Equal(expectedData[i].Id, result[i].Id);
                Assert.Equal(expectedData[i].CargoList.Count, result[i].CargoList.Count);

                for (int j = 0; j < expectedData[i].CargoList.Count; j++)
                {
                    Assert.NotNull(result[i].CargoList[j]);
                    Assert.Equal(expectedData[i].CargoList[j].Id, result[i].CargoList[j].Id);
                }
            }
        }

        [Fact]
        public void LoadReport_WithUnsupportedExtension_ThrowsArgumentException()
        {
            // Arrange
            var invalidFilePath = "test_report.txt";
            var reportService = new ReportService<Warehouse>();

            // Act & Assert
            reportService
                 .Invoking(x => x.LoadReport(invalidFilePath))
                 .Should().Throw<ArgumentException>()
                 .WithMessage($"Unknown file extension: {Path.GetExtension(invalidFilePath)}");
        }
    }
}