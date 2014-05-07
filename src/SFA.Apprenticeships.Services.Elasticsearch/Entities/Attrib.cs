
namespace SFA.Apprenticeships.Repository.Elasticsearch.Entities
{
    public class Attrib
    {
        public Attrib() { }

        public Attrib(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public int Value { get; set; }
    }
}
