using System.Text;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Logistic.DAL
{
    public class XmlRepository<T> : IReportRepository<T>
    {
        public void Create(List<T> entities)
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileName = $"{typeof(T).Name}_{timeStamp}.xml";

            using (FileStream fs = File.Create(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                serializer.Serialize(fs, entities);
            }
        }

        public List<T> Read(string fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                List<T> entities = (List<T>)serializer.Deserialize(fs);
                foreach (T item in entities)
                {
                    Console.WriteLine(item.ToString());
                }
                return entities;
            }
        }
    }
}