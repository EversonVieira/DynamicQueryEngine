namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    /// <summary>
    /// Operadores suportados em subqueries
    /// </summary>
    public enum SubQueryOperator
    {
        /// <summary>WHERE field IN (SELECT ...)</summary>
        In,
        /// <summary>WHERE field NOT IN (SELECT ...)</summary>
        NotIn,
        /// <summary>WHERE EXISTS (SELECT ...)</summary>
        Exists,
        /// <summary>WHERE NOT EXISTS (SELECT ...)</summary>
        NotExists,
        /// <summary>WHERE field > (SELECT ...)</summary>
        GreaterThan,
        /// <summary>WHERE field >= (SELECT ...)</summary>
        GreaterOrEqual,
        /// <summary>WHERE field < (SELECT ...)</summary>
        LessThan,
        /// <summary>WHERE field <= (SELECT ...)</summary>
        LessOrEqual,
        /// <summary>WHERE field = (SELECT ...)</summary>
        Equal,
        /// <summary>WHERE field != (SELECT ...)</summary>
        NotEqual
    }

    /// <summary>
    /// Representa uma cláusula WHERE com subquery
    /// </summary>
    public class SubQueryWhereClause
    {
        /// <summary>
        /// Nome da tabela (null para EXISTS/NOT EXISTS)
        /// </summary>
        public string? TableName { get; set; }

        /// <summary>
        /// Nome do campo (null para EXISTS/NOT EXISTS)
        /// </summary>
        public string? FieldName { get; set; }

        /// <summary>
        /// Operador da subquery
        /// </summary>
        public SubQueryOperator Operator { get; set; }

        /// <summary>
        /// DataSet contendo a subquery
        /// </summary>
        public SqlDataSet SubQuery { get; set; }

        /// <summary>
        /// Cria uma nova instância de SubQueryWhereClause
        /// </summary>
        public SubQueryWhereClause(string? tableName, string? fieldName, SubQueryOperator op, SqlDataSet subQuery)
        {
            if (subQuery == null)
                throw new ArgumentNullException(nameof(subQuery));

            var isExistsOp = op == SubQueryOperator.Exists || op == SubQueryOperator.NotExists;

            if (!isExistsOp && string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"{op} requires TableName.", nameof(tableName));

            if (!isExistsOp && string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentException($"{op} requires FieldName.", nameof(fieldName));

            if (isExistsOp && (!string.IsNullOrWhiteSpace(tableName) || !string.IsNullOrWhiteSpace(fieldName)))
                throw new ArgumentException($"{op} does not require TableName or FieldName.", nameof(tableName));

            TableName = tableName;
            FieldName = fieldName;
            Operator = op;
            SubQuery = subQuery;
        }
    }
}
