using csLox;
using System.Text;
using static csLox.Expr;

public class AstPrinter : IVisitor<string>
{
    public string VisitBinaryExpr(Binary expr)
    {
        return Parenthesize(expr.op.lexeme, expr.left, expr.right);
    }

    public string VisitGroupingExpr(Grouping expr)
    {
        return Parenthesize("group", expr.expression);
    }

    public string VisitLiteralExpr(Literal expr)
    {
        return expr.value.ToString()!;
    }

    public string VisitUnaryExpr(Unary expr)
    {
        return Parenthesize(expr.op.lexeme, expr.right);
    }

    public string Print(Expr expr)
    {
        return expr.Accept(this);
    }

    private string Parenthesize(string name, params Expr[] expressions)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("(");
        builder.Append(name);
        foreach (Expr expr in expressions)
        {
            builder.Append(" ");
            builder.Append(expr.Accept(this));
        }
        builder.Append(")");
        return builder.ToString();
    }

    // public static void Main(string[] args)
    // {
    //     Expr expression = new Binary(
    //         new Unary(new Token(TokenType.MINUS, "-", null!, 1), new Literal(123)),
    //         new Token(TokenType.STAR, "*", null!, 1),
    //         new Grouping(new Literal(45.67))
    //     );

    //     Console.WriteLine(new AstPrinter().Print(expression));
    // }
}