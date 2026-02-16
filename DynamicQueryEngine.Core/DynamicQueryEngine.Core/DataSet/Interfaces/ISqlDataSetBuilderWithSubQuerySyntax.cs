namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithSubQuerySyntax
    {
        ISqlDataSetBuilder WithSubQuery(string alias, SqlDataSet subQuery);
    }
}
