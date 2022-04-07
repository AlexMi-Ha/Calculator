using Calculator.Core.Interpreting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Parsing {
    public class NodeNum : Node {

        private double num;

        public NodeNum(double num) {
            this.num = num;
        }

        public override double Eval(IContext ctx) {
            return num;
        }
    }
}
