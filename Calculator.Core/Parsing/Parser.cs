using Calculator.Core.Interpreting;
using Calculator.Core.Lexing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Calculator.Core.Parsing {
    public class Parser {

        public static Node Parse(string s) {
            return new Parser(new Lexer(s)).ParseExpression();
        }

        public static double Eval(string s, IContext ctx) {
            return Parse(s).Eval(ctx);
        }

        private Lexer lexer;

        public Parser(Lexer lexer) {
            this.lexer = lexer;
        }

        public Node ParseExpression() {
            Node expr = ParseAssignment();

            if(lexer.CurrentToken.Type != TokenType.EOF) {
                throw new SyntaxException("Unexpected characters at the end of the expression");
            }

            return expr;
        }

        private Node ParseAssignment() {
            int startIndex = lexer.CurrentIndex;
            if(lexer.CurrentToken.Type == TokenType.IDENTIFIER 
                && lexer.NextToken().Type == TokenType.ASSIGN) {
                lexer.NextToken();
                Node rhs = ParseLogicOr();
                return new NodeAssign(lexer.Tokens[startIndex].Text, rhs);
            }
            lexer.CurrentIndex = startIndex;
            return ParseLogicOr();
        }

        /// <summary>
        /// Generic Parser Method for the different operators in the order of operations
        /// </summary>
        /// <param name="nextOperator">Next operator group method in the order of operations</param>
        /// <param name="tokenToOperator">Mapping of the operator tokens in the group to specific functions</param>
        /// <returns></returns>
        private Node ParseOperator(Func<Node> nextOperator, Func<TokenType, Func<double, double, double>> tokenToOperator) {
            Node lhs = nextOperator();

            while (true) {
                Func<double, double, double> op = tokenToOperator(lexer.CurrentToken.Type);

                if (op == null)
                    return lhs;

                lexer.NextToken();

                Node rhs = nextOperator();
                lhs = new NodeBinary(lhs, rhs, op);
            }
        }

        private Node ParseLogicOr() {

            return ParseOperator(ParseLogicAnd, (t) => {
                return t switch {
                    TokenType.LOGIC_OR => (a, b) => a > 0 || b > 0 ? 1 : 0,
                    _ => null
                };
            });
        }

        private Node ParseLogicAnd() {

            return ParseOperator(ParseBitwiseOr, (t) => {
                return t switch {
                    TokenType.LOGIC_AND => (a, b) => a > 0 && b > 0 ? 1 : 0,
                    _ => null
                };
            });
        }

        private Node ParseBitwiseOr() {

            return ParseOperator(ParseBitwiseAnd, (t) => {
                return t switch {
                    TokenType.BIT_OR => (a, b) => (int)a | (int)b,
                    TokenType.BIT_XOR => (a, b) => (int)a ^ (int)b,
                    _ => null
                };
            });
        }

        private Node ParseBitwiseAnd() {

            return ParseOperator(ParseEquality, (t) => {
                return t switch {
                    TokenType.BIT_AND => (a, b) => (int)a & (int)b,
                    _ => null
                };
            });
        }

        private Node ParseEquality() {

            return ParseOperator(ParseCompare, (t) => {
                return t switch {
                    TokenType.EQUALS => (a, b) => a == b ? 1 : 0,
                    TokenType.NOT_EQ => (a, b) => a != b ? 1 : 0,
                    _ => null
                };
            });
        }

        private Node ParseCompare() {

            return ParseOperator(ParseBitShift, (t) => {
                return t switch {
                    TokenType.LESS => (a, b) => a < b ? 1 : 0,
                    TokenType.LESS_EQ => (a, b) => a <= b ? 1 : 0,
                    TokenType.GREATER => (a, b) => a > b ? 1 : 0,
                    TokenType.GREATER_EQ => (a, b) => a >= b ? 1 : 0,
                    _ => null
                };
            });
        }

        private Node ParseBitShift() {

            return ParseOperator(ParseAddition, (t) => {
                return t switch {
                    TokenType.BITSHIFT_RIGHT => (a, b) => (int)a >> (int)b,
                    TokenType.BITSHIFT_LEFT => (a, b) => (int)a << (int)b,
                    _ => null
                };
            });
        }

        private Node ParseAddition() {

            return ParseOperator(ParseMultiplication, (t) => {
                return t switch {
                    TokenType.ADD => (a, b) => a + b,
                    TokenType.SUB => (a, b) => a - b,
                    _ => null
                };
            });
        }

        private Node ParseMultiplication() {

            return ParseOperator(ParsePower, (t) => {
                return t switch {
                    TokenType.MULT => (a, b) => a * b,
                    TokenType.DIV => (a, b) => a / b,
                    TokenType.INT_DIV => (a, b) => (int)a / (int)b,
                    TokenType.MOD => (a, b) => a % b,
                    _ => null
                };
            });
        }

        private Node ParsePower() {

            return ParseOperator(ParseUnary, (t) => {
                return t switch {
                    TokenType.POWER => (a, b) => Math.Pow(a, b),
                    _ => null
                };
            });
        }

        private Node ParseUnary() {
            
            switch(lexer.CurrentToken.Type) {
                case TokenType.ADD:  // noop -> skip
                    lexer.NextToken();
                    return ParseUnary();
                case TokenType.SUB: {
                        lexer.NextToken();
                        Node rhs = ParseUnary();
                        return new NodeUnary(rhs, (a) => -a);
                    }
                case TokenType.BIT_COMPLEMENT: {
                        lexer.NextToken();
                        Node rhs = ParseUnary();
                        return new NodeUnary(rhs, (a) => ~(int)a);
                    }
                case TokenType.NOT: {
                        lexer.NextToken();
                        Node rhs = ParseUnary();
                        return new NodeUnary(rhs, (a) => a == 0 ? 1 : 0);
                    }
            }

            return ParseHighest();
        }

        private Node ParseHighest() {
            
            switch(lexer.CurrentToken.Type) {
                case TokenType.NUM: {
                        Node node = new NodeNum(double.Parse(lexer.CurrentToken.Text, CultureInfo.InvariantCulture));
                        lexer.NextToken();
                        return node;
                    }
                case TokenType.OPEN_PARENS: {
                        lexer.NextToken(); // skip '('
                        Node node = ParseAssignment(); // new top level parsing
                        if (lexer.CurrentToken.Type != TokenType.CLOSE_PARENS)
                            throw new SyntaxException("Missing closing parenthesis!");
                        lexer.NextToken();
                        return node;
                    }
                case TokenType.TRUE: {
                        Node node = new NodeNum(1);
                        lexer.NextToken();
                        return node;
                    }
                case TokenType.FALSE: {
                        Node node = new NodeNum(0);
                        lexer.NextToken();
                        return node;
                    }
                case TokenType.IDENTIFIER: {
                        string name = lexer.CurrentToken.Text;
                        lexer.NextToken();

                        if(lexer.CurrentToken.Type != TokenType.OPEN_PARENS) {
                            return new NodeVar(name);
                        }

                        lexer.NextToken();

                        var args = new List<Node>();
                        while(true) {
                            args.Add(ParseAssignment());

                            if(lexer.CurrentToken.Type == TokenType.COMMA) {
                                lexer.NextToken();
                                continue;
                            }

                            break; // all args done
                        }

                        if(lexer.CurrentToken.Type != TokenType.CLOSE_PARENS)
                            throw new SyntaxException("Missing closing parenthesis!");
                        lexer.NextToken();

                        return new NodeFunction(name, args.ToArray());
                    }
            }

            throw new SyntaxException($"Unexpected Token: {lexer.CurrentToken.Text} at position {lexer.CurrentToken.StartPos}");
        }

    }

}
