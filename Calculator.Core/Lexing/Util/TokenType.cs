using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Lexing {
    public enum TokenType {
        EOF,
        // Operators
        ADD,
        SUB,
        MULT,
        INT_DIV,
        DIV,
        MOD,
        POWER,  // a to the power of b
        // Logic operators
        LOGIC_AND,
        LOGIC_OR,
        NOT,
        NOT_EQ,
        EQUALS,
        GREATER,
        GREATER_EQ,
        LESS,
        LESS_EQ,
        // Bitwise operators
        BIT_AND,
        BIT_OR,
        BIT_XOR,
        BITSHIFT_RIGHT,
        BITSHIFT_LEFT,
        BIT_COMPLEMENT,
        // Other symbols
        OPEN_PARENS,
        CLOSE_PARENS,
        COMMA,
        ASSIGN,
        // Text based
        IDENTIFIER,
        // Literals
        TRUE,
        FALSE,
        NUM,
    }
}
