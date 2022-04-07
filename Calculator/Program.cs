using Calculator.Core.Interpreting;
using Calculator.Core.Parsing;
using System;

namespace Calculator.CLI {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            var lib = new StandardLibrary();
            while (true) {
                string inp = Console.ReadLine();
                Console.WriteLine("= " + Parser.Eval(inp, lib));
            }
        }
    }
}
