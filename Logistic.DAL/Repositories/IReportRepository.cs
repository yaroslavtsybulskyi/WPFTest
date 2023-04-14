namespace Logistic.DAL
{
    public interface IReportRepository<T>
    {
        void Create(List<T> entities);
        List<T> Read(string fileName);
    }
}