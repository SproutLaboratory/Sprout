using System;

namespace SproutInterpreter
{
    public partial class Parser
    {
        private ASTNode ParsePrimary()
        {
            Log($"ParsePrimary: текущий токен = {currentToken}");

            // ===== ПРИВЕДЕНИЕ ТИПОВ =====
            if (Check(Token.TokenType.IntKeyword) || 
                Check(Token.TokenType.FloatKeyword) || 
                Check(Token.TokenType.BoolKeyword) || 
                Check(Token.TokenType.StrKeyword))
            {
                string type = currentToken.Value;
                Advance();
                var operand = ParsePrimary();
                return new UnaryOpNode(type, operand);
            }
            
            // ===== ЛИТЕРАЛЫ =====
            if (IsNumber())
            {
                double value = double.Parse(currentToken.Value, System.Globalization.CultureInfo.InvariantCulture);
                Advance();
                return new NumberNode(value);
            }
            
            if (IsString())
            {
                string value = currentToken.Value;
                Advance();
                return new StringNode(value);
            }
            
            if (IsBool())
            {
                bool value = Check(Token.TokenType.True);
                Advance();
                return new BoolNode(value);
            }
            
            if (IsNull())
            {
                Advance();
                return new StringNode("null");
            }
            
            // ===== ТАБЛИЦА { ... } =====
            if (IsPunctuation("{"))
            {
                return ParseTable();
            }
            
            // ===== МАССИВ [ ... ] =====
            if (IsPunctuation("["))
            {
                return ParseArray();
            }
            
            // ===== СКОБКИ ( ... ) =====
            if (IsPunctuation("("))
            {
                Advance();
                var expr = ParseExpression();
                Expect(Token.TokenType.Punctuation, ")");
                return expr;
            }

            // ===== ИДЕНТИФИКАТОР =====
            if (IsIdentifier())
            {
                return ParseIdentifier();
            }

            // ===== INPUT =====
            if (Check(Token.TokenType.Input))
            {
                Log("INPUT");
                return ParseInput();
            }
            
            throw new Exception($"Неожиданный токен: {currentToken}");
        }

        private ASTNode ParseArray()
        {
            Log("Создание нового массива");
            Advance(); // пропускаем "["
            var node = new ArrayNode();
            while (currentToken != null && currentToken.Value != "]")
            {
                if (Check(Token.TokenType.Punctuation, ","))
                    Advance();
                else
                {
                    node.Elements.Add(ParseExpression());
                    if (Check(Token.TokenType.Punctuation, ","))
                        Advance();
                }
            }
            Expect(Token.TokenType.Punctuation, "]");
            return node;
        }

        private ASTNode ParseIdentifier()
        {
            string name = currentToken.Value;
            Advance();
            
            Log($"Идентификатор: {name}");

            // ===== ВЫЗОВ ФУНКЦИИ: func() =====
            if (currentToken != null && currentToken.Value == "(")
            {
                Log($"Вызов функции: {name}()");
                return ParseCallNode(name);
            }

            // ===== ИНДЕКСАЦИЯ: name[index] или ДОСТУП К ТАБЛИЦЕ: table.key или table[key] =====
            if (currentToken != null && (IsPunctuation("[") || IsPunctuation(".") || currentToken.Value == ":"))
            {
                // Парсим цепочку доступа
                var node = ParseTableAccess(name);
                
                // Проверяем, не является ли это присваиванием с индексацией
                if (IsOperator("="))
                {
                    Advance();
                    var value = ParseExpression();
                    
                    if (node is BinaryOpNode binOp && binOp.Operator == "index")
                    {
                        var setNode = new SetIndexNode(binOp.Left, binOp.Right);
                        setNode.Value = value;
                        return setNode;
                    }
                    if (node is SetIndexNode existingSetNode)
                    {
                        existingSetNode.Value = value;
                        return existingSetNode;
                    }
                    
                    throw new Exception($"Не удалось выполнить присваивание для индексации");
                }
                
                return node;
            }

            Log($"Переменная: {name}");
            return new VariableNode(name);
        }

        private ASTNode ParseIndexChain(string name)
        {
            Log($"Индексация: {name}[...]");
            ASTNode node = new VariableNode(name);

            while (currentToken != null && IsPunctuation("["))
            {
                Advance(); // пропускаем "["

                ASTNode index;
                if (Check(Token.TokenType.Var))
                {
                    Advance();
                    if (IsIdentifier())
                    {
                        string varName = currentToken.Value;
                        Advance();
                        index = new VariableNode(varName);
                        Log($"  Индекс: var {varName}");
                    }
                    else
                    {
                        throw new Exception("Ожидалось имя переменной после var");
                    }
                }
                else if (IsIdentifier())
                {
                    string varName = currentToken.Value;
                    Advance();
                    index = new VariableNode(varName);
                    Log($"  Индекс: {varName} (переменная)");
                }
                else if (IsString())
                {
                    string key = currentToken.Value;
                    Advance();
                    index = new StringNode(key);
                    Log($"  Индекс: \"{key}\" (ключ)");
                }
                else
                {
                    index = ParseExpression();
                    Log($"  Индекс: выражение");
                }

                if (currentToken == null || !IsPunctuation("]"))
                    throw new Exception($"Ожидался ], получено {currentToken}");
                
                Advance(); // пропускаем "]"
                node = new BinaryOpNode("index", node, index);
            }

            return node;
        }
    }
}