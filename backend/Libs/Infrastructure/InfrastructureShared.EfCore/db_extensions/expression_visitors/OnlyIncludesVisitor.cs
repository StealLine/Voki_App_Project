using System.Linq.Expressions;
using SharedKernel.exceptions;

namespace InfrastructureShared.EfCore.db_extensions.expression_visitors;

public sealed class OnlyIncludesVisitor : ExpressionVisitor
{
    private bool HasInclude { get; set; }
    private bool HasForbiddenCalls { get; set; }
    private string? FirstForbiddenMethodName { get; set; }

    protected override Expression VisitMethodCall(MethodCallExpression node) {
        if (HasForbiddenCalls)
            return node;

        var name = node.Method.Name;

        if (name is "Include" or "ThenInclude") {
            HasInclude = true;
            return base.VisitMethodCall(node);
        }

        // to let Property / EF.Property pass
        if (name is "Property") {
            return base.VisitMethodCall(node);
        }

        // AsSplitQuery is needed with multiple includes 
        if (name is "AsSplitQuery") {
            return base.VisitMethodCall(node);
        }
        
        HasForbiddenCalls = true;
        FirstForbiddenMethodName = name;
        return node;
    }

    public static void EnsureOnlyIncludes<T>(
        IQueryable<T> query,
        string caller,
        string callerFilePath,
        int callerLineNumber
    ) {
        var v = new OnlyIncludesVisitor();
        v.Visit(query.Expression);

        if (!v.HasInclude) {
            UnexpectedBehaviourException.ThrowErr(
                err: ErrFactory.ProgramBug(
                    $"At least one Include/ThenInclude call is required in '{caller}'. " +
                    $"File: {callerFilePath}. Line number: {callerLineNumber}"
                ),
                userMessage: "Something went wrong while processing your request. Please try again late",
                caller: caller
            );
        }

        if (v.HasForbiddenCalls) {
            UnexpectedBehaviourException.ThrowErr(
                err: ErrFactory.ProgramBug(
                    $"Only Include/ThenInclude and Property calls are allowed here. " +
                    $"Forbidden call: {v.FirstForbiddenMethodName} passed to '{caller}' method. " +
                    $"File: {callerFilePath}. Line number: {callerLineNumber}"
                ),
                userMessage: "Something went wrong while processing your request. Please try again late",
                caller: caller
            );
        }
    }
}