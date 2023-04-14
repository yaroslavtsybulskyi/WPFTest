using System.Xml.Serialization;
using FluentAssertions;
using Logistic.DAL;
using Logistic.Models;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Logistic.Core.Services.Tests
{
    public class ReportServiceTests
    {
        [Fact]
        public void CreateReport_WithValidEntitiesAndReportType_CallsCorrectRepositoryMethod()
        {
            // Arrange
            var jsonRepositoryMock = new Mock<IReportRepository<Warehouse>>();
            var xmlRepositoryMock = new Mock<IReportRepository<Warehouse>>();
            var reportService = new ReportService<Warehouse>(jsonRepositoryMock.Object, xmlRepositoryMock.Object);

            var warehouse1 = new Warehouse(new List<Cargo>() { new Cargo(5, 100), new Cargo(10, 200) });
            var warehouse2 = new Warehouse(new List<Cargo>() { new Cargo(20, 300), new Cargo(30, 400) });

            var warehouses = new List<Warehouse>() { warehouse1, warehouse2 };

            // Act
            reportService.CreateReport(warehouses, ReportType.Xml);
            reportService.CreateReport(warehouses, ReportType.Json);

            // Assert
            xmlRepositoryMock.Verify(x => x.Create(warehouses), Times.Once);
            jsonRepositoryMock.Verify(x => x.Create(warehouses), Times.Once);
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

            var testDataDirectory = "TestData";
            var xmlFilePath = Path.Combine(testDataDirectory, "test_report.xml");

            var serializer = new XmlSerializer(typeof(List<Warehouse>));
            using (var writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, expectedData);
            }

            var xmlRepository = new XmlRepository<Warehouse>();
            var reportService = new ReportService<Warehouse>(null, xmlRepository);

            // Act
            var result = reportService.LoadReport(xmlFilePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedData.Count, result.Count);

            Assert.Equal(expectedData[0].CargoList.Count, result[0].CargoList.Count);
            Assert.Equal(expectedData[0].CargoList[0].Weight, result[0].CargoList[0].Weight);
            Assert.Equal(expectedData[0].CargoList[0].Volume, result[0].CargoList[0].Volume);
            Assert.Equal(expectedData[0].CargoList[1].Weight, result[0].CargoList[1].Weight);
            Assert.Equal(expectedData[0].CargoList[1].Volume, result[0].CargoList[1].Volume);

            Assert.Equal(expectedData[1].CargoList.Count, result[1].CargoList.Count);
            Assert.Equal(expectedData[1].CargoList[0].Weight, result[1].CargoList[0].Weight);
            Assert.Equal(expectedData[1].CargoList[0].Volume, result[1].CargoList[0].Volume);
            Assert.Equal(expectedData[1].CargoList[1].Weight, result[1].CargoList[1].Weight);
            Assert.Equal(expectedData[1].CargoList[1].Volume, result[1].CargoList[1].Volume);
        }

        [Fact]
        public void LoadReport_LoadsJsonDataFromFile()
        {
            // Arrange
            var expectedData = new List<Warehouse>()
            {
                new Warehouse(new List<Cargo>() { new Cargo(1, 10), new Cargo(2, 20) }),
                new Warehouse(new List<Cargo>() { new Cargo(3, 30), new Cargo(4, 40) })
            };

            var testDataDirectory = "TestData";
            var jsonFilePath = Path.Combine(testDataDirectory, "test_report.json");

            // Serialize the expected data to a JSON file
            File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(expectedData));

            var jsonRepositoryMock = new Mock<IReportRepository<Warehouse>>();
            jsonRepositoryMock.Setup(x => x.Read(jsonFilePath)).Returns(expectedData);
            var reportService = new ReportService<Warehouse>(jsonRepositoryMock.Object, null);

            // Act
            var result = reportService.LoadReport(jsonFilePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedData.Count, result.Count);
            Assert.Equal(expectedData[0].CargoList[0].Weight, result[0].CargoList[0].Weight);
            Assert.Equal(expectedData[0].CargoList[0].Volume, result[0].CargoList[0].Volume);
            Assert.Equal(expectedData[0].CargoList[1].Weight, result[0].CargoList[1].Weight);
            Assert.Equal(expectedData[0].CargoList[1].Volume, result[0].CargoList[1].Volume);
            Assert.Equal(expectedData[1].CargoList[0].Weight, result[1].CargoList[0].Weight);
            Assert.Equal(expectedData[1].CargoList[0].Volume, result[1].CargoList[0].Volume);
            Assert.Equal(expectedData[1].CargoList[1].Weight, result[1].CargoList[1].Weight);
            Assert.Equal(expectedData[1].CargoList[1].Volume, result[1].CargoList[1].Volume);
        }

        [Fact]
        public void LoadReport_WithUnknownExtension_ThrowsArgumentException()
        {
            // Arrange
            var fileName = "test_warehouse.txt";
            var jsonRepositoryMock = new Mock<IReportRepository<Warehouse>>();
            var xmlRepositoryMock = new Mock<IReportRepository<Warehouse>>();
            var reportService = new ReportService<Warehouse>(jsonRepositoryMock.Object, xmlRepositoryMock.Object);

            // Act
            Action act = () => reportService.LoadReport(fileName);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage($"Unknown file extension: {Path.GetExtension(fileName)}");
        }
    }
}
