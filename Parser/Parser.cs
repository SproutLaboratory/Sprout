using System;
using System.Collections.Generic;

namespace SproutInterpreter
{
    public partial class Parser
    {
        private List<Token> tokens;
        private int pos;
        private Token currentToken;
        private bool enableLogging = false;

        public Parser(List<Token> tokens, bool enableLogging = false)
        {
            this.tokens = tokens;
            pos = 0;
            currentToken = tokens.Count > 0 ? tokens[0] : null;
            this.enableLogging = enableLogging;
        }

        public List<ASTNode> Parse()
        {
            var nodes = new List<ASTNode>();
            while (currentToken != null && currentToken.Type != Token.TokenType.EOF)
            {
                if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                var node = ParseStatement();
                if (node != null) nodes.Add(node);
                if (Check(Token.TokenType.Semicolon)) Advance();
                while (Check(Token.TokenType.NewLine)) Advance();
            }
            return nodes;
        }
    }
}