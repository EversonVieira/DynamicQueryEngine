namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithMaterializedView
    {
        ISqlDataSetBuilder WithMaterializedView(string viewName, TimeSpan? timeSpan);
    }


}
