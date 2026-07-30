using System.Collections.Generic;

namespace SproutInterpreter
{
    public abstract class ASTNode { }
    
    public class NumberNode : ASTNode 
    { 
        public double Value { get; set; } 
        public NumberNode(double value) => Value = value; 
    }
    
    public class StringNode : ASTNode 
    { 
        public string Value { get; set; } 
        public StringNode(string value) => Value = value; 
    }
    
    public class BoolNode : ASTNode 
    { 
        public bool Value { get; set; } 
        public BoolNode(bool value) => Value = value; 
    }
    
    public class ArrayNode : ASTNode 
    { 
        public List<ASTNode> Elements { get; set; } = new List<ASTNode>(); 
    }
    
    public class DictNode : ASTNode 
    { 
        public Dictionary<string, ASTNode> Elements { get; set; } = new Dictionary<string, ASTNode>(); 
    }
    
    public class VariableNode : ASTNode 
    { 
        public string Name { get; set; } 
        public VariableNode(string name) => Name = name; 
    }
    
    public class AssignmentNode : ASTNode 
    { 
        public string Name { get; set; } 
        public ASTNode Value { get; set; } 
        public bool IsLocal { get; set; } 
        public AssignmentNode(string name, ASTNode value, bool isLocal = false) 
        { 
            Name = name; 
            Value = value; 
            IsLocal = isLocal; 
        } 
    }
    
    public class BinaryOpNode : ASTNode 
    { 
        public string Operator { get; set; } 
        public ASTNode Left { get; set; } 
        public ASTNode Right { get; set; } 
        public int Line { get; set; } 
        public BinaryOpNode(string op, ASTNode left, ASTNode right, int line = 0) 
        { 
            Operator = op; 
            Left = left; 
            Right = right; 
            Line = line; 
        } 
    }
    
    public class UnaryOpNode : ASTNode 
    { 
        public string Operator { get; set; } 
        public ASTNode Operand { get; set; } 
        public UnaryOpNode(string op, ASTNode operand) 
        { 
            Operator = op; 
            Operand = operand; 
        } 
    }
    
    public class SliceNode : ASTNode 
    { 
        public ASTNode Collection { get; set; } 
        public ASTNode Start { get; set; } 
        public ASTNode End { get; set; } 
        public ASTNode Step { get; set; } 
    }
    
    public class IfNode : ASTNode 
    { 
        public ASTNode Condition { get; set; } 
        public List<ASTNode> ThenBody { get; set; } = new List<ASTNode>(); 
        public List<ASTNode> ElseBody { get; set; } = new List<ASTNode>(); 
    }
    
    public class RepeatNode : ASTNode 
    { 
        public ASTNode Count { get; set; } 
        public string Variable { get; set; } 
        public List<ASTNode> Body { get; set; } = new List<ASTNode>(); 
    }
    
    public class ForNode : ASTNode 
    { 
        public string Variable { get; set; } 
        public ASTNode Start { get; set; } 
        public ASTNode End { get; set; } 
        public ASTNode Step { get; set; } 
        public List<ASTNode> Body { get; set; } = new List<ASTNode>(); 
    }
    
    public class WhileNode : ASTNode 
    { 
        public ASTNode Condition { get; set; } 
        public List<ASTNode> Body { get; set; } = new List<ASTNode>(); 
    }
    
    public class BreakNode : ASTNode { }
    
    public class OutNode : ASTNode 
    { 
        public ASTNode Expression { get; set; } 
    }
    
    public class InputNode : ASTNode 
    { 
        public ASTNode Prompt { get; set; } 
        public string AsType { get; set; } 
    }
    
    public class FunctionDefNode : ASTNode 
    { 
        public string Name { get; set; } 
        public List<string> Parameters { get; set; } = new List<string>(); 
        public string Scope { get; set; } = "local"; 
        public List<ASTNode> Body { get; set; } = new List<ASTNode>(); 
    }
    
    public class CallNode : ASTNode 
    { 
        public string Name { get; set; } 
        public List<ASTNode> Arguments { get; set; } = new List<ASTNode>(); 
    }
    
    public class ReturnSendNode : ASTNode 
    { 
        public ASTNode Value { get; set; } 
    }
    
    public class ReturnRunNode : ASTNode 
    { 
        public List<ASTNode> Body { get; set; } = new List<ASTNode>(); 
    }
    
    public class BlockNode : ASTNode 
    { 
        public List<ASTNode> Statements { get; set; } = new List<ASTNode>(); 
    }
    
    public class ImportNode : ASTNode
    {
        public bool IsPathSetter { get; set; }
        public string PathToSet { get; set; }
        public string LibraryName { get; set; }
    }
    
    public class GlobalNode : ASTNode 
    { 
        public string VariableName { get; set; } 
    }
    
    public class LocalNode : ASTNode 
    { 
        public string VariableName { get; set; } 
    }

    public class TryNode : ASTNode
    {
        public List<ASTNode> TryBody { get; set; } = new List<ASTNode>();
        public string CatchVariable { get; set; } = "e";
        public List<ASTNode> CatchBody { get; set; } = new List<ASTNode>();
    }

    public class SetIndexNode : ASTNode
    {
        public ASTNode Collection { get; set; }
        public ASTNode Index { get; set; }
        public ASTNode Value { get; set; }
        
        public SetIndexNode(ASTNode collection, ASTNode index, ASTNode value = null)
        {
            Collection = collection;
            Index = index;
            Value = value;
        }
    }
}