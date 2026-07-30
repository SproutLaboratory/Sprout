using System;
using System.Collections.Generic;

namespace SproutInterpreter
{
    public class Function
    {
        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public List<ASTNode> Body { get; set; }
        public ScopedEnvironment Closure { get; set; }
        public string Scope { get; set; } = "local";
        public Function(string name, List<string> parameters, List<ASTNode> body, ScopedEnvironment closure)
        { Name = name; Parameters = parameters; Body = body; Closure = closure; }
    }
}