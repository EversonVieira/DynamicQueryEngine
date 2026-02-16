using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithGroupBySyntax
    {
        ISqlDataSetBuilder WithGroupBy(string tableName, string fieldName);
    }
}
