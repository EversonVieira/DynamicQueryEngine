namespace DynamicQueryEngine.Core.DataSet
{
    public record SqlDataSetTableRelation(string SourceTable, string JoinedTable, string SourceField, string JoinedField, SqlDataSetTableRelationType Type);

    public enum SqlDataSetTableRelationType
    {
        InnerJoin,
        LeftJoin,
        RightJoin,
        FullJoin
    }
}
