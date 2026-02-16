namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    public class AggregateFunction
    {
        public string FunctionName { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string? Alias { get; set; }

        public AggregateFunction(string functionName, string tableName, string fieldName, string? alias = null)
        {
            FunctionName = functionName?.ToUpper() ?? throw new ArgumentNullException(nameof(functionName));
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Alias = alias;

            // Validar funções suportadas
            var validFunctions = new[] { "SUM", "COUNT", "AVG", "MIN", "MAX" };
            if (!validFunctions.Contains(FunctionName))
                throw new ArgumentException($"Function '{FunctionName}' not supported. Use: {string.Join(", ", validFunctions)}", nameof(functionName));
        }
    }
}
