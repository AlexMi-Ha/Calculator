using Calculator.Core.Interpreting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Parsing {
    public class NodeBinary : Node {

        private Node lhs, rhs;
        private Func<double, double, double> op;

        public NodeBinary(Node lhs, Node rhs, Func<double, double, double> op) {
            this.lhs = lhs;
            this.rhs = rhs;
            this.op = op;
        }

        public override double Eval(IContext ctx) {
            return op(lhs.Eval(ctx), rhs.Eval(ctx));
        }
    }
}
