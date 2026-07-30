using System;
using System.Collections.Generic;

namespace SproutInterpreter
{
    public class Lexer
    {
        private string text;
        private int pos;
        private char currentChar;

        public Lexer(string text)
        {
            this.text = text;
            pos = 0;
            currentChar = text.Length > 0 ? text[0] : '\0';
        }

        private void Advance() { pos++; currentChar = pos < text.Length ? text[pos] : '\0'; }
        
        private void SkipWhitespace() 
        { 
            while (char.IsWhiteSpace(currentChar) && currentChar != '\n') 
                Advance(); 
        }

        private void SkipComment()
        {
            if (currentChar == '#' && pos + 2 < text.Length && 
                text[pos] == '#' && text[pos + 1] == '#' && text[pos + 2] == '#')
            {
                Advance(); Advance(); Advance();
                while (currentChar != '\0')
                {
                    if (currentChar == '#' && pos + 2 < text.Length && 
                        text[pos] == '#' && text[pos + 1] == '#' && text[pos + 2] == '#')
                    { 
                        Advance(); Advance(); Advance(); 
                        break; 
                    }
                    Advance();
                }
                return;
            }
            if (currentChar == '#') 
            { 
                while (currentChar != '\0' && currentChar != '\n') 
                    Advance(); 
            }
        }

        private string ReadNumber()
        {
            string result = "";
            bool hasDot = false;
            while (char.IsDigit(currentChar) || currentChar == '.')
            {
                if (currentChar == '.') 
                { 
                    if (hasDot) break; 
                    hasDot = true; 
                }
                result += currentChar;
                Advance();
            }
            return result;
        }
        
        private string ReadIdentifier()
        {
            string result = "";
            while (char.IsLetterOrDigit(currentChar) || currentChar == '_')
            {
                result += currentChar;
                Advance();
            }
            return result;
        }

        private string ReadString()
        {
            char quote = currentChar;
            Advance();
            string result = "";
            while (currentChar != '\0' && currentChar != quote)
            {
                if (currentChar == '\\') 
                { 
                    Advance(); 
                    result += currentChar switch 
                    { 
                        'n' => '\n', 
                        't' => '\t', 
                        '\\' => '\\', 
                        '"' => '"', 
                        '\'' => '\'', 
                        '`' => '`',
                        _ => currentChar 
                    };
                }
                else 
                    result += currentChar;
                Advance();
            }
            if (currentChar == quote) Advance();
            return result;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();
            int line = 1;

            while (currentChar != '\0')
            {
                SkipWhitespace();
                SkipComment();
                SkipWhitespace();

                if (currentChar == '\0') break;

                if (currentChar == '\n' || currentChar == '\r')
                {
                    if (currentChar == '\r') 
                    { 
                        Advance(); 
                        if (currentChar == '\n') Advance(); 
                    }
                    else Advance();
                    line++;
                    tokens.Add(new Token(Token.TokenType.NewLine, "\n", line));
                    continue;
                }

                if (char.IsDigit(currentChar))
                {
                    tokens.Add(new Token(Token.TokenType.Number, ReadNumber(), line));
                    continue;
                }

                if (char.IsLetter(currentChar) || currentChar == '_')
                {
                    string id = ReadIdentifier();
                    
                    switch (id)
                    {
                        case "out": tokens.Add(new Token(Token.TokenType.Out, id, line)); continue;
                        case "input": tokens.Add(new Token(Token.TokenType.Input, id, line)); continue;
                        case "if": tokens.Add(new Token(Token.TokenType.If, id, line)); continue;
                        case "elif": tokens.Add(new Token(Token.TokenType.Elif, id, line)); continue;
                        case "else": tokens.Add(new Token(Token.TokenType.Else, id, line)); continue;
                        case "for": tokens.Add(new Token(Token.TokenType.For, id, line)); continue;
                        case "while": tokens.Add(new Token(Token.TokenType.While, id, line)); continue;
                        case "repeat": tokens.Add(new Token(Token.TokenType.Repeat, id, line)); continue;
                        case "break": tokens.Add(new Token(Token.TokenType.Break, id, line)); continue;
                        case "function": tokens.Add(new Token(Token.TokenType.Function, id, line)); continue;
                        case "return": tokens.Add(new Token(Token.TokenType.Return, id, line)); continue;
                        case "send": tokens.Add(new Token(Token.TokenType.Send, id, line)); continue;
                        case "run": tokens.Add(new Token(Token.TokenType.Run, id, line)); continue;
                        case "import": tokens.Add(new Token(Token.TokenType.Import, id, line)); continue;
                        case "at": tokens.Add(new Token(Token.TokenType.At, id, line)); continue;
                        case "global": tokens.Add(new Token(Token.TokenType.Global, id, line)); continue;
                        case "local": tokens.Add(new Token(Token.TokenType.Local, id, line)); continue;
                        case "true": tokens.Add(new Token(Token.TokenType.True, id, line)); continue;
                        case "false": tokens.Add(new Token(Token.TokenType.False, id, line)); continue;
                        case "null": tokens.Add(new Token(Token.TokenType.Null, id, line)); continue;
                        case "and": tokens.Add(new Token(Token.TokenType.And, id, line)); continue;
                        case "or": tokens.Add(new Token(Token.TokenType.Or, id, line)); continue;
                        case "not": tokens.Add(new Token(Token.TokenType.Not, id, line)); continue;
                        case "to": tokens.Add(new Token(Token.TokenType.To, id, line)); continue;
                        case "times": tokens.Add(new Token(Token.TokenType.Times, id, line)); continue;
                        case "step": tokens.Add(new Token(Token.TokenType.Step, id, line)); continue;
                        case "int": tokens.Add(new Token(Token.TokenType.IntKeyword, id, line)); continue;
                        case "float": tokens.Add(new Token(Token.TokenType.FloatKeyword, id, line)); continue;
                        case "bool": tokens.Add(new Token(Token.TokenType.BoolKeyword, id, line)); continue;
                        case "str": tokens.Add(new Token(Token.TokenType.StrKeyword, id, line)); continue;
                        case "try": tokens.Add(new Token(Token.TokenType.Try, id, line)); continue;
                        case "catch": tokens.Add(new Token(Token.TokenType.Catch, id, line)); continue;
                        case "var": tokens.Add(new Token(Token.TokenType.Var, id, line)); continue;
                    }
                    
                    tokens.Add(new Token(Token.TokenType.Identifier, id, line));
                    continue;
                }

                if (currentChar == '"' || currentChar == '\'')
                {
                    tokens.Add(new Token(Token.TokenType.String, ReadString(), line));
                    continue;
                }

                if (currentChar == ';')
                {
                    Advance();
                    tokens.Add(new Token(Token.TokenType.Semicolon, ";", line));
                    continue;
                }

                if (currentChar == '.')
                {
                    Advance();
                    tokens.Add(new Token(Token.TokenType.Punctuation, ".", line));
                    continue;
                }

                if ("+-*/%<>=!&|?".Contains(currentChar))
                {
                    string op = currentChar.ToString();
                    Advance();
                    
                    if (currentChar == '=' && (op == "=" || op == "!" || op == ">" || op == "<" || op == "?"))
                    {
                        op += currentChar;
                        Advance();
                    }
                    else if (currentChar == '?' && op == "?")
                    {
                        op += currentChar;
                        Advance();
                    }
                    else if (currentChar == '*' && op == "*")
                    {
                        op += currentChar;
                        Advance();
                    }
                    
                    tokens.Add(new Token(Token.TokenType.Operator, op, line));
                    continue;
                }

                if (currentChar == '`')
                {
                    Advance();
                    tokens.Add(new Token(Token.TokenType.Backtick, "`", line));
                    continue;
                }

                if ("{}[](),:".Contains(currentChar))
                {
                    tokens.Add(new Token(Token.TokenType.Punctuation, currentChar.ToString(), line));
                    Advance();
                    continue;
                }

                throw new Exception($"Неожиданный символ: '{currentChar}' на строке {line}");
            }

            tokens.Add(new Token(Token.TokenType.EOF, "", line));
            return tokens;
        }
    }
}