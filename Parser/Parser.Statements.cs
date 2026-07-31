using System;
using System.Collections.Generic;

namespace SproutInterpreter
{
    public partial class Parser
    {
        private ASTNode ParseStatement()
        {
            if (currentToken == null) return null;

            // ===== ОПЕРАТОРЫ =====
            if (Check(Token.TokenType.Out)) return ParseOut();
            if (Check(Token.TokenType.Input)) return ParseInput();
            if (Check(Token.TokenType.Function)) return ParseFunctionDef();
            if (Check(Token.TokenType.Return)) return ParseReturn();
            if (Check(Token.TokenType.If)) return ParseIf();
            if (Check(Token.TokenType.While)) return ParseWhile();
            if (Check(Token.TokenType.For)) return ParseFor();
            if (Check(Token.TokenType.Repeat)) return ParseRepeat();
            if (Check(Token.TokenType.Break))
            {
                Advance();
                return new BreakNode();
            }
            if (Check(Token.TokenType.Import)) return ParseImport();
            if (Check(Token.TokenType.Global)) return ParseGlobal();
            if (Check(Token.TokenType.Local)) return ParseLocal();
            if (Check(Token.TokenType.Try)) return ParseTry();

            // ===== БЛОК =====
            if (IsPunctuation("{"))
            {
                return ParseBlock();
            }

            // ВСЕ выражения (включая присваивания, индексацию, вызовы функций) идут через ParseExpression
            return ParseExpression();
        }

        private OutNode ParseOut()
        {
            Advance();
            var node = new OutNode();
            node.Expression = ParseExpression();
            if (Check(Token.TokenType.Semicolon)) Advance();
            return node;
        }

        private InputNode ParseInput()
        {
            Advance();
            var node = new InputNode();
            if (IsPunctuation("("))
            {
                Expect(Token.TokenType.Punctuation, "(");
                node.Prompt = ParseExpression();
                Expect(Token.TokenType.Punctuation, ")");
            }
            else
            {
                node.Prompt = ParseExpression();
            }
            if (Check(Token.TokenType.StrKeyword) || Check(Token.TokenType.IntKeyword) || 
                Check(Token.TokenType.FloatKeyword) || Check(Token.TokenType.BoolKeyword))
            {
                node.AsType = currentToken.Value;
                Advance();
            }
            return node;
        }

        private ImportNode ParseImport()
        {
            Advance();
            var node = new ImportNode();

            if (Check(Token.TokenType.At))
            {
                Advance();
                node.IsPathSetter = true;
                if (Check(Token.TokenType.String))
                {
                    node.PathToSet = currentToken.Value;
                    Advance();
                }
                else
                {
                    throw new Exception("Ожидался строковый путь после import at");
                }
                return node;
            }

            if (IsIdentifier() || IsString())
            {
                node.LibraryName = currentToken.Value;
                Advance();
                return node;
            }

            throw new Exception("Ожидалось имя библиотеки после import");
        }

        private BlockNode ParseBlock()
        {
            Advance();
            var block = new List<ASTNode>();
            while (currentToken != null && currentToken.Value != "}")
            {
                if (Check(Token.TokenType.NewLine)) { Advance(); continue; }
                var stmt = ParseStatement();
                if (stmt != null) block.Add(stmt);
                if (Check(Token.TokenType.Semicolon)) Advance();
                while (Check(Token.TokenType.NewLine)) Advance();
            }
            Expect(Token.TokenType.Punctuation, "}");
            return new BlockNode { Statements = block };
        }

        private ASTNode ParseCallNode(string name)
        {
            var call = new CallNode { Name = name };
            Expect(Token.TokenType.Punctuation, "(");
            while (currentToken != null && currentToken.Value != ")")
            {
                call.Arguments.Add(ParseExpression());
                if (Check(Token.TokenType.Punctuation, ",")) Advance();
            }
            Expect(Token.TokenType.Punctuation, ")");

            // После вызова функции может быть индексация: func()[index]
            if (currentToken != null && IsPunctuation("["))
            {
                ASTNode node = call;
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

                    if (currentToken == null || !IsPunctuation("]"))
                        throw new Exception($"Ожидался ], получено {currentToken}");

                    Advance(); // пропускаем "]"
                    node = new BinaryOpNode("index", node, index);
                }
                return node;
            }

            return call;
        }
    }
}