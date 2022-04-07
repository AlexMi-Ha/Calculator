using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Exceptions {
    public class UnknownCharacterException : Exception {
        public UnknownCharacterException(string message) : base(message) {
        }
    }
}
