namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    public class GroupByClause
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }

        public GroupByClause(string tableName, string fieldName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
        }
    }
}
