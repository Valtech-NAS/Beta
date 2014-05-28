namespace SFA.Apprenticeships.Services.Elasticsearch.Entities.Elasticsearch
{
    using System.Collections.Generic;

    /// <summary>
    /// Do not change property names!
    /// </summary>
    public class Item<T> where T : new()
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public T _source { get; set; }
        public List<string> Sort { get; set; }
    }
}
