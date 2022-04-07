using Calculator.Core.Interpreting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Parsing {
    public class NodeVar : Node {

        private string varName;

        public NodeVar(string varName) {
            this.varName = varName;
        }

        public override double Eval(IContext ctx) {
            return ctx.GetVariable(varName);
        }    }
}
