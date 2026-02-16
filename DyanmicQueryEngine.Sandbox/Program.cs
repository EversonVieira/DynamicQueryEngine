
using DynamicQueryEngine.Core.DataSet;

var sqlBuilder = new SqlDataSetBuilder(new DynamicQueryEngine.Core.Config.SqlDataSetBuilderConfig());

sqlBuilder.WithMainTable("Test")
          .WithField("Test", "Id", SqlDataSetFieldType.Guid)
          .WithField("Test", "Name", SqlDataSetFieldType.String);
          
          

var ds = sqlBuilder.Build();


Console.WriteLine(ds.GetSqlQuery());
Console.WriteLine(ds.Id);
Console.ReadKey();
