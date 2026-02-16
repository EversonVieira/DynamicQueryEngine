namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithCacheExpiration
    {
        ISqlDataSetBuilder WithCacheExpiration(TimeSpan timeSpan);
    }


}
