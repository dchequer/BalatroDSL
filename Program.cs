using BalatroDSL.AST;
using BalatroDSL.Scoring;
using System;
using System.Diagnostics;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Enter cards (or type 'exit' to quit):");
            var expression = Console.ReadLine();

            if (string.IsNullOrEmpty(expression) || expression.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                break;
            }

            try
            {
                var scanner = new Scanner(new MemoryStream(Encoding.UTF8.GetBytes(expression.ToUpper())));
                var parser = new Parser(scanner);
                parser.Parse();

                if (parser.errors.count == 0)
                {
                    Console.WriteLine("Valid hand definition.");
                    // convert to ast and eval

                    parser.currentHand.Cards.ForEach(card =>
                    {
                        Console.WriteLine($"Card: {card}");
                    });

                    var ast = HandScorer.BuildScoringTree(parser.currentHand);
                    var dot = new DotExporter().Export(ast);

                    // output "scored" hand as well as score
                    Console.WriteLine($"Scored: {parser.currentHand.label} with {parser.currentHand.chips * parser.currentHand.multiplier} chips");

                    File.WriteAllText("score_tree.dot", dot);

                    // create image using dot and graphviz
                    var dotPath = "dot"; 
                    var imageFile = "score_tree.png";

                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = dotPath,
                        Arguments = string.Join(" ", new[]
                        {
                            "-Tpng",
                            "-Gdpi=600",
                            "-Gbgcolor=\"#1e1e1e\"",
                            "-Nfontname=\"Segoe UI\"",
                            "-Nfontsize=14",
                            "-Ncolor=\"#f0f0f0\"",
                            "-Nfontcolor=\"#f0f0f0\"",
                            "-Nstyle=filled",
                            "-Nfillcolor=\"#2d2d2d\"",
                            //"-rankdir=LR",
                            "-Ecolor=\"#808080\"",
                            $"score_tree.dot -o {imageFile}"
                        }),
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                    process.WaitForExit();

                    if (File.Exists(imageFile))
                    {
                        Console.WriteLine($"Scoring tree image created: {imageFile}");
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = imageFile,
                            UseShellExecute = true
                        });
                    }
                    else
                    {
                        Console.WriteLine("Failed to create scoring tree image.");
                    }

                }
                else
                {
                    Console.WriteLine("Invalid hand definition.");
                    Console.WriteLine($"{parser.errors.count} errors found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during parsing: {ex.Message}");
            }
        }
    }
}