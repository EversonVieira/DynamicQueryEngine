namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithMainTableSyntax
    {
        ISqlDataSetBuilder WithMainTable(string mainTable);
    }
}
