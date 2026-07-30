using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SproutInterpreter
{
    public partial class Interpreter
    {
        private ScopedEnvironment globalEnv = new ScopedEnvironment();
        private string libraryPath = ".";
        private Dictionary<string, object> loadedLibraries = new Dictionary<string, object>();
        private bool enableLogging = false;
        private bool importPathSet = false;
        private bool _firstExecution = true; // Для логирования сборок при первом запуске

        public void EnableLogging()
        {
            enableLogging = true;
        }

        private void Log(string message, bool force = false)
        {
            if (enableLogging || force)
                Console.WriteLine($"  📌 {message}");
        }

        // ----- ЛОГИРОВАНИЕ ЗАГРУЖЕННЫХ СБОРОК -----
        private void LogLoadedAssemblies()
        {
            if (!enableLogging) return;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Log($"📦 Всего загружено сборок: {assemblies.Length}");
            foreach (var assembly in assemblies)
            {
                try
                {
                    var name = assembly.GetName();
                    Log($"  📚 {name.Name} v{name.Version}");
                }
                catch
                {
                    Log($"  📚 {assembly.FullName}");
                }
            }
        }

        public Interpreter()
        {
            // Подписываемся на событие поиска сборок
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                try
                {
                    string assemblyName = new AssemblyName(args.Name).Name;
                    
                    // Ищем в папке lib (уже установлена через import at)
                    string libPath = Path.Combine(libraryPath, assemblyName + ".dll");
                    if (File.Exists(libPath))
                    {
                        Log($"🔍 Загрузка зависимости: {assemblyName} из {libPath}");
                        return Assembly.LoadFile(libPath);
                    }
                    
                    // Ищем в папке с EXE + lib
                    string exeLibPath = Path.Combine(AppContext.BaseDirectory, "lib", assemblyName + ".dll");
                    if (File.Exists(exeLibPath))
                    {
                        Log($"🔍 Загрузка зависимости: {assemblyName} из {exeLibPath}");
                        return Assembly.LoadFile(exeLibPath);
                    }
                }
                catch (Exception ex)
                {
                    Log($"❌ Ошибка загрузки зависимости: {ex.Message}");
                }
                return null;
            };
            
            LoadStandardLibrary();
        }

        private bool IsNumericOrBool(SproutValue value)
        {
            return value.Type == SproutValue.ValueType.Number || value.Type == SproutValue.ValueType.Bool;
        }

        private double GetNumericValue(SproutValue value)
        {
            if (value.Type == SproutValue.ValueType.Bool)
                return value.AsBool() ? 1.0 : 0.0;
            if (value.Type == SproutValue.ValueType.Number)
                return value.AsNumber();
            throw new Exception($"Ожидалось число или bool, получено {value.Type}");
        }

        private void AddCSharpFunction(string name, Func<List<SproutValue>, SproutValue> func)
        {
            Log($"Регистрация C# функции: {name}");
            globalEnv.SetCSharpFunction(name, new CSharpFunction(name, func));
        }

        private ScopedEnvironment GetRootEnvironment(ScopedEnvironment env)
        {
            while (env.Parent != null)
                env = env.Parent;
            return env;
        }

        public SproutValue Execute(string code)
        {
            // При первом выполнении выводим список всех загруженных сборок
            if (_firstExecution)
            {
                _firstExecution = false;
                LogLoadedAssemblies();
            }

            try
            {
                Log($"Выполнение кода:\n{code}");
                
                var lexer = new Lexer(code);
                var tokens = lexer.Tokenize();
                
                if (enableLogging)
                {
                    Log("Токены:");
                    foreach (var t in tokens)
                        Log($"  {t}");
                }
                
                var parser = new Parser(tokens, enableLogging);
                var ast = parser.Parse();
                
                if (enableLogging)
                {
                    Log("AST узлы:");
                    foreach (var node in ast)
                        Log($"  {node.GetType().Name}");
                }

                var result = ExecuteNode(ast, globalEnv);
                return result;
            }
            catch (ReturnException ex)
            {
                return ex.Value;
            }
        }

        private SproutValue ExecuteNode(List<ASTNode> nodes, ScopedEnvironment env)
        {
            SproutValue result = new SproutValue();
            foreach (var node in nodes)
            {
                result = ExecuteNode(node, env);
                if (result != null && result.Type == SproutValue.ValueType.String && result.AsString() == "return")
                    break;
            }
            return result;
        }

        private SproutValue ExecuteNode(ASTNode node, ScopedEnvironment env)
        {
            if (node == null) return new SproutValue();

            return node switch
            {
                NumberNode n => new SproutValue(n.Value),
                StringNode s => new SproutValue(s.Value),
                BoolNode b => new SproutValue(b.Value),
                VariableNode v => ExecuteVariable(v, env),
                AssignmentNode a => ExecuteAssignment(a, env),
                BinaryOpNode b => ExecuteBinaryOp(b, env),
                UnaryOpNode u => ExecuteUnaryOp(u, env),
                OutNode o => ExecuteOut(o, env),
                InputNode i => ExecuteInput(i, env),
                FunctionDefNode f => ExecuteFunctionDef(f, env),
                CallNode c => ExecuteCall(c, env),
                ReturnSendNode r => ExecuteReturnSend(r, env),
                ReturnRunNode r => ExecuteReturnRun(r, env),
                IfNode i => ExecuteIf(i, env),
                RepeatNode r => ExecuteRepeat(r, env),
                ForNode f => ExecuteFor(f, env),
                WhileNode w => ExecuteWhile(w, env),
                BreakNode b => ExecuteBreak(b, env),
                ImportNode i => ExecuteImport(i, env),
                ArrayNode a => ExecuteArray(a, env),
                DictNode d => ExecuteDict(d, env),
                BlockNode b => ExecuteBlock(b, env),
                GlobalNode g => ExecuteGlobal(g, env),
                LocalNode l => ExecuteLocal(l, env),
                TryNode t => ExecuteTry(t, env),
                SetIndexNode s => ExecuteSetIndex(s, env),
                _ => throw new Exception($"Неизвестный узел: {node.GetType()}")
            };
        }

        private SproutValue ExecuteSetIndex(SetIndexNode node, ScopedEnvironment env)
        {
            var collection = ExecuteNode(node.Collection, env);
            var index = ExecuteNode(node.Index, env);
            var value = ExecuteNode(node.Value, env);
            
            if (collection.Type == SproutValue.ValueType.Array)
            {
                var arr = collection.AsArray();
                int idx = (int)index.AsNumber();
                if (idx < 0) idx = arr.Count + idx;
                if (idx < 0 || idx >= arr.Count)
                    throw new Exception($"Индекс {idx} вне диапазона [0, {arr.Count - 1}]");
                arr[idx] = value;
                return value;
            }
            
            if (collection.Type == SproutValue.ValueType.Dict)
            {
                var dict = collection.AsDict();
                string key = index.AsString();
                dict[key] = value;
                return value;
            }
            
            throw new Exception($"Нельзя установить значение по индексу для типа {collection.Type}");
        }

        private SproutValue ExecuteTry(TryNode node, ScopedEnvironment env)
        {
            try
            {
                var localEnv = env.CreateChild();
                return ExecuteNode(node.TryBody, localEnv);
            }
            catch (Exception ex)
            {
                Log($"Перехвачена ошибка: {ex.Message}");
                var catchEnv = env.CreateChild();
                catchEnv.SetVariable(node.CatchVariable, new SproutValue(ex.Message));
                return ExecuteNode(node.CatchBody, catchEnv);
            }
        }

        private SproutValue ExecuteVariable(VariableNode node, ScopedEnvironment env)
        {
            if (env.HasVariable(node.Name))
            {
                var val = env.GetVariable(node.Name);
                Log($"Переменная '{node.Name}' = {val}");
                return val;
            }
            throw new Exception($"Переменная '{node.Name}' не определена");
        }

        private SproutValue ExecuteAssignment(AssignmentNode node, ScopedEnvironment env)
        {
            var value = ExecuteNode(node.Value, env);
            env.SetVariable(node.Name, value);
            Log($"Переменная '{node.Name}' = {value}");
            return value;
        }

        private SproutValue ExecuteBinaryOp(BinaryOpNode node, ScopedEnvironment env)
        {
            if (node.Operator == "negate")
            {
                var operand = ExecuteNode(node.Right, env);
                if (operand.Type == SproutValue.ValueType.Number)
                    return new SproutValue(-operand.AsNumber());
                throw new Exception($"Нельзя применить отрицание к {operand.Type}");
            }

            if (node.Operator == "not")
            {
                var operand = ExecuteNode(node.Right, env);
                return new SproutValue(!operand.AsBool());
            }

            if (node.Operator == "index")
            {
                var left = ExecuteNode(node.Left, env);
                var right = ExecuteNode(node.Right, env);
                
                if (left.Type == SproutValue.ValueType.String)
                {
                    string s = left.AsString();
                    int idx = (int)right.AsNumber();
                    if (idx < 0) idx = s.Length + idx;
                    if (idx < 0 || idx >= s.Length)
                        throw new Exception($"Индекс {idx} вне диапазона [0, {s.Length - 1}]");
                    return new SproutValue(s[idx].ToString());
                }
                
                if (left.Type == SproutValue.ValueType.Array)
                {
                    var arr = left.AsArray();
                    int idx = (int)right.AsNumber();
                    if (idx < 0) idx = arr.Count + idx;
                    if (idx < 0 || idx >= arr.Count)
                        throw new Exception($"Индекс {idx} вне диапазона [0, {arr.Count - 1}]");
                    return arr[idx];
                }
                
                if (left.Type == SproutValue.ValueType.Dict)
                {
                    var dict = left.AsDict();
                    string key = right.AsString();
                    if (!dict.ContainsKey(key))
                        throw new Exception($"Ключ '{key}' не найден");
                    return dict[key];
                }
                
                throw new Exception($"Нельзя индексировать тип {left.Type}");
            }

            var leftVal = ExecuteNode(node.Left, env);
            var rightVal = ExecuteNode(node.Right, env);
            
            Log($"BinaryOp: {leftVal} {node.Operator} {rightVal}");

            if (node.Operator == "==" || node.Operator == "=?")
            {
                return CompareWithConversion(leftVal, rightVal);
            }

            if (node.Operator == "!=")
            {
                var result = CompareWithConversion(leftVal, rightVal);
                return new SproutValue(!result.AsBool());
            }

            if (node.Operator == "?=" || node.Operator == "??")
            {
                return CompareStrict(leftVal, rightVal);
            }

            if (leftVal.Type == SproutValue.ValueType.String && rightVal.Type == SproutValue.ValueType.Number)
            {
                if (node.Operator == "+")
                    return new SproutValue(leftVal.AsString() + rightVal.AsNumber().ToString());
                if (node.Operator == "*")
                {
                    int count = (int)rightVal.AsNumber();
                    if (count < 0) count = 0;
                    return new SproutValue(string.Concat(Enumerable.Repeat(leftVal.AsString(), count)));
                }
                throw new Exception($"Оператор '{node.Operator}' не поддерживается для строки и числа");
            }

            if (leftVal.Type == SproutValue.ValueType.Number && rightVal.Type == SproutValue.ValueType.String)
            {
                if (node.Operator == "+")
                    return new SproutValue(leftVal.AsNumber().ToString() + rightVal.AsString());
                if (node.Operator == "*")
                {
                    int count = (int)leftVal.AsNumber();
                    if (count < 0) count = 0;
                    return new SproutValue(string.Concat(Enumerable.Repeat(rightVal.AsString(), count)));
                }
                throw new Exception($"Оператор '{node.Operator}' не поддерживается для числа и строки");
            }

            if (leftVal.Type == SproutValue.ValueType.String && rightVal.Type == SproutValue.ValueType.String)
            {
                string l = leftVal.AsString();
                string r = rightVal.AsString();
                return node.Operator switch
                {
                    "+" => new SproutValue(l + r),
                    _ => throw new Exception($"Оператор '{node.Operator}' не поддерживается для строк")
                };
            }

            if (leftVal.Type == SproutValue.ValueType.Number && rightVal.Type == SproutValue.ValueType.Number)
            {
                double l = leftVal.AsNumber();
                double r = rightVal.AsNumber();
                return node.Operator switch
                {
                    "+" => new SproutValue(l + r),
                    "-" => new SproutValue(l - r),
                    "*" => new SproutValue(l * r),
                    "/" => new SproutValue(l / r),
                    "%" => new SproutValue(l % r),
                    "**" => new SproutValue(Math.Pow(l, r)),
                    ">" => new SproutValue(l > r),
                    "<" => new SproutValue(l < r),
                    ">=" => new SproutValue(l >= r),
                    "<=" => new SproutValue(l <= r),
                    _ => throw new Exception($"Неизвестный оператор: {node.Operator}")
                };
            }

            if (leftVal.Type == SproutValue.ValueType.Bool && rightVal.Type == SproutValue.ValueType.Bool)
            {
                bool l = leftVal.AsBool();
                bool r = rightVal.AsBool();
                return node.Operator switch
                {
                    "and" => new SproutValue(l && r),
                    "or" => new SproutValue(l || r),
                    _ => throw new Exception($"Оператор '{node.Operator}' не поддерживается для Bool")
                };
            }

            throw new Exception($"Несовместимые типы: {leftVal.Type} и {rightVal.Type} для оператора '{node.Operator}'");
        }

        private SproutValue CompareWithConversion(SproutValue left, SproutValue right)
        {
            if (left.Type == right.Type)
                return CompareSameType(left, right);

            if (left.Type == SproutValue.ValueType.Number && right.Type == SproutValue.ValueType.String)
            {
                if (double.TryParse(right.AsString(), System.Globalization.NumberStyles.Any, 
                    System.Globalization.CultureInfo.InvariantCulture, out double num))
                    return new SproutValue(left.AsNumber() == num);
                return new SproutValue(false);
            }
            if (left.Type == SproutValue.ValueType.String && right.Type == SproutValue.ValueType.Number)
            {
                if (double.TryParse(left.AsString(), System.Globalization.NumberStyles.Any, 
                    System.Globalization.CultureInfo.InvariantCulture, out double num))
                    return new SproutValue(num == right.AsNumber());
                return new SproutValue(false);
            }

            if (left.Type == SproutValue.ValueType.Number && right.Type == SproutValue.ValueType.Bool)
                return new SproutValue(left.AsNumber() == (right.AsBool() ? 1.0 : 0.0));
            if (left.Type == SproutValue.ValueType.Bool && right.Type == SproutValue.ValueType.Number)
                return new SproutValue((left.AsBool() ? 1.0 : 0.0) == right.AsNumber());

            if (left.Type == SproutValue.ValueType.String && right.Type == SproutValue.ValueType.Bool)
            {
                string s = left.AsString().ToLower().Trim();
                bool b = right.AsBool();
                if (s == "true" || s == "1") return new SproutValue(b == true);
                if (s == "false" || s == "0") return new SproutValue(b == false);
                return new SproutValue(false);
            }
            if (left.Type == SproutValue.ValueType.Bool && right.Type == SproutValue.ValueType.String)
            {
                string s = right.AsString().ToLower().Trim();
                bool b = left.AsBool();
                if (s == "true" || s == "1") return new SproutValue(b == true);
                if (s == "false" || s == "0") return new SproutValue(b == false);
                return new SproutValue(false);
            }

            if (left.Type == SproutValue.ValueType.Null || right.Type == SproutValue.ValueType.Null)
            {
                if (left.Type == SproutValue.ValueType.Null && right.Type == SproutValue.ValueType.Null)
                    return new SproutValue(true);
                return new SproutValue(false);
            }

            if (left.Type == SproutValue.ValueType.Array && right.Type == SproutValue.ValueType.Array)
            {
                var arr1 = left.AsArray();
                var arr2 = right.AsArray();
                if (arr1.Count != arr2.Count) return new SproutValue(false);
                for (int i = 0; i < arr1.Count; i++)
                {
                    var cmp = CompareWithConversion(arr1[i], arr2[i]);
                    if (!cmp.AsBool()) return new SproutValue(false);
                }
                return new SproutValue(true);
            }

            if (left.Type == SproutValue.ValueType.Dict && right.Type == SproutValue.ValueType.Dict)
            {
                var dict1 = left.AsDict();
                var dict2 = right.AsDict();
                if (dict1.Count != dict2.Count) return new SproutValue(false);
                foreach (var kv in dict1)
                {
                    if (!dict2.ContainsKey(kv.Key)) return new SproutValue(false);
                    var cmp = CompareWithConversion(kv.Value, dict2[kv.Key]);
                    if (!cmp.AsBool()) return new SproutValue(false);
                }
                return new SproutValue(true);
            }

            return new SproutValue(false);
        }

        private SproutValue CompareStrict(SproutValue left, SproutValue right)
        {
            if (left.Type != right.Type)
                return new SproutValue(false);
            return CompareSameType(left, right);
        }

        private SproutValue CompareSameType(SproutValue left, SproutValue right)
        {
            if (left.Type == SproutValue.ValueType.Number)
                return new SproutValue(left.AsNumber() == right.AsNumber());
            if (left.Type == SproutValue.ValueType.String)
                return new SproutValue(left.AsString() == right.AsString());
            if (left.Type == SproutValue.ValueType.Bool)
                return new SproutValue(left.AsBool() == right.AsBool());
            if (left.Type == SproutValue.ValueType.Null)
                return new SproutValue(true);
            if (left.Type == SproutValue.ValueType.Array)
            {
                var arr1 = left.AsArray();
                var arr2 = right.AsArray();
                if (arr1.Count != arr2.Count) return new SproutValue(false);
                for (int i = 0; i < arr1.Count; i++)
                {
                    var cmp = CompareStrict(arr1[i], arr2[i]);
                    if (!cmp.AsBool()) return new SproutValue(false);
                }
                return new SproutValue(true);
            }
            if (left.Type == SproutValue.ValueType.Dict)
            {
                var dict1 = left.AsDict();
                var dict2 = right.AsDict();
                if (dict1.Count != dict2.Count) return new SproutValue(false);
                foreach (var kv in dict1)
                {
                    if (!dict2.ContainsKey(kv.Key)) return new SproutValue(false);
                    var cmp = CompareStrict(kv.Value, dict2[kv.Key]);
                    if (!cmp.AsBool()) return new SproutValue(false);
                }
                return new SproutValue(true);
            }
            return new SproutValue(false);
        }

        private SproutValue ExecuteUnaryOp(UnaryOpNode node, ScopedEnvironment env)
        {
            var operand = ExecuteNode(node.Operand, env);
            Log($"UnaryOp: {node.Operator}({operand})");
            return node.Operator switch
            {
                "int" => ConvertToInt(operand),
                "float" => ConvertToFloat(operand),
                "bool" => ConvertToBool(operand),
                "str" => new SproutValue(operand.ToString()),
                _ => throw new Exception($"Неизвестный унарный оператор: {node.Operator}")
            };
        }

        private SproutValue ConvertToInt(SproutValue value)
        {
            if (value.Type == SproutValue.ValueType.Bool)
                return new SproutValue(value.AsBool() ? 1.0 : 0.0);
            if (value.Type == SproutValue.ValueType.Number)
                return new SproutValue(Math.Floor(value.AsNumber()));
            if (value.Type == SproutValue.ValueType.String)
            {
                string s = value.AsString().Trim();
                if (int.TryParse(s, System.Globalization.NumberStyles.Integer, 
                    System.Globalization.CultureInfo.InvariantCulture, out int intResult))
                    return new SproutValue(intResult);
                if (double.TryParse(s, System.Globalization.NumberStyles.Any, 
                    System.Globalization.CultureInfo.InvariantCulture, out double doubleResult))
                    return new SproutValue(Math.Floor(doubleResult));
                throw new Exception($"Не удалось преобразовать '{s}' в int");
            }
            throw new Exception($"Не удалось преобразовать {value.Type} в int");
        }

        private SproutValue ConvertToFloat(SproutValue value)
        {
            if (value.Type == SproutValue.ValueType.Bool)
                return new SproutValue(value.AsBool() ? 1.0 : 0.0);
            if (value.Type == SproutValue.ValueType.Number)
                return new SproutValue(value.AsNumber());
            if (value.Type == SproutValue.ValueType.String)
            {
                string s = value.AsString().Trim();
                if (double.TryParse(s, System.Globalization.NumberStyles.Any, 
                    System.Globalization.CultureInfo.InvariantCulture, out double num))
                    return new SproutValue(num);
                throw new Exception($"Не удалось преобразовать '{s}' в float");
            }
            throw new Exception($"Не удалось преобразовать {value.Type} в float");
        }

        private SproutValue ConvertToBool(SproutValue value)
        {
            if (value.Type == SproutValue.ValueType.Number)
                return new SproutValue(value.AsNumber() != 0);
            if (value.Type == SproutValue.ValueType.String)
            {
                string s = value.AsString().Trim();
                if (s == "0") return new SproutValue(false);
                if (s == "1") return new SproutValue(true);
                return new SproutValue(!string.IsNullOrEmpty(s));
            }
            if (value.Type == SproutValue.ValueType.Null)
                return new SproutValue(false);
            if (value.Type == SproutValue.ValueType.Array)
                return new SproutValue(value.AsArray().Count > 0);
            if (value.Type == SproutValue.ValueType.Dict)
                return new SproutValue(value.AsDict().Count > 0);
            if (value.Type == SproutValue.ValueType.Bool)
                return value;
            return new SproutValue(true);
        }

        private SproutValue ExecuteOut(OutNode node, ScopedEnvironment env)
        {
            var result = ExecuteNode(node.Expression, env);
            var output = result.ToString();
            Console.WriteLine(output);
            Log($"OUT: {output}");
            return new SproutValue();
        }

        private SproutValue ExecuteInput(InputNode node, ScopedEnvironment env)
        {
            if (node.Prompt != null)
            {
                var prompt = ExecuteNode(node.Prompt, env);
                Console.Write(prompt.ToString() + " ");
            }
            var input = Console.ReadLine();
            Log($"INPUT: {input}");
            if (node.AsType == "int")
            {
                if (int.TryParse(input, out int intResult))
                    return new SproutValue(intResult);
                if (double.TryParse(input, out double doubleResult))
                    return new SproutValue(Math.Floor(doubleResult));
                throw new Exception($"Не удалось преобразовать '{input}' в int");
            }
            else if (node.AsType == "float")
            {
                if (double.TryParse(input, out double result))
                    return new SproutValue(result);
                throw new Exception($"Не удалось преобразовать '{input}' в число");
            }
            else if (node.AsType == "bool")
            {
                if (input == "1") return new SproutValue(true);
                if (input == "0") return new SproutValue(false);
                if (bool.TryParse(input, out bool boolResult))
                    return new SproutValue(boolResult);
                throw new Exception($"Не удалось преобразовать '{input}' в bool");
            }
            return new SproutValue(input);
        }

        private SproutValue ExecuteFunctionDef(FunctionDefNode node, ScopedEnvironment env)
        {
            var func = new Function(node.Name, node.Parameters, node.Body, env);
            func.Scope = node.Scope;
            env.SetFunction(node.Name, func);
            Log($"Функция определена: {node.Name}({string.Join(", ", node.Parameters)}) [{node.Scope}]");
            return new SproutValue();
        }

        private object FindFunction(string name, ScopedEnvironment env)
        {
            if (globalEnv.HasFunction(name))
                return globalEnv.GetFunction(name);
            if (globalEnv.HasVariable(name))
            {
                var varValue = globalEnv.GetVariable(name);
                if (varValue.Type == SproutValue.ValueType.CSharpFunction || varValue.Type == SproutValue.ValueType.Function)
                    return varValue.Value;
            }
            if (env.HasFunction(name))
                return env.GetFunction(name);
            if (env.HasVariable(name))
            {
                var varValue = env.GetVariable(name);
                if (varValue.Type == SproutValue.ValueType.CSharpFunction || varValue.Type == SproutValue.ValueType.Function)
                    return varValue.Value;
            }
            return null;
        }

        private SproutValue ExecuteCall(CallNode node, ScopedEnvironment env)
        {
            Log($"Вызов: {node.Name}({node.Arguments.Count} аргументов)");
            
            // Проверяем, не является ли это библиотекой
            if (globalEnv.HasVariable(node.Name))
            {
                var varValue = globalEnv.GetVariable(node.Name);
                if (varValue.Type == SproutValue.ValueType.Dict)
                {
                    Log($"Обнаружена библиотека '{node.Name}' (словарь)");
                    return ExecuteLibraryCall(varValue.AsDict(), node, env);
                }
            }
            
            if (env.HasVariable(node.Name))
            {
                var varValue = env.GetVariable(node.Name);
                if (varValue.Type == SproutValue.ValueType.Dict)
                {
                    Log($"Обнаружена библиотека '{node.Name}' (словарь)");
                    return ExecuteLibraryCall(varValue.AsDict(), node, env);
                }
            }

            // Обычная функция
            object funcObj = FindFunction(node.Name, env);
            
            if (funcObj != null)
            {
                if (funcObj is CSharpFunction csharpFunc)
                {
                    Log($"Вызов C# функции: {node.Name}");
                    return ExecuteCSharpFunction(csharpFunc, node.Arguments, env);
                }
                if (funcObj is Function func)
                {
                    Log($"Вызов пользовательской функции: {node.Name}");
                    return ExecuteUserFunction(func, node.Arguments, env);
                }
            }

            throw new Exception($"Функция или библиотека '{node.Name}' не определена");
        }

        private SproutValue ExecuteLibraryCall(Dictionary<string, SproutValue> dict, CallNode node, ScopedEnvironment env)
        {
            Log($"Вызов библиотеки '{node.Name}': {dict.Count} методов");

            if (node.Arguments.Count == 0)
                throw new Exception($"Не указано имя метода для вызова библиотеки '{node.Name}'");
            
            var methodNameValue = ExecuteNode(node.Arguments[0], env);
            string methodName = methodNameValue.AsString();
            Log($"Имя метода: '{methodName}'");
            
            // Поиск метода в словаре библиотеки
            if (dict.ContainsKey(methodName))
            {
                var method = dict[methodName];
                if (method.Type == SproutValue.ValueType.CSharpFunction)
                {
                    var args = node.Arguments.Skip(1).ToList();
                    Log($"Вызов C# метода: {methodName} с {args.Count} аргументами");
                    return ExecuteCSharpFunction(method.AsCSharpFunction(), args, env);
                }
                else
                {
                    Log($"Метод '{methodName}' не является C# функцией, тип: {method.Type}");
                }
            }
            
            // Поиск с игнорированием регистра
            foreach (var kv in dict)
            {
                if (string.Equals(kv.Key, methodName, StringComparison.OrdinalIgnoreCase))
                {
                    if (kv.Value.Type == SproutValue.ValueType.CSharpFunction)
                    {
                        var args = node.Arguments.Skip(1).ToList();
                        Log($"Вызов C# метода (игнорируя регистр): {kv.Key} с {args.Count} аргументами");
                        return ExecuteCSharpFunction(kv.Value.AsCSharpFunction(), args, env);
                    }
                }
            }
            
            // Если метод не найден, выводим список доступных
            var availableMethods = string.Join(", ", dict.Keys);
            Log($"Доступные методы: {availableMethods}");
            throw new Exception($"Метод '{methodName}' не найден в библиотеке '{node.Name}'. Доступные: {availableMethods}");
        }

        private SproutValue ExecuteCSharpFunction(CSharpFunction csharpFunc, List<ASTNode> argNodes, ScopedEnvironment env)
        {
            try
            {
                Log($"  Выполнение C# функции: {csharpFunc.Name}");
                var args = argNodes.Select(a => ExecuteNode(a, env)).ToList();
                Log($"  Аргументы: {string.Join(", ", args.Select(a => a.ToString()))}");
                
                var result = csharpFunc.Func(args);
                Log($"  Результат: {result}");
                return result;
            }
            catch (TargetInvocationException ex)
            {
                var innerEx = ex.InnerException ?? ex;
                string errorMessage = innerEx.Message;
                if (string.IsNullOrEmpty(errorMessage))
                    errorMessage = "Ошибка без сообщения (возможно, нативная)";
                
                Log($"❌ Ошибка в C# функции: {errorMessage}");
                Log($"   StackTrace: {innerEx.StackTrace ?? "Нет StackTrace"}");
                
                // Возвращаем ошибку как результат
                return new SproutValue($"❌ Ошибка: {errorMessage}");
            }
            catch (Exception ex)
            {
                Log($"❌ Ошибка при выполнении C# функции: {ex.Message}");
                return new SproutValue($"❌ Ошибка: {ex.Message}");
            }
        }

        private SproutValue ExecuteUserFunction(Function func, List<ASTNode> args, ScopedEnvironment env)
        {
            ScopedEnvironment localEnv = func.Scope == "global" ? new ScopedEnvironment(GetRootEnvironment(env)) : new ScopedEnvironment();

            if (args.Count != func.Parameters.Count)
                throw new Exception($"Функция {func.Name} ожидает {func.Parameters.Count} аргументов, получено {args.Count}");

            for (int i = 0; i < args.Count; i++)
            {
                var value = ExecuteNode(args[i], env);
                localEnv.SetVariable(func.Parameters[i], value);
                Log($"Параметр {func.Parameters[i]} = {value}");
            }

            SproutValue result = new SproutValue();
            foreach (var stmt in func.Body)
            {
                try 
                { 
                    result = ExecuteNode(stmt, localEnv); 
                }
                catch (ReturnException ex) 
                { 
                    Log($"Return: {ex.Value}");
                    return ex.Value; 
                }
            }
            return result;
        }

        private SproutValue ExecuteReturnSend(ReturnSendNode node, ScopedEnvironment env)
        {
            var value = ExecuteNode(node.Value, env);
            Log($"Return send: {value}");
            throw new ReturnException(value);
        }

        private SproutValue ExecuteReturnRun(ReturnRunNode node, ScopedEnvironment env)
        {
            var localEnv = env.CreateChild();
            foreach (var stmt in node.Body)
            {
                try { ExecuteNode(stmt, localEnv); }
                catch (ReturnException ex) 
                { 
                    Log($"Return run: {ex.Value}");
                    return ex.Value; 
                }
            }
            return new SproutValue();
        }

        private SproutValue ExecuteIf(IfNode node, ScopedEnvironment env)
        {
            var condition = ExecuteNode(node.Condition, env);
            Log($"If условие: {condition}");
            
            if (condition.AsBool())
            {
                Log($"If: true");
                var localEnv = env.CreateChild();
                return ExecuteNode(node.ThenBody, localEnv);
            }
            else if (node.ElseBody.Count > 0)
            {
                Log($"If: false, else");
                var localEnv = env.CreateChild();
                return ExecuteNode(node.ElseBody, localEnv);
            }
            Log($"If: false, нет else");
            return new SproutValue();
        }

        private SproutValue ExecuteRepeat(RepeatNode node, ScopedEnvironment env)
        {
            var countVal = ExecuteNode(node.Count, env);
            Log($"Repeat: {countVal} раз");
            
            if (countVal.Type != SproutValue.ValueType.Number && countVal.Type != SproutValue.ValueType.Bool)
                throw new Exception($"repeat: ожидается число или bool, получено {countVal.Type}");
            
            int iterations = (int)GetNumericValue(countVal);

            for (int i = 0; i < iterations; i++)
            {
                var localEnv = env.CreateChild();
                if (!string.IsNullOrEmpty(node.Variable))
                    localEnv.SetVariable(node.Variable, new SproutValue(i));

                try { ExecuteNode(node.Body, localEnv); }
                catch (BreakException) { break; }
            }
            return new SproutValue();
        }

        private SproutValue ExecuteFor(ForNode node, ScopedEnvironment env)
        {
            var startVal = ExecuteNode(node.Start, env);
            var endVal = ExecuteNode(node.End, env);
            var stepVal = ExecuteNode(node.Step, env);
            
            Log($"For: {node.Variable} от {startVal} до {endVal} с шагом {stepVal}");
            
            if (!IsNumericOrBool(startVal))
                throw new Exception($"for: ожидается число или bool, получено {startVal.Type}");
            if (!IsNumericOrBool(endVal))
                throw new Exception($"for: ожидается число или bool, получено {endVal.Type}");
            if (!IsNumericOrBool(stepVal))
                throw new Exception($"for: ожидается число или bool, получено {stepVal.Type}");
            
            double start = GetNumericValue(startVal);
            double end = GetNumericValue(endVal);
            double step = GetNumericValue(stepVal);
            
            var localEnv = env.CreateChild();
            
            if (step > 0)
            {
                for (double i = start; i <= end; i += step)
                {
                    localEnv.SetVariable(node.Variable, new SproutValue(i));
                    try { ExecuteNode(node.Body, localEnv); }
                    catch (BreakException) { break; }
                }
            }
            else if (step < 0)
            {
                for (double i = start; i >= end; i += step)
                {
                    localEnv.SetVariable(node.Variable, new SproutValue(i));
                    try { ExecuteNode(node.Body, localEnv); }
                    catch (BreakException) { break; }
                }
            }
            else
            {
                throw new Exception("for: шаг не может быть 0");
            }
            
            return new SproutValue();
        }

        private SproutValue ExecuteWhile(WhileNode node, ScopedEnvironment env)
        {
            Log($"While: условие {node.Condition}");
            
            while (true)
            {
                var condition = ExecuteNode(node.Condition, env);
                if (condition.Type != SproutValue.ValueType.Number && condition.Type != SproutValue.ValueType.Bool)
                    throw new Exception($"while: ожидается число или bool, получено {condition.Type}");
                if (!condition.AsBool()) break;

                var localEnv = env.CreateChild();
                try { ExecuteNode(node.Body, localEnv); }
                catch (BreakException) { break; }
            }
            return new SproutValue();
        }

        private class BreakException : Exception { }

        private SproutValue ExecuteBreak(BreakNode node, ScopedEnvironment env)
        {
            Log("Break");
            throw new BreakException();
        }

        private SproutValue ExecuteArray(ArrayNode node, ScopedEnvironment env)
        {
            var list = new List<SproutValue>();
            foreach (var elem in node.Elements)
                list.Add(ExecuteNode(elem, env));
            Log($"Создан массив: {list.Count} элементов");
            return new SproutValue(list);
        }

        private SproutValue ExecuteDict(DictNode node, ScopedEnvironment env)
        {
            var dict = new Dictionary<string, SproutValue>();
            foreach (var kv in node.Elements)
                dict[kv.Key] = ExecuteNode(kv.Value, env);
            Log($"Создан словарь: {dict.Count} ключей");
            return new SproutValue(dict);
        }

        private SproutValue ExecuteBlock(BlockNode node, ScopedEnvironment env)
        {
            var localEnv = env.CreateChild();
            return ExecuteNode(node.Statements, localEnv);
        }

        #region Глобализация
        private SproutValue ExecuteGlobal(GlobalNode node, ScopedEnvironment env)
        {
            string varName = node.VariableName;
            if (!env.HasVariable(varName))
                throw new Exception($"Переменная '{varName}' не определена локально");
            var value = env.GetVariable(varName);
            var rootEnv = GetRootEnvironment(env);
            rootEnv.SetVariable(varName, value);
            Log($"global(): {varName} = {value}");
            return new SproutValue();
        }

        private SproutValue ExecuteLocal(LocalNode node, ScopedEnvironment env)
        {
            string varName = node.VariableName;
            var rootEnv = GetRootEnvironment(env);
            if (!rootEnv.HasVariable(varName))
                throw new Exception($"Глобальная переменная '{varName}' не определена");
            var value = rootEnv.GetVariable(varName);
            env.SetVariable(varName, value);
            Log($"local(): {varName} = {value}");
            return new SproutValue();
        }
        #endregion

        #region Импорт библиотек
        private SproutValue ExecuteImport(ImportNode node, ScopedEnvironment env)
        {
            if (node.IsPathSetter)
            {
                if (importPathSet)
                    throw new Exception("import at может быть только в начале кода");
                
                string exeDir = AppContext.BaseDirectory;
                string path = node.PathToSet;
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(exeDir, path);
                libraryPath = Path.GetFullPath(path);
                importPathSet = true;
                Log($"Путь к библиотекам: {libraryPath}");
                Console.WriteLine($"📁 Путь к библиотекам: {libraryPath}");
                return new SproutValue();
            }

            string libName = node.LibraryName;
            Log($"Импорт библиотеки: '{libName}'");
            
            if (loadedLibraries.ContainsKey(libName))
                throw new Exception($"Библиотека '{libName}' уже загружена");

            string dllPath = Path.Combine(libraryPath, libName + ".dll");
            dllPath = Path.GetFullPath(dllPath);
            Log($"Поиск DLL: {dllPath}");

            if (!File.Exists(dllPath))
            {
                string exePath = AppContext.BaseDirectory; // ← Добавить эту строку!
                dllPath = Path.Combine(exePath, libName + ".dll");
                dllPath = Path.GetFullPath(dllPath);
                Log($"Поиск в папке EXE: {dllPath}");
            }
            if (!File.Exists(dllPath))
            {
                Log($"❌ DLL НЕ НАЙДЕНА!");
                throw new Exception($"Библиотека '{libName}' не найдена. Искали в:\n  {libraryPath}\n  {AppContext.BaseDirectory}");
            }

            Log($"✅ DLL найдена: {dllPath}");

            try
            {
                var assembly = Assembly.LoadFile(dllPath);
                Log($"✅ DLL загружена");
                // Дополнительное логирование: имя и версия сборки
                Log($"   Имя: {assembly.GetName().Name}, версия: {assembly.GetName().Version}");
                
                var libObj = new Dictionary<string, SproutValue>();

                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && type.IsPublic)
                    {
                        Log($"  📦 Класс: {type.Name}");
                        var methodDict = new Dictionary<string, SproutValue>();
                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

                        foreach (var method in methods)
                        {
                            if (method.IsSpecialName) continue;
                            Log($"    📝 Метод: {method.Name} ({method.GetParameters().Length} параметров)");

                            var func = new CSharpFunction(method.Name, args =>
                            {
                                try
                                {
                                    var parameters = method.GetParameters();
                                    if (args.Count != parameters.Length)
                                        throw new Exception($"Метод {method.Name} ожидает {parameters.Length} аргументов, получено {args.Count}");

                                    var convertedArgs = new object[args.Count];
                                    for (int i = 0; i < args.Count; i++)
                                    {
                                        convertedArgs[i] = ConvertValue(args[i], parameters[i].ParameterType);
                                    }

                                    object instance = null;
                                    if (!method.IsStatic)
                                        instance = Activator.CreateInstance(type);

                                    var result = method.Invoke(instance, convertedArgs);
                                    return ConvertToSproutValue(result);
                                }
                                catch (TargetInvocationException ex)
                                {
                                    var innerEx = ex.InnerException ?? ex;
                                    Log($"    ❌ Ошибка в методе {method.Name}: {innerEx.Message}");
                                    throw new Exception($"{innerEx.Message}");
                                }
                                catch (Exception ex)
                                {
                                    Log($"    ❌ Ошибка в методе {method.Name}: {ex.Message}");
                                    throw;
                                }
                            });

                            methodDict[method.Name] = new SproutValue(func);
                        }

                        foreach (var kv in methodDict)
                            libObj[kv.Key] = kv.Value;
                    }
                }

                loadedLibraries[libName] = libObj;

                CSharpFunction libraryFunction = new CSharpFunction(libName, args =>
                {
                    if (args.Count == 0)
                        throw new Exception($"Не указано имя метода для библиотеки '{libName}'");
                    
                    string methodName = args[0].AsString();
                    var methodArgs = args.Skip(1).ToList();
                    
                    if (!libObj.ContainsKey(methodName))
                    {
                        var available = string.Join(", ", libObj.Keys);
                        throw new Exception($"Метод '{methodName}' не найден в библиотеке '{libName}'. Доступные: {available}");
                    }
                    
                    var method = libObj[methodName];
                    if (method.Type != SproutValue.ValueType.CSharpFunction)
                        throw new Exception($"Метод '{methodName}' не является C# функцией");
                    
                    return method.AsCSharpFunction().Func(methodArgs);
                });

                env.SetVariable(libName, new SproutValue(libObj));
                env.SetCSharpFunction(libName, libraryFunction);

                Log($"✅ Библиотека '{libName}' загружена, методов: {libObj.Count}");
                return new SproutValue(libObj);
            }
            catch (Exception ex)
            {
                Log($"❌ Ошибка загрузки: {ex.Message}");
                Log($"   StackTrace: {ex.StackTrace}");
                throw new Exception($"Ошибка загрузки библиотеки '{libName}': {ex.Message}");
            }
        }

        private object ConvertValue(SproutValue value, Type targetType)
        {
            if (value.Type == SproutValue.ValueType.Null)
                return null;
            if (targetType == typeof(int))
                return (int)value.AsNumber();
            if (targetType == typeof(double))
                return value.AsNumber();
            if (targetType == typeof(float))
                return (float)value.AsNumber();
            if (targetType == typeof(string))
                return value.AsString();
            if (targetType == typeof(bool))
                return value.AsBool();
            if (targetType.IsArray && value.Type == SproutValue.ValueType.Array)
            {
                var arr = value.AsArray();
                var elementType = targetType.GetElementType();
                var result = Array.CreateInstance(elementType, arr.Count);
                for (int i = 0; i < arr.Count; i++)
                    result.SetValue(ConvertValue(arr[i], elementType), i);
                return result;
            }
            return value.Value;
        }

        private SproutValue ConvertToSproutValue(object obj)
        {
            if (obj == null)
                return new SproutValue();
            Type type = obj.GetType();
            if (type == typeof(int) || type == typeof(double) || type == typeof(float) || type == typeof(decimal))
                return new SproutValue(Convert.ToDouble(obj));
            if (type == typeof(string))
                return new SproutValue((string)obj);
            if (type == typeof(bool))
                return new SproutValue((bool)obj);
            if (type.IsArray)
            {
                var arr = (Array)obj;
                var result = new List<SproutValue>();
                foreach (var item in arr)
                    result.Add(ConvertToSproutValue(item));
                return new SproutValue(result);
            }
            return new SproutValue(obj.ToString());
        }
        #endregion
    }
}