using DynamicQueryEngine.Core.DataSet.Validation;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilderWithValidationSyntax
    {
        ISqlDataSetBuilder WithValidator(ISqlDataSetValidator validator);
    }
}
