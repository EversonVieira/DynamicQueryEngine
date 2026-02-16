using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithWhereSyntax
    {
        ISqlDataSetBuilder WithWhere(string tableName, string fieldName, string op, object value);
    }
}
