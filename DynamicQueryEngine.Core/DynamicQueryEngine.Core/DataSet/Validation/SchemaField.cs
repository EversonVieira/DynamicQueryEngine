namespace DynamicQueryEngine.Core.DataSet.Validation
{
    public class SchemaField
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public SqlDataSetFieldType FieldType { get; set; }
        public bool IsNullable { get; set; }

        public SchemaField(string tableName, string fieldName, SqlDataSetFieldType fieldType, bool isNullable = true)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            FieldType = fieldType;
            IsNullable = isNullable;
        }
    }
}
