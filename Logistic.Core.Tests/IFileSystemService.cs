namespace Logistic.Core.Services.Tests
{
    public interface IFileSystemService
    {
        bool FileExists(string path);
        string ReadFile(string path);
    }
}