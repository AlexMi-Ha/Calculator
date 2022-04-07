using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core {
    public class SyntaxException : Exception {
        public SyntaxException(string message) : base(message) {
        }
    }
}
