# To reproduce:

Have a SQL Server instance running for the following connection string (or change lines 26 to 31 in `Startup.cs` to use something else like an in-memory one, but I'm not sure if it'll reproduce on an in-memory database):
`Server=localhost;Database=reproduction;Trusted_Connection=True;`

Install dotnet-ef tools, if not done already:
`dotnet tool install --global dotnet-ef`

Apply the migration (which also seeds the data):
`dotnet ef database update --startup-project Reproduction --project Reproduction` 

Launch the project, and enter the following query into `BCP`:
```
{
    backgrounds {
      items {
        effects {
          effect {
            ... on MyTraitEffect {
              trait {
                name
              }
            }
          }
        }
      }
    }
}
```

The following execution error will occur:

```
{
  "errors": [
    {
      "message": "Unexpected Execution Error",
      "locations": [
        {
          "line": 2,
          "column": 3
        }
      ],
      "path": [
        "backgrounds"
      ],
      "extensions": {
        "message": "The client projection contains a reference to a constant expression of 'Reproduction.MyAbilityEffect'. This could potentially cause a memory leak; consider assigning this constant to a local variable and using the variable in the query instead. See https://go.microsoft.com/fwlink/?linkid=2103067 for more information.",
        "stackTrace": "   at Microsoft.EntityFrameworkCore.Query.ShapedQueryCompilingExpressionVisitor.ConstantVerifyingExpressionVisitor.VisitConstant(ConstantExpression constantExpression)\r\n   at System.Linq.Expressions.ConstantExpression.Accept(ExpressionVisitor visitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit(Expression node)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitUnary(UnaryExpression node)\r\n   at System.Linq.Expressions.UnaryExpression.Accept(ExpressionVisitor visitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit(Expression node)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitConditional(ConditionalExpression node)\r\n   at System.Linq.Expressions.ConditionalExpression.Accept(ExpressionVisitor visitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit(Expression node)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitConditional(ConditionalExpression node)\r\n   at System.Linq.Expressions.ConditionalExpression.Accept(ExpressionVisitor visitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit(Expression node)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitMemberAssignment(MemberAssignment node)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitMemberBinding(MemberBinding node)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit[T](ReadOnlyCollection`1 nodes, Func`2 elementVisitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitMemberInit(MemberInitExpression node)\r\n   at System.Linq.Expressions.MemberInitExpression.Accept(ExpressionVisitor visitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit(Expression node)\r\n   at Microsoft.EntityFrameworkCore.Query.RelationalCollectionShaperExpression.VisitChildren(ExpressionVisitor visitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitExtension(Expression node)\r\n   at Microsoft.EntityFrameworkCore.Query.ShapedQueryCompilingExpressionVisitor.ConstantVerifyingExpressionVisitor.VisitExtension(Expression extensionExpression)\r\n   at System.Linq.Expressions.Expression.Accept(ExpressionVisitor visitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit(Expression node)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitMemberAssignment(MemberAssignment node)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitMemberBinding(MemberBinding node)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit[T](ReadOnlyCollection`1 nodes, Func`2 elementVisitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.VisitMemberInit(MemberInitExpression node)\r\n   at System.Linq.Expressions.MemberInitExpression.Accept(ExpressionVisitor visitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit(Expression node)\r\n   at Microsoft.EntityFrameworkCore.Query.ShapedQueryCompilingExpressionVisitor.VerifyNoClientConstant(Expression expression)\r\n   at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.VisitShapedQuery(ShapedQueryExpression shapedQueryExpression)\r\n   at Microsoft.EntityFrameworkCore.Query.ShapedQueryCompilingExpressionVisitor.VisitExtension(Expression extensionExpression)\r\n   at System.Linq.Expressions.Expression.Accept(ExpressionVisitor visitor)\r\n   at System.Linq.Expressions.ExpressionVisitor.Visit(Expression node)\r\n   at Microsoft.EntityFrameworkCore.Query.QueryCompilationContext.CreateQueryExecutor[TResult](Expression query)\r\n   at Microsoft.EntityFrameworkCore.Storage.Database.CompileQuery[TResult](Expression query, Boolean async)\r\n   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.CompileQueryCore[TResult](IDatabase database, Expression query, IModel model, Boolean async)\r\n   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.<>c__DisplayClass12_0`1.<ExecuteAsync>b__0()\r\n   at Microsoft.EntityFrameworkCore.Query.Internal.CompiledQueryCache.GetOrAddQuery[TResult](Object cacheKey, Func`1 compiler)\r\n   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1.GetAsyncEnumerator(CancellationToken cancellationToken)\r\n   at System.Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable`1.GetAsyncEnumerator()\r\n   at HotChocolate.Types.Pagination.QueryableOffsetPagination`1.ExecuteAsync(IQueryable`1 query, CancellationToken cancellationToken)\r\n   at HotChocolate.Types.Pagination.OffsetPaginationAlgorithm`2.ApplyPaginationAsync(TQuery query, OffsetPagingArguments arguments, Nullable`1 totalCount, CancellationToken cancellationToken)\r\n   at HotChocolate.Types.Pagination.QueryableOffsetPagingHandler`1.ResolveAsync(IResolverContext context, IQueryable`1 source, OffsetPagingArguments arguments, CancellationToken cancellationToken)\r\n   at HotChocolate.Types.Pagination.OffsetPagingHandler.HotChocolate.Types.Pagination.IPagingHandler.SliceAsync(IResolverContext context, Object source)\r\n   at HotChocolate.Types.Pagination.PagingMiddleware.InvokeAsync(IMiddlewareContext context)\r\n   at HotChocolate.Utilities.MiddlewareCompiler`1.ExpressionHelper.AwaitTaskHelper(Task task)\r\n   at HotChocolate.Data.ToListMiddleware`1.InvokeAsync(IMiddlewareContext context)\r\n   at HotChocolate.Types.EntityFrameworkObjectFieldDescriptorExtensions.<>c__DisplayClass2_1`1.<<UseDbContext>b__4>d.MoveNext()\r\n--- End of stack trace from previous location ---\r\n   at HotChocolate.Types.EntityFrameworkObjectFieldDescriptorExtensions.<>c__DisplayClass2_1`1.<<UseDbContext>b__4>d.MoveNext()\r\n--- End of stack trace from previous location ---\r\n   at HotChocolate.Execution.Processing.Tasks.ResolverTask.ExecuteResolverPipelineAsync(CancellationToken cancellationToken)\r\n   at HotChocolate.Execution.Processing.Tasks.ResolverTask.TryExecuteAsync(CancellationToken cancellationToken)"
      }
    }
  ],
  "data": {
    "backgrounds": null
  }
}
```

However, if you include any field, such as `id`, in the effect interface, things will work as expected, i.e. the following yields the right response:

```
{
    backgrounds {
      items {
        effects {
          effect {
            id,
            ... on MyTraitEffect {
              trait {
                name
              }
            }
          }
        }
      }
    }
}
```