namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSet
    {
        public List<SqlDataSetField> Fields { get; init; }
        public string MainTable { get; init; }
        public List<SqlDataSetTableRelation> Relations { get; init; }
        public string Id { get; }
        public ISqlDataSetCache? Cache { get; init; }
        public TimeSpan? TimeSpan { get; init;  }
        public string GetSqlQuery();
    }
}