namespace DynamicQueryEngine.Core.DataSet.Validation
{
    public interface ISqlDataSetValidator
    {
        bool ValidateField(string tableName, string fieldName, SqlDataSetFieldType expectedType);
        bool ValidateTable(string tableName);
        bool ValidateJoin(string sourceTable, string sourceField, string joinedTable, string joinedField);
    }

    public class DefaultSqlDataSetValidator : ISqlDataSetValidator
    {
        private readonly Dictionary<string, List<SchemaField>> _schema;

        public DefaultSqlDataSetValidator(Dictionary<string, List<SchemaField>> schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public bool ValidateField(string tableName, string fieldName, SqlDataSetFieldType expectedType)
        {
            if (!_schema.ContainsKey(tableName))
                throw new InvalidOperationException($"Table '{tableName}' not found in schema.");

            var field = _schema[tableName].FirstOrDefault(f => 
                f.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase));

            if (field == null)
                throw new InvalidOperationException($"Field '{fieldName}' not found in table '{tableName}'.");

            if (field.FieldType != expectedType)
                throw new InvalidOperationException($"Field '{fieldName}' in table '{tableName}' is of type '{field.FieldType}', not '{expectedType}'.");

            return true;
        }

        public bool ValidateTable(string tableName)
        {
            if (!_schema.ContainsKey(tableName))
                throw new InvalidOperationException($"Table '{tableName}' not found in schema.");

            return true;
        }

        public bool ValidateJoin(string sourceTable, string sourceField, string joinedTable, string joinedField)
        {
            ValidateField(sourceTable, sourceField, SqlDataSetFieldType.Guid);
            ValidateField(joinedTable, joinedField, SqlDataSetFieldType.Guid);
            return true;
        }
    }
}
