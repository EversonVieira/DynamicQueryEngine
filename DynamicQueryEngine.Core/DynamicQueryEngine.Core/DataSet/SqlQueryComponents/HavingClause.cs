namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    public class HavingClause
    {
        public string AggregateFunction { get; set; }
        public string FieldName { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }

        public HavingClause(string aggregateFunction, string fieldName, string op, object value)
        {
            AggregateFunction = aggregateFunction ?? throw new ArgumentNullException(nameof(aggregateFunction));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Operator = op ?? throw new ArgumentNullException(nameof(op));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
