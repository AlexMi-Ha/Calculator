using Calculator.Core.Interpreting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Parsing {
    public abstract class Node {
        public abstract double Eval(IContext ctx);
    }
}
