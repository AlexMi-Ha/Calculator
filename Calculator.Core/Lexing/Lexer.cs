using Calculator.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core.Lexing {
    public class Lexer {

        private string input;

        private List<Token> tokens;
        public List<Token> Tokens {
            get {
                if (tokens == null || tokens.Count <= 0) Lex();
                return tokens;
            }
        }

        public int CurrentIndex { get; set; } = 0;
        public Token CurrentToken { 
            get {
                return Tokens[CurrentIndex];
            } 
        }
        public Token NextToken() {
            if (CurrentIndex + 1 < Tokens.Count) ++CurrentIndex;
            return CurrentToken;
        }

        public Lexer(string input) {
            this.input = input;
        }

        public List<Token> Lex() {
            tokens = new List<Token>();
            int currentPos = 0;

            while (currentPos < input.Length) {
                int startPos = currentPos;
                char lookahead = input[currentPos];
                if (IsWhitespace(lookahead)) {
                    ++currentPos;  // skip whitespaces
                    continue;
                }
                // Test for operators and other symbols
                switch(lookahead) {
                    case '+':
                        ++currentPos;
                        tokens.Add(new Token(TokenType.ADD, lookahead.ToString(), startPos));
                        continue;
                    case '-':
                        ++currentPos;
                        tokens.Add(new Token(TokenType.SUB, lookahead.ToString(), startPos));
                        continue;
                    case '*':
                        if (input[++currentPos] == '*') {
                            tokens.Add(new Token(TokenType.POWER, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        } else {
                            tokens.Add(new Token(TokenType.MULT, lookahead.ToString(), startPos));
                        }
                        continue;
                    case '/':
                        if (input[++currentPos] == '/') {
                            tokens.Add(new Token(TokenType.INT_DIV, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        } else {
                            tokens.Add(new Token(TokenType.DIV, lookahead.ToString(), startPos));
                        }
                        continue;
                    case '%':
                        ++currentPos;
                        tokens.Add(new Token(TokenType.MOD, lookahead.ToString(), startPos));
                        continue;
                    case '^':
                        ++currentPos;
                        tokens.Add(new Token(TokenType.BIT_XOR, lookahead.ToString(), startPos));
                        continue;
                    case '&':
                        if (input[++currentPos] == '&') {
                            tokens.Add(new Token(TokenType.LOGIC_AND, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        } else {
                            tokens.Add(new Token(TokenType.BIT_AND, lookahead.ToString(), startPos));
                        }
                        continue;
                    case '|':
                        if (input[++currentPos] == '|') {
                            tokens.Add(new Token(TokenType.LOGIC_OR, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        } else {
                            tokens.Add(new Token(TokenType.BIT_OR, lookahead.ToString(), startPos));
                        }
                        continue;
                    case '!':
                        if(input[++currentPos] == '=') {
                            tokens.Add(new Token(TokenType.NOT_EQ, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        }else {
                            tokens.Add(new Token(TokenType.NOT, lookahead.ToString(), startPos));
                        }
                        continue;
                    case '=':
                        if (input[++currentPos] == '=') {
                            tokens.Add(new Token(TokenType.EQUALS, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        } else {
                            tokens.Add(new Token(TokenType.ASSIGN, lookahead.ToString(), startPos));
                        }
                        continue;
                    case '>':
                        if (input[++currentPos] == '=') {
                            tokens.Add(new Token(TokenType.GREATER_EQ, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        } else if (input[currentPos] == '>') {
                            tokens.Add(new Token(TokenType.BITSHIFT_RIGHT, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        } else {
                            tokens.Add(new Token(TokenType.GREATER, lookahead.ToString(), startPos));
                        }
                        continue;
                    case '<':
                        if (input[++currentPos] == '=') {
                            tokens.Add(new Token(TokenType.LESS_EQ, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        } else if (input[currentPos] == '<') {
                            tokens.Add(new Token(TokenType.BITSHIFT_LEFT, lookahead.ToString() + input[currentPos].ToString(), startPos));
                            ++currentPos;
                        } else {
                            tokens.Add(new Token(TokenType.LESS, lookahead.ToString(), startPos));
                        }
                        continue;
                    case '(':
                        ++currentPos;
                        tokens.Add(new Token(TokenType.OPEN_PARENS, lookahead.ToString(), startPos));
                        continue;
                    case ')':
                        ++currentPos;
                        tokens.Add(new Token(TokenType.CLOSE_PARENS, lookahead.ToString(), startPos));
                        continue;
                    case ',':
                        ++currentPos;
                        tokens.Add(new Token(TokenType.COMMA, lookahead.ToString(), startPos));
                        continue;
                    case '~':
                        ++currentPos;
                        tokens.Add(new Token(TokenType.BIT_COMPLEMENT, lookahead.ToString(), startPos));
                        continue;
                }

                // Test for letters
                if(IsLetter(lookahead)) {
                    string text = "";
                    // Add all following letters or digits to the identifier
                    while(currentPos < input.Length && IsLetterOrDigit(input[currentPos])) {
                        text += input[currentPos];
                        ++currentPos;
                    }
                    // is it an identifier or a specific literal?
                    TokenType type = text switch {
                        "true" => TokenType.TRUE,
                        "false" => TokenType.FALSE,
                        _ => TokenType.IDENTIFIER
                    };
                    tokens.Add(new Token(type, text, startPos));

                // Test for numbers
                } else if(IsDigit(lookahead) || lookahead == '.') {
                    string text = "";
                    bool hasExp = false;
                    bool hasDecimal = false;
                    // loop through following symbols
                    while (currentPos < input.Length 
                        && (IsDigit(input[currentPos])  // digits 
                        || (Char.ToLower(input[currentPos]) == 'e' && !hasExp) //exponent symbol
                        || (input[currentPos] == '-' && hasExp) // or negative symbol for a negative exponent 
                        || (input[currentPos] == '.' && !hasDecimal && !hasExp))) { //Decimal point

                        // only allow negative symbol after the exponent symbol
                        if (input[currentPos] == '-' && Char.ToLower(input[currentPos - 1]) != 'e') 
                            break;
                        // set exponent flag
                        if (Char.ToLower(input[currentPos]) == 'e') {
                            hasExp = true;
                            if (text.EndsWith('.')) text += '0';
                        }
                        if (input[currentPos] == '.')
                            hasDecimal = true;

                        text += input[currentPos];
                        ++currentPos;
                    }
                    // if the number ends with the exponent symbol -> remove it; does not belong to the number
                    if(text.EndsWith("e")) {
                        text = text.Substring(0, text.Length - 1);
                        --currentPos;
                    }
                    if (text.EndsWith(".")) text += '0';
                    tokens.Add(new Token(TokenType.NUM, text, startPos));

                // All other characters should throw an exception
                } else {
                    throw new UnknownCharacterException($"Unknown character {lookahead} at position {currentPos}");
                }
            }

            // Add the end of file marker token to the end
            tokens.Add(new Token(TokenType.EOF, "<EOF>", currentPos));
            return tokens;
        }


        #region Helpers
        private bool IsWhitespace(char inp) {
            return Char.IsWhiteSpace(inp);
        }

        private bool IsDigit(char inp) {
            return Char.IsDigit(inp);
        }

        private bool IsLetter(char inp) {
            return Char.IsLetter(inp);
        }

        private bool IsLetterOrDigit(char inp) {
            return Char.IsLetterOrDigit(inp);
        }

        #endregion
    }
}
