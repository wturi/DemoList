using System;
using System.IO;

namespace SetOutDemo
{
    internal class Program
    {
        private const int tabSize = 4;
        private const string usageText = "Usage: INSERTTABS inputfile.txt outputfile.txt";

        private static void Main(string[] args)
        {
            StreamWriter writer = null;

            if (args.Length < 2)
            {
                Console.WriteLine(usageText);
            }

            try
            {
                // Attempt to open output file.
                writer = new StreamWriter(args[1]);
                // Redirect standard output from the console to the output file.
                Console.SetOut(writer);
                // Redirect standard input from the console to the input file.
                Console.SetIn(new StreamReader(args[0]));
            }
            catch (IOException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                errorWriter.WriteLine(usageText);
            }
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                string newLine = line.Replace(("").PadRight(tabSize, ' '), "\t");
                Console.WriteLine(newLine);
            }
            writer.Close();
            // Recover the standard output stream so that a
            // completion message can be displayed.
            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            Console.WriteLine("INSERTTABS has completed the processing of {0}.", args[0]);

            Console.ReadLine();
        }
    }
}