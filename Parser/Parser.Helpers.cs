using System;

namespace SproutInterpreter
{
    public partial class Parser
    {
        private void Advance() 
        { 
            pos++; 
            currentToken = pos < tokens.Count ? tokens[pos] : null; 
        }

        private bool Check(Token.TokenType type, string value = null)
        {
            if (currentToken == null) return false;
            if (currentToken.Type != type) return false;
            if (value != null && currentToken.Value != value) return false;
            return true;
        }

        private void Expect(Token.TokenType type, string value = null)
        {
            if (!Check(type, value))
                throw new Exception($"Ожидалось {type} '{value}', получено {currentToken}");
            Advance();
        }

        private void Log(string message)
        {
            if (enableLogging)
                Console.WriteLine($"  📌 {message}");
        }

        private bool IsIdentifier() => Check(Token.TokenType.Identifier);
        private bool IsNumber() => Check(Token.TokenType.Number);
        private bool IsString() => Check(Token.TokenType.String);
        private bool IsBool() => Check(Token.TokenType.True) || Check(Token.TokenType.False);
        private bool IsNull() => Check(Token.TokenType.Null);
        private bool IsPunctuation(string value) => Check(Token.TokenType.Punctuation, value);
        private bool IsOperator(string value) => Check(Token.TokenType.Operator, value);
    }
}