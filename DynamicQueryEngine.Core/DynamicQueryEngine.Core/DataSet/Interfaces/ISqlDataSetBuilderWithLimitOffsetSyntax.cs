using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithLimitOffsetSyntax
    {
        ISqlDataSetBuilder WithLimitOffset(int? limit, int? offset = null);
    }
}
