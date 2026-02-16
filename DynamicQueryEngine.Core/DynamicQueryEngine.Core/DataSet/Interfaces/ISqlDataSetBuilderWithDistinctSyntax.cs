using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithDistinctSyntax
    {
        ISqlDataSetBuilder WithDistinct();
    }
}
