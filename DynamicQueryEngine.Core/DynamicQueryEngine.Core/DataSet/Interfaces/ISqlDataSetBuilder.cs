using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilder : ISqlDataSetBuilderWithMainTableSyntax,
                                          ISqlDataSetBuilderWithFieldsSyntax, 
                                          ISqlDataSetBuilderWithMemoryCacheSyntax,
                                          ISqlDataSetBuilderWithExternalCache,
                                          ISqlDataSetBuilderWithCacheExpiration,
                                          ISqlDataSetBuilderWithDialectSyntax,
                                          ISqlDataSetBuilderWithWhereSyntax,
                                          ISqlDataSetBuilderWithAdvancedWhereSyntax,
                                          ISqlDataSetBuilderWithSubQueryWhereSyntax,
                                          ISqlDataSetBuilderWithGroupBySyntax,
                                          ISqlDataSetBuilderWithHavingSyntax,
                                          ISqlDataSetBuilderWithOrderBySyntax,
                                          ISqlDataSetBuilderWithLimitOffsetSyntax,
                                          ISqlDataSetBuilderWithJoinSyntax,
                                          ISqlDataSetBuilderWithSubQuerySyntax,
                                          ISqlDataSetBuilderWithValidationSyntax,
                                          ISqlDataSetBuilderWithJsonExportSyntax,
                                          ISqlDataSetBuilderWithDistinctSyntax,
                                          ISqlDataSetBuilderWithAggregateFunctionSyntax
    {
        ISqlDataSet Build();
    }
}
