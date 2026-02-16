using DynamicQueryEngine.Core.DataSet.Interfaces;

namespace DynamicQueryEngine.Core.DataSet
{
    public class QueryableSqlDataSet 
    {
        public ISqlDataSet DataSet { get; init; }

        public QueryableSqlDataSet(ISqlDataSet dataSet)
        {
            DataSet = dataSet;
        }


    }
}
