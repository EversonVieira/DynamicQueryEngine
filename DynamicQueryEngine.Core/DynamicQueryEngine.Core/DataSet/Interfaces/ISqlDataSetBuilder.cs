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
                                          ISqlDataSetBuilderWithCacheExpiration
    {
        ISqlDataSet Build();
    }
}
