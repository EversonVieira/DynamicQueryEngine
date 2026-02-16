using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithAggregateFunctionSyntax
    {
        ISqlDataSetBuilder WithAggregateFunction(string functionName, string tableName, string fieldName, string? alias = null);
    }
}
