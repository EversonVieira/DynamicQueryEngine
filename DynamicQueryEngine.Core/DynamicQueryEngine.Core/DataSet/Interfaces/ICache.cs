namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetCache
    {
        public string? Get(string key);
        public void Set(string key, string value, TimeSpan? expiration);
    }
}