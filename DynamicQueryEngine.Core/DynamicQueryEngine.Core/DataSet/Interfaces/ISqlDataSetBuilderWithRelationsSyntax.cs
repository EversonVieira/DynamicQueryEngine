namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithRelationsSyntax
    {
        ISqlDataSetBuilder WithRelation(string SourceTable, string JoinedTable, string SourceField, string JoinedField, SqlDataSetTableRelationType Type);
    }

    public interface ISqlDataSetBuilderWithMemoryCacheSyntax
    {
        ISqlDataSetBuilder AddLocalCache();
    }

    public interface ISqlDataSetBuilderWithExternalCache
    {
        ISqlDataSetBuilder WithExternalCache(ISqlDataSetCache externalCache);
    }

    public interface ISqlDataSetBuilderWithCacheExpiration
    {
        ISqlDataSetBuilder WithCacheExpiration(TimeSpan timeSpan);
    }
}
