using Newtonsoft.Json;

namespace Logistic.DAL
{
    public class JsonRepository<T> : IReportRepository<T>
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
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                List<T> data = JsonConvert.DeserializeObject<List<T>>(json);
                foreach (T item in data)
                {
                    Console.WriteLine(item.ToString());
                }
                return data;
            }
        }
    }
}