namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithView
    {
        ISqlDataSetBuilder WithView(string viewName);
    }


}
