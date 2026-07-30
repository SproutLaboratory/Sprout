using System;
using System.Collections.Generic;
using System.Linq;

namespace SproutInterpreter
{
    public class SproutValue
    {
        public enum ValueType { Null, Number, String, Bool, Array, Dict, Function, CSharpFunction, Table }
        public ValueType Type { get; private set; }
        public object Value { get; private set; }

        public SproutValue() { Type = ValueType.Null; Value = null; }

        public SproutValue(object value)
        {
            if (value == null) { Type = ValueType.Null; Value = null; return; }
            if (value is double || value is int || value is float || value is decimal)
            { Type = ValueType.Number; Value = Convert.ToDouble(value); return; }
            if (value is string) { Type = ValueType.String; Value = value; return; }
            if (value is bool) { Type = ValueType.Bool; Value = value; return; }
            if (value is List<SproutValue>) { Type = ValueType.Array; Value = value; return; }
            if (value is Dictionary<string, SproutValue>) { Type = ValueType.Dict; Value = value; return; }
            if (value is Function) { Type = ValueType.Function; Value = value; return; }
            if (value is CSharpFunction) { Type = ValueType.CSharpFunction; Value = value; return; }
            if (value is Table) { Type = ValueType.Table; Value = value; return; }
            throw new Exception($"Неизвестный тип: {value.GetType()}");
        }

        public SproutValue(double num) : this((object)num) { }
        public SproutValue(string str) : this((object)str) { }
        public SproutValue(bool b) : this((object)b) { }

        public double AsNumber()
        {
            if (Type != ValueType.Number) throw new Exception($"Ожидалось число, получено {Type}");
            return (double)Value;
        }
        public string AsString() => Type == ValueType.String ? (string)Value : ToString();
        public bool AsBool()
        {
            if (Type == ValueType.Bool) return (bool)Value;
            if (Type == ValueType.Null) return false;
            if (Type == ValueType.Number) return (double)Value != 0;
            return true;
        }
        public List<SproutValue> AsArray()
        {
            if (Type != ValueType.Array) throw new Exception($"Ожидался массив, получено {Type}");
            return (List<SproutValue>)Value;
        }
        public Dictionary<string, SproutValue> AsDict()
        {
            if (Type != ValueType.Dict) throw new Exception($"Ожидался словарь, получено {Type}");
            return (Dictionary<string, SproutValue>)Value;
        }
        public Function AsFunction()
        {
            if (Type != ValueType.Function) throw new Exception($"Ожидалась функция, получено {Type}");
            return (Function)Value;
        }
        public CSharpFunction AsCSharpFunction()
        {
            if (Type != ValueType.CSharpFunction) throw new Exception($"Ожидалась C# функция, получено {Type}");
            return (CSharpFunction)Value;
        }
        public Table AsTable()
        {
            if (Type != ValueType.Table) throw new Exception($"Ожидалась таблица, получено {Type}");
            return (Table)Value;
        }

        public override string ToString()
        {
            return Type switch
            {
                ValueType.Null => "null",
                ValueType.Number => Value.ToString(),
                ValueType.String => (string)Value,
                ValueType.Bool => (bool)Value ? "true" : "false",
                ValueType.Array => "[" + string.Join(", ", ((List<SproutValue>)Value).Select(v => v.ToString())) + "]",
                ValueType.Dict => "{" + string.Join(", ", ((Dictionary<string, SproutValue>)Value).Select(kv => kv.Key + ": " + kv.Value)) + "}",
                ValueType.Function => "<function " + ((Function)Value).Name + ">",
                ValueType.CSharpFunction => "<C# function>",
                ValueType.Table => "<table>",
                _ => "unknown"
            };
        }
    }
}