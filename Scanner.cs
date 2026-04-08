namespace csLox;

public class Scanner(String source)
{
    private List<Token> tokens = new();
    private int start = 0;
    private int current = 0;
    private int line = 1;

    private static Dictionary<String, TokenType> keywords = new()
    {
        {"and", TokenType.AND},
        {"class", TokenType.CLASS},
        {"else", TokenType.ELSE},
        {"false", TokenType.FALSE},
        {"for", TokenType.FOR},
        {"fun", TokenType.FUN},
        {"if", TokenType.IF},
        {"nil", TokenType.NIL},
        {"or", TokenType.OR},
        {"print", TokenType.PRINT},
        {"return", TokenType.RETURN},
        {"super", TokenType.SUPER},
        {"this", TokenType.THIS},
        {"true", TokenType.TRUE},
        {"var", TokenType.VAR},
        {"while", TokenType.WHILE},
    };

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            start = current;
            ScanToken();
        }

        tokens.Add(new Token(TokenType.EOF, "", null!, line));
        return tokens;
    }

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            case '!':
                AddToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                AddToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                AddToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                AddToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '/':
                if (match('/'))
                {
                    while (peek() != '\n' && !IsAtEnd()) Advance();
                }
                else if (match('*'))
                {
                    while (peek() != '*' && peekNext() != '/') Advance();
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                break;

            case '\n':
                line++;
                break;

            case '"':
                Stringg();
                break;

            case 'o':
                if (match('r'))
                {
                    AddToken(TokenType.OR);
                }
                break;

            default:
                if (IsDigit(c))
                {
                    Number();
                }
                else if (IsAlpha(c))
                {
                    Identifier();
                }
                else
                {
                    csLox.Error(line, "Unexpected character.");
                }
                break;
        }
    }

    private bool IsAlpha(char c)
    {
        return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '_';
    }

    private void Identifier()
    {
        while (IsAlphaNumeric(peek())) Advance();

        String text = source.Substring(start, current - start);
        TokenType type = keywords.GetValueOrDefault(text, TokenType.IDENTIFIER);
        //if (type == null) type = TokenType.IDENTIFIER;
        AddToken(type);
    }

    private bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }

    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private void Number()
    {
        while (IsDigit(peek())) Advance();

        // Look for fractional part.
        if (peek() == '.' && IsDigit(peekNext()))
        {
            Advance();

            while (IsDigit(peek())) Advance();
        }

        AddToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
    }

    private char peekNext()
    {
        if (current + 1 >= source.Length) return '\0';
        return source[current + 1];
    }

    private void Stringg()
    {
        while (peek() != '"' && !IsAtEnd())
        {
            if (peek() == '\n') line++;
            Advance();
        }

        if (IsAtEnd())
        {
            csLox.Error(line, "Unterminated string.");
            return;
        }

        Advance();

        String value = source.Substring(start + 1, current - 1 - start - 1);
        AddToken(TokenType.STRING, value);
    }

    private char peek()
    {
        if (IsAtEnd()) return '\0';
        return source[current];
    }

    private bool match(char expected)
    {
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false;

        current++;
        return true;
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null!);
    }

    private void AddToken(TokenType type, object literal)
    {
        String text = source.Substring(start, current - start);
        tokens.Add(new Token(type, text, literal, line));
    }

    private char Advance()
    {
        return source[current++];
    }

    private bool IsAtEnd()
    {
        return current >= source.Length;
    }
}