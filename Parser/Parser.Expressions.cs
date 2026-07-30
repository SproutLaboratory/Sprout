using System;
namespace SproutInterpreter
{
    public partial class Parser
    {
        private ASTNode ParseExpression() => ParseAssignment();

        private ASTNode ParseAssignment()
        {
            var node = ParseOr();
            
            if (enableLogging)
            {
                Log($"ParseAssignment: node = {node?.GetType().Name ?? "null"}");
                if (node is BinaryOpNode binOp)
                    Log($"  Operator: {binOp.Operator}");
                if (node is VariableNode varNode)
                    Log($"  Variable: {varNode.Name}");
                if (node is SetIndexNode setNode)
                    Log($"  SetIndexNode: {setNode.Collection} [{setNode.Index}]");
            }
            
            if (IsOperator("="))
            {
                Advance();
                var value = ParseExpression();
                
                if (node is VariableNode varNode)
                    return new AssignmentNode(varNode.Name, value);
                
                if (node is SetIndexNode setIndexNode)
                {
                    setIndexNode.Value = value;
                    return setIndexNode;
                }
                
                throw new Exception($"Левая часть присваивания должна быть переменной. Получено: {node?.GetType().Name ?? "null"}");
            }
            
            return node;
        }

        private ASTNode ParseOr()
        {
            var node = ParseAnd();
            while (Check(Token.TokenType.Or))
            {
                Advance();
                node = new BinaryOpNode("or", node, ParseAnd());
            }
            return node;
        }

        private ASTNode ParseAnd()
        {
            var node = ParseComparison();
            while (Check(Token.TokenType.And))
            {
                Advance();
                node = new BinaryOpNode("and", node, ParseComparison());
            }
            return node;
        }

        private ASTNode ParseComparison()
        {
            var node = ParseAddSub();
            while (IsOperator("==") || IsOperator("!=") ||
                   IsOperator(">") || IsOperator("<") ||
                   IsOperator(">=") || IsOperator("<=") ||
                   IsOperator("?=") || IsOperator("??"))
            {
                string op = currentToken.Value;
                int line = currentToken.Line;
                Advance();
                node = new BinaryOpNode(op, node, ParseAddSub(), line);
            }
            return node;
        }

        private ASTNode ParseAddSub()
        {
            var node = ParseMulDiv();
            while (IsOperator("+") || IsOperator("-"))
            {
                string op = currentToken.Value;
                int line = currentToken.Line;
                Advance();
                node = new BinaryOpNode(op, node, ParseMulDiv(), line);
            }
            return node;
        }

        private ASTNode ParseMulDiv()
        {
            var node = ParsePower();
            while (IsOperator("*") || IsOperator("/") || IsOperator("%"))
            {
                string op = currentToken.Value;
                int line = currentToken.Line;
                Advance();
                node = new BinaryOpNode(op, node, ParsePower(), line);
            }
            return node;
        }

        private ASTNode ParsePower()
        {
            var node = ParseUnary();
            while (IsOperator("**"))
            {
                string op = currentToken.Value;
                int line = currentToken.Line;
                Advance();
                node = new BinaryOpNode(op, node, ParseUnary(), line);
            }
            return node;
        }

        private ASTNode ParseUnary()
        {
            if (Check(Token.TokenType.Not))
            {
                Advance();
                return new BinaryOpNode("not", null, ParseUnary());
            }

            if (IsOperator("-"))
            {
                Advance();
                if (IsNumber())
                {
                    double value = double.Parse(currentToken.Value, System.Globalization.CultureInfo.InvariantCulture);
                    Advance();
                    return new NumberNode(-value);
                }
                return new BinaryOpNode("negate", null, ParseUnary());
            }

            return ParsePrimary();
        }
    }
}