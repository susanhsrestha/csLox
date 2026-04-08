namespace csLox;

public class Token(
    TokenType type,
    String lexeme,
    Object literal,
    int line
)
{
    public TokenType type = type;
    public String lexeme = lexeme;
    public Object literal = literal;
    public int line = line;
    public override String ToString()
    {
        return type + " " + lexeme + " " + literal;
    }
}