namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithFieldsSyntax
    {
        ISqlDataSetBuilder WithField(string tableName, string fieldName, SqlDataSetFieldType type);
    }
}
