using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSetBuilder : ISqlDataSetBuilderWithMainTableSyntax,
                                          ISqlDataSetBuilderWithFieldsSyntax, 
                                          ISqlDataSetBuilderWithRelationsSyntax,
                                          ISqlDataSetBuilderWithMemoryCacheSyntax,
                                          ISqlDataSetBuilderWithExternalCache,
                                          ISqlDataSetBuilderWithCacheExpiration,
                                          ISqlDataSetBuilderWithDialectSyntax,
                                          ISqlDataSetBuilderWithWhereSyntax,
                                          ISqlDataSetBuilderWithAdvancedWhereSyntax,
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
