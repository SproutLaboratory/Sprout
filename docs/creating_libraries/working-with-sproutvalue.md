[< back](creating-methods.md)

---

# 🔄 SproutValue

This guide explains how to work with `SproutValue` — the universal type that represents all Sprout values in C# libraries.

---

## 📦 What is SproutValue?

`SproutValue` is a wrapper type that can hold any Sprout value:
- Numbers
- Strings
- Booleans
- Arrays
- Dictionaries
- Null
- Functions

---

## 🎯 When to Use SproutValue

Use `SproutValue` when your method needs to:

1. Accept any type of value
2. Work with dynamic data
3. Return different types
4. Process arrays and dictionaries generically

---

## 📊 SproutValue Types

| Type | Description | Accessor |
|------|-------------|----------|
| `Number` | Double-precision float | `.AsNumber()` |
| `String` | Text | `.AsString()` |
| `Bool` | True/false | `.AsBool()` |
| `Array` | List of values | `.AsArray()` |
| `Dict` | Key-value pairs | `.AsDict()` |
| `Null` | No value | `.IsNull()` |
| `Function` | Sprout function | `.AsFunction()` |
| `CSharpFunction` | C# function | `.AsCSharpFunction()` |

---

## 🔧 Basic Usage

### Checking Type
```csharp
public static string GetTypeName(SproutValue value)
{
    return value.Type.ToString();
}
```

### Converting to Specific Type
```csharp
public static double GetNumber(SproutValue value)
{
    if (value.Type == SproutValue.ValueType.Number)
        return value.AsNumber();
    return 0;
}
```

---

## 📊 Working with Arrays

```csharp
using System.Collections.Generic;

public static double SumArray(SproutValue value)
{
    if (value.Type != SproutValue.ValueType.Array)
        return 0;

    var arr = value.AsArray();
    double sum = 0;
    foreach (var item in arr)
    {
        if (item.Type == SproutValue.ValueType.Number)
            sum += item.AsNumber();
    }
    return sum;
}
```

---

## 📖 Working with Dictionaries

```csharp
using System.Collections.Generic;

public static string GetDictValue(SproutValue value, string key)
{
    if (value.Type != SproutValue.ValueType.Dict)
        return "Not a dictionary";

    var dict = value.AsDict();
    if (dict.ContainsKey(key))
        return dict[key].AsString();
    return "Key not found";
}
```

---

## 🧪 Creating SproutValue Objects

```csharp
// Number
SproutValue num = new SproutValue(42.0);

// String
SproutValue str = new SproutValue("Hello");

// Boolean
SproutValue boolean = new SproutValue(true);

// Array
var list = new List<SproutValue>();
list.Add(new SproutValue(1));
list.Add(new SproutValue(2));
SproutValue arr = new SproutValue(list);

// Dictionary
var dict = new Dictionary<string, SproutValue>();
dict["name"] = new SproutValue("Alex");
SproutValue dictVal = new SproutValue(dict);
```

---

## 🎯 Complete Example

```csharp
using System;
using System.Collections.Generic;

public class ValueProcessor
{
    public static string Process(SproutValue value)
    {
        switch (value.Type)
        {
            case SproutValue.ValueType.Number:
                return "Number: " + value.AsNumber();

            case SproutValue.ValueType.String:
                return "String: " + value.AsString();

            case SproutValue.ValueType.Bool:
                return "Boolean: " + value.AsBool();

            case SproutValue.ValueType.Array:
                var arr = value.AsArray();
                return "Array length: " + arr.Count;

            case SproutValue.ValueType.Dict:
                var dict = value.AsDict();
                return "Dictionary size: " + dict.Count;

            case SproutValue.ValueType.Null:
                return "Null value";

            default:
                return "Unknown type";
        }
    }

    public static SproutValue CreateArray()
    {
        var list = new List<SproutValue>();
        list.Add(new SproutValue(1));
        list.Add(new SproutValue("two"));
        list.Add(new SproutValue(true));
        return new SproutValue(list);
    }
}
```

---

## 📋 Summary

| Operation | Code |
|-----------|------|
| Check type | `value.Type == SproutValue.ValueType.Number` |
| Get number | `value.AsNumber()` |
| Get string | `value.AsString()` |
| Get bool | `value.AsBool()` |
| Get array | `value.AsArray()` |
| Get dict | `value.AsDict()` |
| Create value | `new SproutValue(value)` |

---

**Next: [External Libraries](using-external-libs.md)** 🚀
