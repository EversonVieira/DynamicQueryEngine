namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    public class WhereClause
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }

        public WhereClause(string tableName, string fieldName, string op, object value)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Operator = op ?? throw new ArgumentNullException(nameof(op));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
