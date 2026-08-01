[< back](using-external-libs.md)

---

# 🐛 Debugging

This guide explains how to debug Sprout libraries effectively.

---

## 🔍 Types of Debugging

1. **Console Output** — simplest, works everywhere
2. **Logging** — use a logging library
3. **Debugger** — attach a debugger to the process

---

## 📝 Console Output

Add `Console.WriteLine()` to your library:

```csharp
public static double Add(double a, double b)
{
    Console.WriteLine($"Add called with a={a}, b={b}");
    double result = a + b;
    Console.WriteLine($"Result: {result}");
    return result;
}
```

---

## 🔧 Using `-log` Flag in Sprout

Run Sprout with `-log` to see detailed logs:

```bash
Sprout.exe -log script.sprout
```

This shows:
- Tokenization
- AST parsing
- Execution steps
- Function calls

---

## 📄 Logging to File

```csharp
using System.IO;

public static void Log(string message)
{
    File.AppendAllText("debug.log", $"{DateTime.Now}: {message}\n");
}
```

---

## 🔧 Attaching a Debugger (Visual Studio)

1. Open your library project in Visual Studio
2. Set breakpoints
3. Run Sprout.exe:
   ```bash
   Sprout.exe script.sprout
   ```
4. In Visual Studio: **Debug → Attach to Process**
5. Select `Sprout.exe`
6. Execute code that calls your library

---

## 🔧 Using `try`/`catch` in C#

```csharp
public static string SafeMethod(string input)
{
    try
    {
        // risky code
        return ProcessInput(input);
    }
    catch (Exception ex)
    {
        return $"Error: {ex.Message}";
    }
}
```

---

## 🔧 Verifying Method Signatures

Make sure your methods match what Sprout expects:

| C# Method | Sprout Call |
|-----------|-------------|
| `public static string Hello(string name)` | `MyLib("Hello", "Alex")` |
| `public static int Add(int a, int b)` | `str MyLib("Add", 5, 3)` |
| `public static double Sum(List<double> arr)` | `str MyLib("Sum", [1,2,3])` |

---

## 📊 Common Errors and Solutions

| Error | Cause | Solution |
|-------|-------|----------|
| `Method 'X' not found` | Wrong method name or signature | Check spelling and parameters |
| `Argument mismatch` | Wrong number of arguments | Check method parameters |
| `Object not set to instance` | Null reference | Add null checking |
| `Type conversion failed` | Wrong parameter type | Convert to correct type |

---

## 🧪 Example with Full Debugging

```csharp
using System;
using System.IO;
using System.Collections.Generic;

public class DebugLib
{
    private static string logFile = "debug.log";

    public static string ProcessData(List<object> data)
    {
        try
        {
            Log("ProcessData called with " + data.Count + " items");

            // Debug output
            foreach (var item in data)
            {
                Log($"  Item: {item} (Type: {item?.GetType()})");
            }

            // Your logic here
            return "Success";
        }
        catch (Exception ex)
        {
            Log($"Error: {ex.Message}");
            Log($"StackTrace: {ex.StackTrace}");
            return $"Error: {ex.Message}";
        }
    }

    private static void Log(string message)
    {
        string entry = $"{DateTime.Now:HH:mm:ss} - {message}";
        Console.WriteLine(entry);
        File.AppendAllText(logFile, entry + "\n");
    }
}
```

---

## 📋 Summary

| Technique | Use Case |
|-----------|----------|
| `Console.WriteLine` | Quick debugging |
| `-log` flag | Sprout execution logs |
| File logging | Persistent logs |
| Visual Studio debugger | Deep debugging |
| `try`/`catch` | Error handling |
| Logging library | Professional logging |

---

**Next: [Best Practices](best-practices.md)** 🚀
