using DynamicQueryEngine.Core.DataSet.Adapters;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithDialectSyntax
    {
        ISqlDataSetBuilder WithDialect(ISqlDialectAdapter dialectAdapter);
    }
}
