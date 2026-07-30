using System;
namespace SproutInterpreter
{
    public partial class Parser
    {
        private TryNode ParseTry()
        {
            Advance();
            var node = new TryNode();

            if (IsPunctuation("{"))
            {
                Advance();
                while (currentToken != null && currentToken.Value != "}")
                {
                    if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                    var stmt = ParseStatement();
                    if (stmt != null) node.TryBody.Add(stmt);
                    if (Check(Token.TokenType.Semicolon)) Advance();
                    while (Check(Token.TokenType.NewLine)) Advance();
                }
                Expect(Token.TokenType.Punctuation, "}");
            }
            else
            {
                var stmt = ParseStatement();
                if (stmt != null) node.TryBody.Add(stmt);
            }

            if (Check(Token.TokenType.Catch))
            {
                Advance();
                if (IsPunctuation("("))
                {
                    Advance();
                    if (IsIdentifier())
                    {
                        node.CatchVariable = currentToken.Value;
                        Advance();
                    }
                    Expect(Token.TokenType.Punctuation, ")");
                }

                if (IsPunctuation("{"))
                {
                    Advance();
                    while (currentToken != null && currentToken.Value != "}")
                    {
                        if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                        var stmt = ParseStatement();
                        if (stmt != null) node.CatchBody.Add(stmt);
                        if (Check(Token.TokenType.Semicolon)) Advance();
                        while (Check(Token.TokenType.NewLine)) Advance();
                    }
                    Expect(Token.TokenType.Punctuation, "}");
                }
                else
                {
                    var stmt = ParseStatement();
                    if (stmt != null) node.CatchBody.Add(stmt);
                }
            }

            return node;
        }
    }
}