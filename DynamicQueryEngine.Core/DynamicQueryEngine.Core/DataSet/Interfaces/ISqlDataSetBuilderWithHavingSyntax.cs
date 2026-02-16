using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithHavingSyntax
    {
        ISqlDataSetBuilder WithHaving(string aggregateFunction, string fieldName, string op, object value);
    }
}
