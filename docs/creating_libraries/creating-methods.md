[< back](csharp-basics.md)

---

# 🛠️ Creating Methods

This guide explains how to create methods for Sprout libraries.

---

## 📝 Basic Method Structure

```csharp
public static ReturnType MethodName(ParameterType parameterName)
{
    // code
    return value;
}
```

---

## 🔧 Simple Methods

### One Parameter
```csharp
public static string Greet(string name)
{
    return "Hello, " + name + "!";
}
```

### Multiple Parameters
```csharp
public static double Add(double a, double b)
{
    return a + b;
}
```

### No Parameters
```csharp
public static string GetVersion()
{
    return "1.0.0";
}
```

### No Return Value (void)
```csharp
public static void PrintMessage(string msg)
{
    Console.WriteLine(msg);
}
```

---

## 📊 Working with Arrays

Sprout arrays become `List<T>` in C#:

```csharp
using System.Collections.Generic;

public static double Sum(List<double> numbers)
{
    double total = 0;
    foreach (var n in numbers)
        total += n;
    return total;
}
```

### Array of Strings
```csharp
public static string Concat(List<string> strings)
{
    return string.Join("", strings);
}
```

---

## 📖 Working with Dictionaries

```csharp
using System.Collections.Generic;

public static string GetValue(Dictionary<string, string> dict, string key)
{
    if (dict.ContainsKey(key))
        return dict[key];
    return "Key not found";
}
```

---

## 🎯 Methods with Optional Parameters

```csharp
public static string Greet(string name, string greeting = "Hello")
{
    return greeting + ", " + name + "!";
}
```

---

## 🔄 Method Overloading

Same name, different parameters:

```csharp
public static double Add(double a, double b)
{
    return a + b;
}

public static double Add(double a, double b, double c)
{
    return a + b + c;
}

public static string Add(string a, string b)
{
    return a + b;
}
```

---

## 🧪 Error Handling in Methods

```csharp
public static double SafeDivide(double a, double b)
{
    if (b == 0)
    {
        Console.WriteLine("Error: Division by zero");
        return 0;
    }
    return a / b;
}
```

---

## 🎨 Advanced Example

```csharp
using System;
using System.Collections.Generic;

public class MathLib
{
    // Basic arithmetic
    public static double Add(double a, double b) => a + b;
    public static double Sub(double a, double b) => a - b;
    public static double Mul(double a, double b) => a * b;
    public static double Div(double a, double b)
    {
        if (b == 0) throw new Exception("Division by zero");
        return a / b;
    }

    // Working with arrays
    public static double Sum(List<double> numbers)
    {
        double total = 0;
        foreach (var n in numbers)
            total += n;
        return total;
    }

    public static double Avg(List<double> numbers)
    {
        if (numbers.Count == 0) return 0;
        return Sum(numbers) / numbers.Count;
    }

    // Working with dictionaries
    public static string GetValue(Dictionary<string, string> dict, string key)
    {
        return dict.ContainsKey(key) ? dict[key] : "Key not found";
    }
}
```

---

## 📋 Summary

| Method Type | Example |
|-------------|---------|
| Single parameter | `Method(string name)` |
| Multiple params | `Method(int a, int b)` |
| Array/list param | `Method(List<double> list)` |
| Dictionary param | `Method(Dictionary<string, int> dict)` |
| Return value | `return value;` |
| No return | `void Method()` |
| Exception | `throw new Exception("msg")` |

---

**Next: [SproutValue](working-with-sproutvalue.md)** 🚀
