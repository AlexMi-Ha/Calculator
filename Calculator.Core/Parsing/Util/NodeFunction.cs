using Calculator.Core.Interpreting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Parsing {
    public class NodeFunction : Node {

        private string funcName;
        private Node[] args;

        public NodeFunction(string funcName, Node[] args) {
            this.funcName = funcName;
            this.args = args;
        }

        public override double Eval(IContext ctx) {
            double[] argVals = new double[args.Length];
            for(int i = 0; i < args.Length; ++i) {
                argVals[i] = args[i].Eval(ctx);
            }
            return ctx.CallFunction(funcName, argVals);
        }
    }
}
