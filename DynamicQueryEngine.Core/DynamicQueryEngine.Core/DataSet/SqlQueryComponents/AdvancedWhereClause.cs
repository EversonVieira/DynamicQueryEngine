namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    public enum WhereOperator
    {
        Equal,           // =
        NotEqual,        // != ou <>
        GreaterThan,     // >
        GreaterOrEqual,  // >=
        LessThan,        // <
        LessOrEqual,     // <=
        Like,            // LIKE
        NotLike,         // NOT LIKE
        In,              // IN
        NotIn,           // NOT IN
        Between,         // BETWEEN
        IsNull,          // IS NULL
        IsNotNull        // IS NOT NULL
    }

    public class AdvancedWhereClause
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public WhereOperator Operator { get; set; }
        public object? Value { get; set; }
        public object? ValueEnd { get; set; } // Para BETWEEN

        public AdvancedWhereClause(string tableName, string fieldName, WhereOperator op, object? value = null, object? valueEnd = null)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Operator = op;
            Value = value;
            ValueEnd = valueEnd;

            // Validação
            if (op == WhereOperator.Between && (value == null || valueEnd == null))
                throw new ArgumentException("BETWEEN requires both Value and ValueEnd.", nameof(valueEnd));

            if ((op == WhereOperator.IsNull || op == WhereOperator.IsNotNull) && value != null)
                throw new ArgumentException($"{op} does not require a value.", nameof(value));
        }
    }
}
