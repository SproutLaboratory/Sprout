using System;
using System.Collections.Generic;
using System.Linq;

namespace SproutInterpreter
{
    public partial class Interpreter
    {
        private void LoadStandardLibrary()
        {
            Log("Загрузка стандартной библиотеки...");
            
            // ===== БАЗОВЫЕ =====
            AddCSharpFunction("len", args =>
            {
                if (args[0].Type == SproutValue.ValueType.Array)
                    return new SproutValue(args[0].AsArray().Count);
                if (args[0].Type == SproutValue.ValueType.String)
                    return new SproutValue(args[0].AsString().Length);
                if (args[0].Type == SproutValue.ValueType.Dict)
                    return new SproutValue(args[0].AsDict().Count);
                throw new Exception($"len не поддерживается для типа {args[0].Type}");
            });

            // ===== СТРОКОВЫЕ =====
            AddCSharpFunction("upper", args => new SproutValue(args[0].AsString().ToUpper()));
            AddCSharpFunction("lower", args => new SproutValue(args[0].AsString().ToLower()));
            AddCSharpFunction("capitalize", args =>
            {
                string s = args[0].AsString();
                if (string.IsNullOrEmpty(s)) return new SproutValue(s);
                return new SproutValue(char.ToUpper(s[0]) + s.Substring(1).ToLower());
            });
            AddCSharpFunction("title", args =>
            {
                string s = args[0].AsString();
                var words = s.Split(' ');
                for (int i = 0; i < words.Length; i++)
                    if (!string.IsNullOrEmpty(words[i]))
                        words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                return new SproutValue(string.Join(" ", words));
            });
            AddCSharpFunction("strip", args => new SproutValue(args[0].AsString().Trim()));
            AddCSharpFunction("lstrip", args => new SproutValue(args[0].AsString().TrimStart()));
            AddCSharpFunction("rstrip", args => new SproutValue(args[0].AsString().TrimEnd()));
            AddCSharpFunction("replace", args =>
            {
                string s = args[0].AsString();
                string oldStr = args[1].AsString();
                string newStr = args[2].AsString();
                return new SproutValue(s.Replace(oldStr, newStr));
            });
            AddCSharpFunction("split", args =>
            {
                string s = args[0].AsString();
                string separator = args.Count > 1 ? args[1].AsString() : " ";
                var parts = s.Split(new[] { separator }, StringSplitOptions.None);
                var list = new List<SproutValue>();
                foreach (var part in parts)
                    list.Add(new SproutValue(part));
                return new SproutValue(list);
            });
            AddCSharpFunction("join", args =>
            {
                string separator = args[0].AsString();
                var arr = args[1].AsArray();
                var strings = arr.Select(v => v.ToString()).ToArray();
                return new SproutValue(string.Join(separator, strings));
            });
            AddCSharpFunction("contains", args =>
            {
                string s = args[0].AsString();
                string sub = args[1].AsString();
                return new SproutValue(s.Contains(sub));
            });
            AddCSharpFunction("find", args =>
            {
                string s = args[0].AsString();
                string sub = args[1].AsString();
                int start = args.Count > 2 ? (int)args[2].AsNumber() : 0;
                int index = s.IndexOf(sub, start);
                return new SproutValue(index);
            });
            AddCSharpFunction("count", args =>
            {
                if (args[0].Type == SproutValue.ValueType.String)
                {
                    string s = args[0].AsString();
                    string sub = args[1].AsString();
                    int count = 0;
                    int pos = 0;
                    while ((pos = s.IndexOf(sub, pos)) != -1)
                    {
                        count++;
                        pos += sub.Length;
                    }
                    return new SproutValue(count);
                }
                else if (args[0].Type == SproutValue.ValueType.Array)
                {
                    var arr = args[0].AsArray();
                    string target = args[1].ToString();
                    int count = 0;
                    foreach (var item in arr)
                        if (item.ToString() == target)
                            count++;
                    return new SproutValue(count);
                }
                throw new Exception($"count не поддерживается для типа {args[0].Type}");
            });
            AddCSharpFunction("isdigit", args =>
            {
                string s = args[0].AsString();
                return new SproutValue(!string.IsNullOrEmpty(s) && s.All(char.IsDigit));
            });
            AddCSharpFunction("isalpha", args =>
            {
                string s = args[0].AsString();
                return new SproutValue(!string.IsNullOrEmpty(s) && s.All(char.IsLetter));
            });
            AddCSharpFunction("isalnum", args =>
            {
                string s = args[0].AsString();
                return new SproutValue(!string.IsNullOrEmpty(s) && s.All(char.IsLetterOrDigit));
            });
            AddCSharpFunction("isspace", args =>
            {
                string s = args[0].AsString();
                return new SproutValue(!string.IsNullOrEmpty(s) && s.All(char.IsWhiteSpace));
            });
            AddCSharpFunction("islower", args =>
            {
                string s = args[0].AsString();
                return new SproutValue(s == s.ToLower());
            });
            AddCSharpFunction("isupper", args =>
            {
                string s = args[0].AsString();
                return new SproutValue(s == s.ToUpper());
            });
            AddCSharpFunction("to_string", args => new SproutValue(args[0].ToString()));

            // ===== МАССИВЫ =====
            AddCSharpFunction("min", args =>
            {
                var arr = args[0].AsArray();
                if (arr.Count == 0) return new SproutValue(0);
                double min = arr[0].AsNumber();
                foreach (var item in arr)
                    if (item.AsNumber() < min) min = item.AsNumber();
                return new SproutValue(min);
            });
            AddCSharpFunction("max", args =>
            {
                var arr = args[0].AsArray();
                if (arr.Count == 0) return new SproutValue(0);
                double max = arr[0].AsNumber();
                foreach (var item in arr)
                    if (item.AsNumber() > max) max = item.AsNumber();
                return new SproutValue(max);
            });
            AddCSharpFunction("sum_arr", args =>
            {
                var arr = args[0].AsArray();
                double sum = 0;
                foreach (var item in arr)
                    sum += item.AsNumber();
                return new SproutValue(sum);
            });
            AddCSharpFunction("append", args => { 
                var arr = args[0].AsArray(); 
                arr.Add(args[1]); 
                return new SproutValue(arr); 
            });
            AddCSharpFunction("insert", args => { 
                var arr = args[0].AsArray(); 
                arr.Insert((int)args[1].AsNumber(), args[2]); 
                return new SproutValue(arr); 
            });
            AddCSharpFunction("pop", args =>
            {
                var arr = args[0].AsArray();
                if (arr.Count == 0) throw new Exception("pop from empty array");
                int index = args.Count > 1 ? (int)args[1].AsNumber() : arr.Count - 1;
                if (index < 0) index = arr.Count + index;
                var result = arr[index];
                arr.RemoveAt(index);
                return result;
            });
            AddCSharpFunction("remove", args => { 
                var arr = args[0].AsArray(); 
                arr.RemoveAll(x => x.ToString() == args[1].ToString()); 
                return new SproutValue(arr); 
            });
            AddCSharpFunction("find_index", args =>
            {
                var arr = args[0].AsArray();
                string target = args[1].ToString();
                for (int i = 0; i < arr.Count; i++)
                    if (arr[i].ToString() == target)
                        return new SproutValue(i);
                return new SproutValue(-1);
            });
            AddCSharpFunction("sort", args =>
            {
                var arr = args[0].AsArray();
                arr.Sort((a, b) => a.ToString().CompareTo(b.ToString()));
                return new SproutValue(arr);
            });
            AddCSharpFunction("reverse", args =>
            {
                var arr = args[0].AsArray();
                arr.Reverse();
                return new SproutValue(arr);
            });
            AddCSharpFunction("clear", args =>
            {
                if (args[0].Type == SproutValue.ValueType.Array)
                {
                    args[0].AsArray().Clear();
                    return new SproutValue(args[0].AsArray());
                }
                else if (args[0].Type == SproutValue.ValueType.Dict)
                {
                    args[0].AsDict().Clear();
                    return new SproutValue(args[0].AsDict());
                }
                throw new Exception($"clear не поддерживается для типа {args[0].Type}");
            });

            // ===== СЛОВАРИ =====
            AddCSharpFunction("keys", args => new SproutValue(args[0].AsDict().Keys.Select(k => new SproutValue(k)).ToList()));
            AddCSharpFunction("values", args => new SproutValue(args[0].AsDict().Values.ToList()));
            AddCSharpFunction("items", args =>
            {
                var dict = args[0].AsDict();
                var items = new List<SproutValue>();
                foreach (var kv in dict)
                    items.Add(new SproutValue(new List<SproutValue> { new SproutValue(kv.Key), kv.Value }));
                return new SproutValue(items);
            });
            AddCSharpFunction("get", args =>
            {
                var dict = args[0].AsDict();
                string key = args[1].AsString();
                if (dict.ContainsKey(key))
                    return dict[key];
                if (args.Count > 2)
                    return args[2];
                return new SproutValue();
            });
            AddCSharpFunction("set", args =>
            {
                var dict = args[0].AsDict();
                string key = args[1].AsString();
                dict[key] = args[2];
                return new SproutValue(dict);
            });
            AddCSharpFunction("has_key", args => new SproutValue(args[0].AsDict().ContainsKey(args[1].AsString())));
            AddCSharpFunction("remove_key", args =>
            {
                var dict = args[0].AsDict();
                dict.Remove(args[1].AsString());
                return new SproutValue(dict);
            });
            AddCSharpFunction("dict_copy", args => new SproutValue(new Dictionary<string, SproutValue>(args[0].AsDict())));
            AddCSharpFunction("merge", args =>
            {
                var dict1 = args[0].AsDict();
                var dict2 = args[1].AsDict();
                var result = new Dictionary<string, SproutValue>(dict1);
                foreach (var kv in dict2)
                    result[kv.Key] = kv.Value;
                return new SproutValue(result);
            });

            // ===== МАТЕМАТИКА =====
            AddCSharpFunction("abs", args => new SproutValue(Math.Abs(args[0].AsNumber())));
            AddCSharpFunction("round", args => new SproutValue(Math.Round(args[0].AsNumber())));
            AddCSharpFunction("floor", args => new SproutValue(Math.Floor(args[0].AsNumber())));
            AddCSharpFunction("ceil", args => new SproutValue(Math.Ceiling(args[0].AsNumber())));
            AddCSharpFunction("pow", args => new SproutValue(Math.Pow(args[0].AsNumber(), args[1].AsNumber())));
            AddCSharpFunction("sqrt", args => new SproutValue(Math.Sqrt(args[0].AsNumber())));
            AddCSharpFunction("sin", args => new SproutValue(Math.Sin(args[0].AsNumber())));
            AddCSharpFunction("cos", args => new SproutValue(Math.Cos(args[0].AsNumber())));
            AddCSharpFunction("tan", args => new SproutValue(Math.Tan(args[0].AsNumber())));
            AddCSharpFunction("random", args => new SproutValue(new Random().Next((int)args[0].AsNumber(), (int)args[1].AsNumber() + 1)));
            AddCSharpFunction("random_float", args => new SproutValue(new Random().NextDouble() * (args[1].AsNumber() - args[0].AsNumber()) + args[0].AsNumber()));

            // ===== ПРОВЕРКА ТИПОВ =====
            AddCSharpFunction("is_string", args => new SproutValue(args[0].Type == SproutValue.ValueType.String));
            AddCSharpFunction("is_number", args => new SproutValue(args[0].Type == SproutValue.ValueType.Number));
            AddCSharpFunction("is_array", args => new SproutValue(args[0].Type == SproutValue.ValueType.Array));
            AddCSharpFunction("is_dict", args => new SproutValue(args[0].Type == SproutValue.ValueType.Dict));
            AddCSharpFunction("is_bool", args => new SproutValue(args[0].Type == SproutValue.ValueType.Bool));
            AddCSharpFunction("is_null", args => new SproutValue(args[0].Type == SproutValue.ValueType.Null));

            // ===== ТАБЛИЦЫ =====
            AddCSharpFunction("Table", args =>
            {
                return new SproutValue(new Table());
            });

            AddCSharpFunction("TableFromArray", args =>
            {
                var arr = args[0].AsArray();
                var table = new Table();
                for (int i = 0; i < arr.Count; i++)
                {
                    var value = arr[i];
                    if (value.Type == SproutValue.ValueType.Number)
                        table.Set(i + 1, DynValue.NewNumber(value.AsNumber()));
                    else if (value.Type == SproutValue.ValueType.String)
                        table.Set(i + 1, DynValue.NewString(value.AsString()));
                    else if (value.Type == SproutValue.ValueType.Bool)
                        table.Set(i + 1, DynValue.NewBoolean(value.AsBool()));
                    else if (value.Type == SproutValue.ValueType.Table)
                        table.Set(i + 1, DynValue.NewTable(value.AsTable()));
                    else
                        table.Set(i + 1, DynValue.NewNil());
                }
                return new SproutValue(table);
            });

            // ===== КОНСТАНТЫ =====
            globalEnv.SetVariable("PI", new SproutValue(Math.PI));
            globalEnv.SetVariable("E", new SproutValue(Math.E));
            
            Log("Стандартная библиотека загружена");
        }
    }
}