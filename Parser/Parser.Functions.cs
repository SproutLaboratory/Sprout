using System;
using System.Collections.Generic;

namespace SproutInterpreter
{
    public partial class Parser
    {
        private FunctionDefNode ParseFunctionDef()
        {
            Advance();
            string name = currentToken.Value;
            Advance();
            var node = new FunctionDefNode { Name = name };
            
            Expect(Token.TokenType.Punctuation, "(");
            while (currentToken != null && currentToken.Value != ")")
            {
                node.Parameters.Add(currentToken.Value);
                Advance();
                if (Check(Token.TokenType.Punctuation, ",")) Advance();
            }
            Expect(Token.TokenType.Punctuation, ")");

            if (Check(Token.TokenType.Global))
            {
                Advance();
                node.Scope = "global";
            }
            else if (Check(Token.TokenType.Local))
            {
                Advance();
                node.Scope = "local";
            }
            else
            {
                node.Scope = "local";
            }

            if (IsPunctuation("{"))
            {
                Advance();
                while (currentToken != null && currentToken.Value != "}")
                {
                    if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                    var stmt = ParseStatement();
                    if (stmt != null) node.Body.Add(stmt);
                    if (Check(Token.TokenType.Semicolon)) Advance();
                    while (Check(Token.TokenType.NewLine)) Advance();
                }
                Expect(Token.TokenType.Punctuation, "}");
            }
            else
            {
                var stmt = ParseStatement();
                if (stmt != null) node.Body.Add(stmt);
            }
            return node;
        }

        private ASTNode ParseReturn()
        {
            Advance();
            if (Check(Token.TokenType.Send))
            {
                Advance();
                var node = new ReturnSendNode();
                node.Value = ParseExpression();
                return node;
            }

            if (Check(Token.TokenType.Run))
            {
                Advance();
                var node = new ReturnRunNode();
                if (IsPunctuation("{"))
                {
                    Advance();
                    while (currentToken != null && currentToken.Value != "}")
                    {
                        if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                        var stmt = ParseStatement();
                        if (stmt != null) node.Body.Add(stmt);
                        if (Check(Token.TokenType.Semicolon)) Advance();
                        while (Check(Token.TokenType.NewLine)) Advance();
                    }
                    Expect(Token.TokenType.Punctuation, "}");
                }
                else
                {
                    var stmt = ParseStatement();
                    if (stmt != null) node.Body.Add(stmt);
                }
                return node;
            }

            throw new Exception("Ожидалось send или run после return");
        }
    }
}