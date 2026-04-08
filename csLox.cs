using System.Text;

namespace csLox;

public class csLox
{
    static bool HadError = false;
    public static void Main(String[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: cslox [script]");
            Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
    }

    private static void RunFile(String path)
    {
        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
        Run(new String(Encoding.ASCII.GetChars(bytes)));

        if (HadError) Environment.Exit(65);
    }

    private static void RunPrompt()
    {
        TextReader reader = Console.In;
        for (; ; )
        {
            Console.Write(">");
            string? line = reader.ReadLine();
            if (line == null) break;
            Run(line);
            HadError = false;
        }
    }

    private static void Run(String source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, String message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, String where, String message)
    {
        Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
        HadError = true;
    }
}
