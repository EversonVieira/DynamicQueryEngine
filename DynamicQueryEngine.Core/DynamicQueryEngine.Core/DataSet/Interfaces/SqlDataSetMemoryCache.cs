namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public class SqlDataSetMemoryCache : ISqlDataSetCache
    {
        private record CacheItem(string Data, DateTimeOffset? Expiration);

        private Dictionary<string, CacheItem> _data = [];

        public string? Get(string key)
        {
            var result = _data.GetValueOrDefault(key);

            if ((result?.Expiration.HasValue ?? false) && result.Expiration.Value < DateTimeOffset.UtcNow)
            {
                _data.Remove(key);
                return null;
            }

            return result?.Data;
        }

        public void Set(string key, string value, TimeSpan? expiration)
        {
            _data[key] = new CacheItem(value, expiration is null ? null : DateTimeOffset.UtcNow.Add(expiration.Value));
        }
    }
}