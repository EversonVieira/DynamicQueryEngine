namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithExternalCache
    {
        ISqlDataSetBuilder WithExternalCache(ISqlDataSetCache externalCache);
    }


}
