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
            Advance();
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

            // ===== ИНДЕКСАЦИЯ: name`index` =====
            if (currentToken != null && currentToken.Type == Token.TokenType.Backtick)
            {
                return ParseIndexChain(name);
            }

            // ===== ВЫЗОВ ФУНКЦИИ: func() =====
            if (currentToken != null && currentToken.Value == "(")
            {
                Log($"Вызов функции: {name}()");
                var call = ParseCallNode(name);
                
                if (currentToken != null && currentToken.Type == Token.TokenType.Backtick)
                {
                    var node = call;
                    while (currentToken != null && currentToken.Type == Token.TokenType.Backtick)
                    {
                        Advance();
                        
                        ASTNode index;
                        if (Check(Token.TokenType.Var))
                        {
                            Advance();
                            if (IsIdentifier())
                            {
                                string varName = currentToken.Value;
                                Advance();
                                index = new VariableNode(varName);
                            }
                            else
                            {
                                throw new Exception("Ожидалось имя переменной после var");
                            }
                        }
                        else
                        {
                            index = ParseExpression();
                        }
                        
                        if (currentToken == null || currentToken.Type != Token.TokenType.Backtick)
                            throw new Exception($"Ожидался `, получено {currentToken}");
                        
                        Advance();
                        node = new BinaryOpNode("index", node, index);
                    }
                    return node;
                }
                
                return call;
            }

            Log($"Переменная: {name}");
            return new VariableNode(name);
        }

        private ASTNode ParseIndexChain(string name)
        {
            Log($"Индексация: {name}`...`");
            ASTNode node = new VariableNode(name);

            while (currentToken != null && currentToken.Type == Token.TokenType.Backtick)
            {
                Advance(); // пропускаем первый бэктик

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
                    // Если это идентификатор и следующий токен не бэктик - это ключ словаря
                    string key = currentToken.Value;
                    Advance();
                    index = new StringNode(key);
                    Log($"  Индекс: {key} (ключ словаря)");
                }
                else
                {
                    index = ParseExpression();
                    Log($"  Индекс: выражение");
                }

                // Проверяем закрывающий бэктик
                if (currentToken == null || currentToken.Type != Token.TokenType.Backtick)
                {
                    // Если нет закрывающего бэктика, возвращаем то, что есть
                    // (это может быть конец индексации)
                    node = new BinaryOpNode("index", node, index);
                    return node;
                }

                Advance(); // пропускаем закрывающий бэктик
                node = new BinaryOpNode("index", node, index);
            }

            return node;
        }
    }
}