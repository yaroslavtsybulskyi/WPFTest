namespace Logistic.ConsoleClient.Repositories
{
    public class JsonRepository<T>
    {
        public void Create(List<T> entities)
        {
            string entityName = typeof(T).Name;
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileName = $"{entityName}_{timeStamp}.json";

            using (FileStream fs = File.Create(fileName))
            {
                System.Text.Json.JsonSerializer.SerializeAsync(fs, entities).Wait();
            }
        }

        public List<T> Read(string fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                List<T> data = System.Text.Json.JsonSerializer.DeserializeAsync<List<T>>(fs).Result;
                foreach (T item in data)
                {
                    Console.WriteLine(item.ToString());
                }
                return data;
            }
        }
    }
}
