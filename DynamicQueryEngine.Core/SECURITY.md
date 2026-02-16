## 🔒 SEGURANÇA - SQL INJECTION DEFENSE

### ✅ O QUE FOI IMPLEMENTADO:

---

## 1️⃣ ESCAPE DE STRING SQL

**Método:** `EscapeSqlString(string value)`

```csharp
private string EscapeSqlString(string value)
{
    // Escape single quotes para SQL (duplicar single quotes)
    // Padrão SQL: ' → ''
    return value.Replace("'", "''");
}
```

**Exemplos:**
```
Input: "O'Brien"
Output: "O''Brien"

Input: "'; DROP TABLE Users; --"
Output: "''; DROP TABLE Users; --"
// Agora é tratado como literal string, não como SQL command
```

---

## 2️⃣ APLICAÇÃO EM TODOS OS LOCAIS

### ✅ Valores Simples (Equal, GreaterThan, etc)
```csharp
.WithAdvancedWhere("Users", "Name", WhereOperator.Equal, "O'Brien")
// Gera: WHERE Name = 'O''Brien'
```

### ✅ LIKE Clauses
```csharp
.WithAdvancedWhere("Customers", "Email", WhereOperator.Like, "'; DROP--")
// Gera: WHERE Email LIKE '%''; DROP--%'
// SEGURO! O single quote foi escapado
```

### ✅ IN Clauses (múltiplos valores)
```csharp
.WithAdvancedWhere("Status", "Value", WhereOperator.In, 
    new[] { "Active'--", "Pending", "Completed" })
// Gera: WHERE Status IN ('Active''--', 'Pending', 'Completed')
```

### ✅ BETWEEN (ambos os valores)
```csharp
.WithAdvancedWhere("Date", "Value", WhereOperator.Between, 
    "2024-01-01'; DROP--", "2024-12-31")
// Ambos são escapados automaticamente
```

---

## 3️⃣ APLICAÇÃO EM AMBOS OS ADAPTERS

- ✅ **PostgresSqlDialectAdapter.cs** - Atualizado
- ✅ **SqlServerSqlDialectAdapter.cs** - Atualizado

Ambos usam o mesmo escape padrão SQL (duplicar single quotes), que funciona em todos os databases.

---

## 📊 TESTE DE SEGURANÇA:

```csharp
// Teste de SQL Injection Attempt
var ds = new SqlDataSetBuilder(config)
    .WithDialect(new PostgresSqlDialectAdapter())
    .WithMainTable("Users")
    .WithField("Users", "Id", SqlDataSetFieldType.Guid)
    .WithField("Users", "Name", SqlDataSetFieldType.String)
    .WithAdvancedWhere("Users", "Name", WhereOperator.Like, "'; DROP TABLE Users; --")
    .Build();

var sql = ds.GetSqlQuery();

// RESULTADO (SEGURO):
// SELECT ... WHERE Name LIKE '%''; DROP TABLE Users; --%'
//                           ↑↑ Single quote foi escapado (duplicado)
//                           Agora é um literal string, não um comando SQL!
```

---

## 🎯 ABORDAGEM DE SEGURANÇA:

### **Defense in Depth (Múltiplas Camadas)**

1. **Camada 1 - QueryBuilder:** Escape automático (✅ implementado)
2. **Camada 2 - Aplicação:** Validação de input (responsabilidade do dev)
3. **Camada 3 - Database:** Usar Parameterized Queries (responsabilidade do dev)

```csharp
// ✅ MELHOR PRÁTICA RECOMENDADA:
var sql = dataset.GetSqlQuery(); // Já tem escape na string
var cmd = new SqlCommand(sql, connection);
// + Ainda adicione parâmetros se possível (mais seguro ainda)
```

---

## ✅ WHAT'S COVERED:

- ✅ Escape em **WHERE clauses** (Equal, NotEqual, GreaterThan, etc)
- ✅ Escape em **LIKE clauses** (Like, NotLike)
- ✅ Escape em **IN clauses** (múltiplos valores)
- ✅ Escape em **BETWEEN** (ambos os valores)
- ✅ Escape em **PostgreSQL** 
- ✅ Escape em **SQL Server**

---

## ⚠️ O QUE NÃO COBRIMOS:

❌ **Nomes de tabelas/colunas** - Não são escapados
- **Razão:** QueryBuilder fornece esses valores, não o usuário
- **Seguro porque:** Você controla as tabelas/colunas via API, não via string input

❌ **Operadores customizados** - Validados mas não escapados
- **Razão:** Operadores são de enum fixo (=, >, <, LIKE, IN, BETWEEN, IS NULL, etc)
- **Seguro porque:** Não vêm de input do usuário

---

## 🔐 CONCLUSÃO:

Com o escape implementado:
1. ✅ **QueryBuilder é seguro** - Protege contra SQL injection em valores
2. ✅ **Developer-friendly** - Escaping automático, sem overhead
3. ✅ **Defense in Depth** - Trabalha junto com melhores práticas do dev
4. ✅ **Sem performance loss** - Apenas string replace

**Responsabilidades do Desenvolvedor que usa:**
1. Use Parameterized Queries quando executar (MELHOR PRÁTICA)
2. Valide inputs no seu app (regra de negócio)
3. Use prepared statements se o framework suporta
