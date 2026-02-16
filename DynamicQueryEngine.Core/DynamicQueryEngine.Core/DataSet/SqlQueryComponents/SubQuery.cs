namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    public class SubQuery
    {
        public string Alias { get; set; }
        public SqlDataSet DataSet { get; set; }

        public SubQuery(string alias, SqlDataSet dataSet)
        {
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentException("Alias cannot be null or empty.", nameof(alias));

            Alias = alias;
            DataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
        }
    }
}
