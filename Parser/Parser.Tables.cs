using System;
using System.Collections.Generic;

namespace SproutInterpreter
{
    public partial class Parser
    {
        // ===== ПАРСИНГ ТАБЛИЦЫ: { key = value, key2 = value2, ... } =====
        private ASTNode ParseTable()
        {
            Log("Создание новой таблицы");
            Advance(); // пропускаем "{"
            
            var node = new DictNode();
            var arrayIndex = 1;
            
            while (currentToken != null && currentToken.Value != "}")
            {
                if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                if (Check(Token.TokenType.Punctuation, ",")) 
                { 
                    Advance(); 
                    continue; 
                }
                
                // ===== ПРОВЕРЯЕМ НА КЛЮЧ-ЗНАЧЕНИЕ =====
                // Случай 1: [ключ] = значение (вычисляемый ключ)
                if (IsPunctuation("["))
                {
                    Advance(); // пропускаем "["
                    var keyExpr = ParseExpression();
                    Expect(Token.TokenType.Punctuation, "]");
                    Expect(Token.TokenType.Operator, "=");
                    var valueExpr = ParseExpression();
                    node.Elements[$"[{keyExpr}]"] = valueExpr;
                }
                // Случай 2: ключ = значение (идентификатор как ключ)
                else if (IsIdentifier() && PeekNext() is Token next && next.Value == "=")
                {
                    string key = currentToken.Value;
                    Advance(); // пропускаем ключ
                    Advance(); // пропускаем "="
                    var value = ParseExpression();
                    node.Elements[key] = value;
                }
                // Случай 3: значение (массивная часть)
                else
                {
                    var value = ParseExpression();
                    node.Elements[arrayIndex.ToString()] = value;
                    arrayIndex++;
                }
                
                if (Check(Token.TokenType.Punctuation, ","))
                    Advance();
                while (Check(Token.TokenType.NewLine)) Advance();
            }
            
            Expect(Token.TokenType.Punctuation, "}");
            return node;
        }
        
        // ===== ПАРСИНГ ВЫЗОВА МЕТОДА ТАБЛИЦЫ: table:method(args) =====
        private CallNode ParseTableMethodCall(string tableName)
        {
            Log($"Вызов метода таблицы: {tableName}:method()");
            Advance(); // пропускаем ":"
            
            if (!IsIdentifier())
                throw new Exception("Ожидалось имя метода после :");
            
            string methodName = currentToken.Value;
            Advance();
            
            var call = new CallNode { Name = tableName };
            call.Arguments.Add(new StringNode(methodName));
            
            Expect(Token.TokenType.Punctuation, "(");
            while (currentToken != null && currentToken.Value != ")")
            {
                call.Arguments.Add(ParseExpression());
                if (Check(Token.TokenType.Punctuation, ",")) Advance();
            }
            Expect(Token.TokenType.Punctuation, ")");
            
            return call;
        }
        
        // ===== ПАРСИНГ ДОСТУПА К ТАБЛИЦЕ: table[key] или table.key =====
        private ASTNode ParseTableAccess(string name)
        {
            Log($"Доступ к таблице: {name}");
            
            // table.key
            if (IsPunctuation("."))
            {
                Advance();
                if (IsIdentifier())
                {
                    string key = currentToken.Value;
                    Advance();
                    return new BinaryOpNode("index", new VariableNode(name), new StringNode(key));
                }
                throw new Exception("Ожидалось имя поля после .");
            }
            
            // table[key]
            if (IsPunctuation("["))
            {
                Advance();
                var index = ParseExpression();
                Expect(Token.TokenType.Punctuation, "]");
                return new BinaryOpNode("index", new VariableNode(name), index);
            }
            
            // table:method() - вызов метода
            if (currentToken != null && currentToken.Value == ":")
            {
                return ParseTableMethodCall(name);
            }
            
            return new VariableNode(name);
        }
        
        // ===== ПРОВЕРКА СЛЕДУЮЩЕГО ТОКЕНА =====
        private Token PeekNext()
        {
            if (pos + 1 < tokens.Count)
                return tokens[pos + 1];
            return null;
        }
    }
}