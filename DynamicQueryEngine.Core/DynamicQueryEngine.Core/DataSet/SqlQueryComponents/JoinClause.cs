namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    public class JoinClause
    {
        public string SourceTable { get; set; }
        public string JoinedTable { get; set; }
        public string SourceField { get; set; }
        public string JoinedField { get; set; }
        public JoinType JoinType { get; set; }

        public JoinClause(string sourceTable, string joinedTable, string sourceField, string joinedField, JoinType joinType = JoinType.Inner)
        {
            SourceTable = sourceTable ?? throw new ArgumentNullException(nameof(sourceTable));
            JoinedTable = joinedTable ?? throw new ArgumentNullException(nameof(joinedTable));
            SourceField = sourceField ?? throw new ArgumentNullException(nameof(sourceField));
            JoinedField = joinedField ?? throw new ArgumentNullException(nameof(joinedField));
            JoinType = joinType;
        }
    }

    public enum JoinType
    {
        Inner,
        Left,
        Right,
        Full,
        Cross
    }
}
