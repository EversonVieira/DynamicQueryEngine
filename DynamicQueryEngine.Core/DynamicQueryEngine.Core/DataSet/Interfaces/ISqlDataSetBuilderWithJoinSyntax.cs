using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithJoinSyntax
    {
        ISqlDataSetBuilder WithJoin(string sourceTable, string joinedTable, string sourceField, string joinedField, JoinType joinType = JoinType.Inner);
    }
}
