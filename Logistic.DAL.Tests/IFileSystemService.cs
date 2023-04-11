namespace Logistic.DAL.Tests
{
    public interface IFileSystemService
    {
        bool FileExists(string path);
        string ReadFile(string path);
    }
}