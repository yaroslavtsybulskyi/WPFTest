using Logistic.Models;

namespace Logistic.Core.Services
{
    public interface IReportService<T>
    {
        void CreateReport(List<T> entities, ReportType reportType);
        List<T> LoadReport(string fileName);
    }
}
