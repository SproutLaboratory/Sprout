[< back](getting-started.md)

---

# 📘 C# Basics for Sprout Libraries

This guide covers the essential C# knowledge you need to create libraries for Sprout.

---

## 📦 Namespaces

Every library should be wrapped in a namespace:

```csharp
namespace MyLibrary
{
    // code goes here
}
```

---

## 🏛️ Public Classes

The class must be `public` so Sprout can access it:

```csharp
public class MyLibrary
{
    // methods go here
}
```

---

## 🔧 Static Methods

All methods that Sprout calls must be `public` and `static`:

```csharp
public static string SayHello(string name)
{
    return $"Hello, {name}!";
}
```

---

## 📊 Parameter Types

Sprout automatically converts values to C# types:

| Sprout Type | C# Type |
|-------------|---------|
| Number | `double`, `int`, `float` |
| String | `string` |
| Boolean | `bool` |
| Array | `List<T>` or `T[]` |
| Dictionary | `Dictionary<string, T>` |
| Null | `null` or `Nullable<T>` |

---

## 🔄 Working with Arrays

Sprout arrays become `List<T>` in C#:

```csharp
public static double Sum(List<double> numbers)
{
    double total = 0;
    foreach (var n in numbers)
        total += n;
    return total;
}
```

---

## 📖 Working with Dictionaries

```csharp
public static string GetValue(Dictionary<string, string> dict, string key)
{
    if (dict.ContainsKey(key))
        return dict[key];
    return "Key not found";
}
```

---

## ⚠️ Error Handling

Use `try`/`catch` to handle errors gracefully:

```csharp
public static string SafeDivide(double a, double b)
{
    try
    {
        return (a / b).ToString();
    }
    catch
    {
        return "Error: Cannot divide by zero";
    }
}
```

---

## 🎯 Method Overloading

You can have multiple methods with the same name but different parameters:

```csharp
public static double Add(double a, double b)
{
    return a + b;
}

public static double Add(double a, double b, double c)
{
    return a + b + c;
}
```

---

## 🧪 Working with External Libraries

You can use NuGet packages in your library:

```bash
dotnet add package Newtonsoft.Json
```

```csharp
using Newtonsoft.Json;

public static string ToJson(object obj)
{
    return JsonConvert.SerializeObject(obj);
}
```

---

## 📋 Summary

| Concept | Syntax |
|---------|--------|
| Namespace | `namespace MyLib { ... }` |
| Public class | `public class MyClass { ... }` |
| Static method | `public static void Method() { ... }` |
| Return type | `public static string Method() { ... }` |
| List parameter | `public static void Method(List<double> list)` |
| Dictionary parameter | `public static void Method(Dictionary<string, int> dict)` |

---

**Next: [Creating Methods](creating-methods.md)** 🚀
