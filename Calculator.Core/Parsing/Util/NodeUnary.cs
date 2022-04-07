using Calculator.Core.Interpreting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Parsing {
    public class NodeUnary : Node {

        private Node rhs;
        private Func<double, double> op;

        public NodeUnary(Node rhs, Func<double, double> op) {
            this.rhs = rhs;
            this.op = op;
        }

        public override double Eval(IContext ctx) {
            return op(rhs.Eval(ctx));
        }
     }
}
