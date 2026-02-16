using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithOrderBySyntax
    {
        ISqlDataSetBuilder WithOrderBy(string tableName, string fieldName, SortDirection direction = SortDirection.Ascending);
    }
}
