
using DynamicQueryEngine.Core.DataSet;
using DynamicQueryEngine.Core.DataSet.Adapters;
using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;
using DynamicQueryEngine.Core.DataSet.Validation;

var config = new DynamicQueryEngine.Core.Config.SqlDataSetBuilderConfig();

Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║   DynamicQueryEngine - DEMONSTRAÇÃO COMPLETA                   ║");
Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");

// ============================================
// 1. DISTINCT
// ============================================
Console.WriteLine("=== 1. DISTINCT ===\n");
var dsDistinct = new SqlDataSetBuilder(config)
    .WithDialect(new PostgresSqlDialectAdapter())
    .WithMainTable("Users")
    .WithField("Users", "Department", SqlDataSetFieldType.String)
    .WithDistinct()
    .Build();

Console.WriteLine(dsDistinct.GetSqlQuery());
Console.WriteLine();

// ============================================
// 2. AGGREGATE FUNCTIONS (SUM, COUNT, AVG, MIN, MAX)
// ============================================
Console.WriteLine("=== 2. AGGREGATE FUNCTIONS ===\n");
var dsAgg = new SqlDataSetBuilder(config)
    .WithDialect(new PostgresSqlDialectAdapter())
    .WithMainTable("Sales")
    .WithField("Sales", "UserId", SqlDataSetFieldType.Guid)
    .WithAggregateFunction("SUM", "Sales", "Amount", "Total")
    .WithAggregateFunction("COUNT", "Sales", "Id", "Count")
    .WithAggregateFunction("AVG", "Sales", "Amount", "Average")
    .WithGroupBy("Sales", "UserId")
    .Build();

Console.WriteLine(dsAgg.GetSqlQuery());
Console.WriteLine();

// ============================================
// 3. ADVANCED WHERE - BETWEEN
// ============================================
Console.WriteLine("=== 3. ADVANCED WHERE - BETWEEN ===\n");
var dsBetween = new SqlDataSetBuilder(config)
    .WithDialect(new PostgresSqlDialectAdapter())
    .WithMainTable("Orders")
    .WithField("Orders", "Id", SqlDataSetFieldType.Guid)
    .WithField("Orders", "Amount", SqlDataSetFieldType.Decimal)
    .WithAdvancedWhere("Orders", "Amount", WhereOperator.Between, 100, 5000)
    .Build();

Console.WriteLine(dsBetween.GetSqlQuery());
Console.WriteLine();

// ============================================
// 4. ADVANCED WHERE - LIKE (Padrão de texto)
// ============================================
Console.WriteLine("=== 4. ADVANCED WHERE - LIKE ===\n");
var dsLike = new SqlDataSetBuilder(config)
    .WithDialect(new PostgresSqlDialectAdapter())
    .WithMainTable("Customers")
    .WithField("Customers", "Id", SqlDataSetFieldType.Guid)
    .WithField("Customers", "Email", SqlDataSetFieldType.String)
    .WithAdvancedWhere("Customers", "Email", WhereOperator.Like, "gmail")
    .Build();

Console.WriteLine(dsLike.GetSqlQuery());
Console.WriteLine();

// ============================================
// 5. ADVANCED WHERE - IN (Múltiplos valores)
// ============================================
Console.WriteLine("=== 5. ADVANCED WHERE - IN ===\n");
var statuses = new[] { "Active", "Pending", "Completed" };
var dsIn = new SqlDataSetBuilder(config)
    .WithDialect(new PostgresSqlDialectAdapter())
    .WithMainTable("Tasks")
    .WithField("Tasks", "Id", SqlDataSetFieldType.Guid)
    .WithField("Tasks", "Status", SqlDataSetFieldType.String)
    .WithAdvancedWhere("Tasks", "Status", WhereOperator.In, statuses)
    .Build();

Console.WriteLine(dsIn.GetSqlQuery());
Console.WriteLine();

// ============================================
// 6. ADVANCED WHERE - IS NULL
// ============================================
Console.WriteLine("=== 6. ADVANCED WHERE - IS NULL ===\n");
var dsIsNull = new SqlDataSetBuilder(config)
    .WithDialect(new PostgresSqlDialectAdapter())
    .WithMainTable("Users")
    .WithField("Users", "Id", SqlDataSetFieldType.Guid)
    .WithField("Users", "Name", SqlDataSetFieldType.String)
    .WithAdvancedWhere("Users", "DeletedAt", WhereOperator.IsNull)
    .Build();

Console.WriteLine(dsIsNull.GetSqlQuery());
Console.WriteLine();

// ============================================
// 7. QUERY SUPER COMPLEXA COM TUDO
// ============================================
Console.WriteLine("=== 7. QUERY SUPER COMPLEXA ===\n");
var dsSuper = new SqlDataSetBuilder(config)
    .WithDialect(new PostgresSqlDialectAdapter())
    .WithMainTable("Orders")
    .WithField("Orders", "OrderId", SqlDataSetFieldType.Guid)
    .WithField("Orders", "Date", SqlDataSetFieldType.DateTime)
    .WithField("Customers", "Name", SqlDataSetFieldType.String)
    .WithField("Products", "ProductName", SqlDataSetFieldType.String)
    .WithField("Orders", "Amount", SqlDataSetFieldType.Decimal)
    .WithDistinct()
    .WithAggregateFunction("SUM", "Orders", "Amount", "TotalAmount")
    .WithAggregateFunction("COUNT", "Orders", "Id", "OrderCount")
    .WithJoin("Orders", "Customers", "CustomerId", "Id", JoinType.Inner)
    .WithJoin("Orders", "Products", "ProductId", "Id", JoinType.Left)
    .WithWhere("Orders", "OrderId", ">", 0)
    .WithAdvancedWhere("Orders", "Amount", WhereOperator.Between, 100, 10000)
    .WithAdvancedWhere("Orders", "Date", WhereOperator.IsNotNull)
    .WithGroupBy("Orders", "CustomerId")
    .WithGroupBy("Orders", "ProductId")
    .WithHaving("SUM", "Amount", ">", 1000)
    .WithOrderBy("Orders", "Amount", SortDirection.Descending)
    .WithOrderBy("Customers", "Name", SortDirection.Ascending)
    .WithLimitOffset(limit: 50, offset: 0)
    .Build();

Console.WriteLine(dsSuper.GetSqlQuery());
Console.WriteLine();

// ============================================
// 8. COMPLEXITY REPORT
// ============================================
Console.WriteLine("=== 8. COMPLEXITY REPORT ===\n");
Console.WriteLine(dsSuper.GetComplexityReport());
Console.WriteLine();

// Comparar complexidade com queries simples
var dsSimple = new SqlDataSetBuilder(config)
    .WithDialect(new PostgresSqlDialectAdapter())
    .WithMainTable("Users")
    .WithField("Users", "Id", SqlDataSetFieldType.Guid)
    .WithField("Users", "Name", SqlDataSetFieldType.String)
    .Build();

Console.WriteLine("COMPARAÇÃO DE COMPLEXIDADE:\n");
Console.WriteLine($"Query Simples: {dsSimple.ComplexityScore.TotalScore} points ({dsSimple.ComplexityScore.GetLevel()})");
Console.WriteLine($"Query Complexa: {dsSuper.ComplexityScore.TotalScore} points ({dsSuper.ComplexityScore.GetLevel()})");
Console.WriteLine();

// ============================================
// 9. EXEMPLOS REAIS - SQL SERVER vs PostgreSQL
// ============================================
Console.WriteLine("=== 9. MESMO DATASET, DIALETOS DIFERENTES ===\n");

var builder = new SqlDataSetBuilder(config)
    .WithMainTable("Products")
    .WithField("Products", "Id", SqlDataSetFieldType.Guid)
    .WithField("Products", "Name", SqlDataSetFieldType.String)
    .WithField("Products", "Price", SqlDataSetFieldType.Decimal)
    .WithAdvancedWhere("Products", "Price", WhereOperator.GreaterThan, 50)
    .WithOrderBy("Products", "Price", SortDirection.Descending)
    .WithLimitOffset(10);

var dsPostgres = builder.WithDialect(new PostgresSqlDialectAdapter()).Build();
var dsSqlServer = new SqlDataSetBuilder(config)
    .WithDialect(new SqlServerSqlDialectAdapter())
    .WithMainTable("Products")
    .WithField("Products", "Id", SqlDataSetFieldType.Guid)
    .WithField("Products", "Name", SqlDataSetFieldType.String)
    .WithField("Products", "Price", SqlDataSetFieldType.Decimal)
    .WithAdvancedWhere("Products", "Price", WhereOperator.GreaterThan, 50)
    .WithOrderBy("Products", "Price", SortDirection.Descending)
    .WithLimitOffset(10)
    .Build();

Console.WriteLine("PostgreSQL:");
Console.WriteLine(dsPostgres.GetSqlQuery());
Console.WriteLine("\nSQL Server:");
Console.WriteLine(dsSqlServer.GetSqlQuery());
Console.WriteLine();

Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║   DEMONSTRAÇÃO FINALIZADA COM SUCESSO!                        ║");
Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");

Console.ReadKey();
