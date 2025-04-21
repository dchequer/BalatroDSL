using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Enter hand definition (or type 'exit' to quit):");

            string? line;
            var inputLines = new List<string>();

            while ((line = Console.ReadLine()) != null)
            {
                if (line.Trim().ToLower() == "exit")
                    return;

                inputLines.Add(line);

                if (line.Trim() == "}")
                    break;
            }

            var fullInput = string.Join(Environment.NewLine, inputLines);

            try
            {
                var scanner = new Scanner(new MemoryStream(Encoding.UTF8.GetBytes(fullInput)));
                var parser = new Parser(scanner);
                parser.Parse();

                if (parser.errors.count == 0)
                {
                    Console.WriteLine("Valid hand definition.");
                    // convert to ast and eval
                }
                else
                {
                    Console.WriteLine("Invalid hand definition.");
                    Console.WriteLine($"{parser.errors.count} errors found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during parsis: {ex.Message}");
            }
        }
    }
}