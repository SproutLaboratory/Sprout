using System;
using System.Collections.Generic;

namespace SproutInterpreter
{
    public partial class Parser
    {
        private IfNode ParseIf()
        {
            Advance();
            var node = new IfNode();
            node.Condition = ParseExpression();
            
            if (node.Condition == null)
                throw new Exception("Ожидалось условие после if");

            if (IsPunctuation("{"))
            {
                Advance();
                while (currentToken != null && currentToken.Value != "}")
                {
                    if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                    var stmt = ParseStatement();
                    if (stmt != null) node.ThenBody.Add(stmt);
                    if (Check(Token.TokenType.Semicolon)) Advance();
                    while (Check(Token.TokenType.NewLine)) Advance();
                }
                Expect(Token.TokenType.Punctuation, "}");
            }
            else
            {
                var stmt = ParseStatement();
                if (stmt != null) node.ThenBody.Add(stmt);
            }

            while (Check(Token.TokenType.Elif))
            {
                Advance();
                var elifNode = new IfNode();
                elifNode.Condition = ParseExpression();
                
                if (IsPunctuation("{"))
                {
                    Advance();
                    while (currentToken != null && currentToken.Value != "}")
                    {
                        if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                        var stmt = ParseStatement();
                        if (stmt != null) elifNode.ThenBody.Add(stmt);
                        if (Check(Token.TokenType.Semicolon)) Advance();
                        while (Check(Token.TokenType.NewLine)) Advance();
                    }
                    Expect(Token.TokenType.Punctuation, "}");
                }
                else
                {
                    var stmt = ParseStatement();
                    if (stmt != null) elifNode.ThenBody.Add(stmt);
                }
                node.ElseBody.Add(elifNode);
            }

            if (Check(Token.TokenType.Else))
            {
                Advance();
                if (IsPunctuation("{"))
                {
                    Advance();
                    while (currentToken != null && currentToken.Value != "}")
                    {
                        if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                        var stmt = ParseStatement();
                        if (stmt != null) node.ElseBody.Add(stmt);
                        if (Check(Token.TokenType.Semicolon)) Advance();
                        while (Check(Token.TokenType.NewLine)) Advance();
                    }
                    Expect(Token.TokenType.Punctuation, "}");
                }
                else
                {
                    var stmt = ParseStatement();
                    if (stmt != null) node.ElseBody.Add(stmt);
                }
            }

            return node;
        }
    }
}