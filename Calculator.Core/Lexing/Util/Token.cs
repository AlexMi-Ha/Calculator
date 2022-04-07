using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Lexing {
    public class Token {

        public TokenType Type { get; set; }
        public string Text { get; set; }
        public int StartPos { get; set; }

        public Token(TokenType type, string text, int startPos) {
            this.Type = type;
            this.Text = text;
            this.StartPos = startPos;
        }

    }
}
