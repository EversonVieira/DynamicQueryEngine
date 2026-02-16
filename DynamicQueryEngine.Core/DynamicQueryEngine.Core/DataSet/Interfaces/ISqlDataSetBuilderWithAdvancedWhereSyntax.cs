using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithAdvancedWhereSyntax
    {
        ISqlDataSetBuilder WithAdvancedWhere(string tableName, string fieldName, WhereOperator op, object? value = null, object? valueEnd = null);
    }
}
