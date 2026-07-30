using System;
namespace SproutInterpreter
{
    public partial class Parser
    {
        private GlobalNode ParseGlobal()
        {
            Advance();
            if (IsPunctuation("("))
            {
                Advance();
                string varName = currentToken.Value;
                Advance();
                Expect(Token.TokenType.Punctuation, ")");
                return new GlobalNode { VariableName = varName };
            }
            throw new Exception("Ожидалось global(имя)");
        }

        private LocalNode ParseLocal()
        {
            Advance();
            if (IsPunctuation("("))
            {
                Advance();
                string varName = currentToken.Value;
                Advance();
                Expect(Token.TokenType.Punctuation, ")");
                return new LocalNode { VariableName = varName };
            }
            throw new Exception("Ожидалось local(имя)");
        }
    }
}