namespace SproutInterpreter
{
    public class Token
    {
        public enum TokenType 
        { 
            Number, String, Identifier, Keyword, Operator, Punctuation, 
            Semicolon, NewLine, EOF, 
            IntKeyword, FloatKeyword, BoolKeyword, StrKeyword,
            Out, Input, If, Elif, Else, For, While, Repeat, Break,
            Function, Return, Send, Run, Import, At, 
            Global, Local, True, False, Null, 
            And, Or, Not, To, Times, Step,
            Try, Catch,
            Var
        }
        
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
        
        public Token(TokenType type, string value, int line = 0) 
        { 
            Type = type; 
            Value = value; 
            Line = line; 
        }
        
        public override string ToString() => $"{Type}: '{Value}' (строка {Line})";
    }
}