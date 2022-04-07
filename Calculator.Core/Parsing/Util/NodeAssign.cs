using Calculator.Core.Interpreting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Parsing {
    public class NodeAssign : Node {

        private string varname;
        private Node rhs;

        public NodeAssign(string varname, Node rhs) {
            this.varname = varname;
            this.rhs = rhs;
        }

        public override double Eval(IContext ctx) {
            return ctx.SetVariable(varname, rhs.Eval(ctx));
        }
    }
}
