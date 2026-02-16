namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class OrderByClause
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public SortDirection Direction { get; set; }

        public OrderByClause(string tableName, string fieldName, SortDirection direction = SortDirection.Ascending)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Direction = direction;
        }
    }
}
