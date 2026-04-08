namespace csLox.Tool;
public class GenerateAst
{
    // public static void Main(string[] args)
    // {
    //     if (args.Length != 1)
    //     {
    //         Console.Error.WriteLine("Usage: generate_ast <output_directory>");
    //         Environment.Exit(64);
    //     }
    //     String outputDir = args[0];
    //     defineAst(outputDir, "Expr", new List<String> {
    //         "Binary   : Expr left, Token op, Expr right",
    //         "Grouping : Expr expression",
    //         "Literal  : Object value",
    //         "Unary    : Token op, Expr right"
    //     });
    // }

    private static void defineAst(string outputDir, string baseName, List<string> types)
    {
        String path = outputDir + "/" + baseName + ".cs";
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("");
            writer.WriteLine("namespace csLox");
            writer.WriteLine("{");

            writer.WriteLine($"    public abstract class {baseName}");
            writer.WriteLine("    {");

            defineVisitor(writer, baseName, types);

            foreach (string type in types)
            {
                string className = type.Split(':')[0].Trim();
                string fields = type.Split(':')[1].Trim();
                defineType(writer, baseName, className, fields);
            }
            writer.WriteLine("");
            writer.WriteLine("        public abstract T Accept<T>(IVisitor<T> visitor);");

            writer.WriteLine("    }");
        }

    }

    private static void defineVisitor(StreamWriter writer, string baseName, List<string> types)
    {
        writer.WriteLine("public interface IVisitor<T> {");
        foreach (string type in types)
        {
            string typeName = type.Split(':')[0].Trim();
            writer.WriteLine($"    T Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
        }
        writer.WriteLine("    }");
    }

        private static void defineType(StreamWriter writer, string baseName, string className, string fields)
        {
            writer.WriteLine($"        public class {className} : {baseName}");
            writer.WriteLine("        {");

            string[] fieldList = fields.Split(", ");
            foreach (string field in fieldList)
            {
                writer.WriteLine($"            public {field};");
            }

            writer.WriteLine("");
            writer.WriteLine($"            public {className}({fields})");
            writer.WriteLine("            {");

            foreach (string field in fieldList)
            {
                string name = field.Split(' ')[1];
                writer.WriteLine($"                this.{name} = {name};");
            }

            writer.WriteLine("            }");
            writer.WriteLine("");
            writer.WriteLine($"            public override T Accept<T>(IVisitor<T> visitor)");
            writer.WriteLine("            {");
            writer.WriteLine($"                return visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("            }");
            writer.WriteLine("        }");

        }
}