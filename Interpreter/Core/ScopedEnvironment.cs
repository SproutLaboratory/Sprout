using System;
using System.Collections.Generic;

namespace SproutInterpreter
{
    public class ScopedEnvironment
    {
        private Dictionary<string, SproutValue> variables = new Dictionary<string, SproutValue>();
        private Dictionary<string, object> functions = new Dictionary<string, object>();
        private ScopedEnvironment parent;

        public ScopedEnvironment Parent => parent;

        public ScopedEnvironment(ScopedEnvironment parent = null)
        {
            this.parent = parent;
        }

        public void SetVariable(string name, SproutValue value) 
        { 
            variables[name] = value; 
        }
        
        public SproutValue GetVariable(string name)
        {
            if (variables.ContainsKey(name)) return variables[name];
            if (parent != null) return parent.GetVariable(name);
            throw new Exception($"Переменная '{name}' не определена");
        }
        public void SetFunction(string name, Function func) { functions[name] = func; }
        public void SetCSharpFunction(string name, CSharpFunction func) { functions[name] = func; }
        public object GetFunction(string name)
        {
            if (functions.ContainsKey(name)) return functions[name];
            if (parent != null) return parent.GetFunction(name);
            throw new Exception($"Функция '{name}' не определена");
        }
        public bool HasVariable(string name) 
        { 
            return variables.ContainsKey(name) || (parent != null && parent.HasVariable(name)); 
        }
        public bool HasFunction(string name) 
        { 
            return functions.ContainsKey(name) || (parent != null && parent.HasFunction(name)); 
        }
        public ScopedEnvironment CreateChild() => new ScopedEnvironment(this);
        public void Merge(ScopedEnvironment other)
        {
            foreach (var kv in other.variables) variables[kv.Key] = kv.Value;
            foreach (var kv in other.functions) functions[kv.Key] = kv.Value;
        }
        public IEnumerable<KeyValuePair<string, SproutValue>> GetVariables() => variables;
        public IEnumerable<KeyValuePair<string, object>> GetFunctions() => functions;
    }
}