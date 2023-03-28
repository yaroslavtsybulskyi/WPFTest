using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using Logistic.ConsoleClient.Enums;
using Logistic.ConsoleClient.Repositories;

namespace Logistic.ConsoleClient.Services
{
    public class ReportService<T>
    {
        private readonly JsonRepository<T> _jsonRepository;
        private readonly XmlRepository<T> _xmlRepository;

        public ReportService()
        {
            _jsonRepository = new JsonRepository<T>();
            _xmlRepository = new XmlRepository<T>();
        }

        public ReportService(JsonRepository<T> jsonRepository, XmlRepository<T> xmlRepository)
        {
            _jsonRepository = jsonRepository;
            _xmlRepository = xmlRepository;
        }

        public void CreateReport(List<T> entities, ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.Xml:
                    _xmlRepository.Create(entities);
                    break;
                case ReportType.Json:
                    _jsonRepository.Create(entities);
                    break;
                default:
                    throw new ArgumentException($"Unsupported report type: {reportType}");
            }
        }

        public List<T> LoadReport(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            switch (extension)
            {
                case ".xml":
                    XmlRepository<T> xmlRepository = new XmlRepository<T>();
                    return xmlRepository.Read(fileName);

                case ".json":
                    JsonRepository<T> jsonRepository = new JsonRepository<T>();
                    return jsonRepository.Read(fileName);

                default:
                    throw new ArgumentException($"Unknown file extension: {extension}");
            }
        }
    }
}
