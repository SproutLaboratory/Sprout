using System;
namespace SproutInterpreter
{
    public partial class Parser
    {
        private RepeatNode ParseRepeat()
        {
            Advance();
            var node = new RepeatNode();
            node.Count = ParseExpression();
            if (Check(Token.TokenType.Times))
            {
                Advance();
                if (IsIdentifier()) { node.Variable = currentToken.Value; Advance(); }
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

        private ForNode ParseFor()
        {
            Advance();
            var node = new ForNode();
            node.Variable = currentToken.Value;
            Advance();

            if (!Check(Token.TokenType.To))
                throw new Exception("Ожидалось 'to' в цикле for");
            Advance();

            node.End = ParseExpression();

            if (Check(Token.TokenType.Punctuation, ","))
            {
                Advance();
                node.Start = ParseExpression();
            }
            else
            {
                node.Start = new NumberNode(0);
            }

            if (Check(Token.TokenType.Punctuation, ","))
            {
                Advance();
                node.Step = ParseExpression();
            }
            else
            {
                node.Step = new NumberNode(1);
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

        private WhileNode ParseWhile()
        {
            Advance();
            var node = new WhileNode();
            node.Condition = ParseExpression();

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
    }
}