using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Interpreting {
    public interface IContext {

        public double SetVariable(string varName, double val);

        public double GetVariable(string varName);

        public double CallFunction(string funcName, double[] args);

    }
}
