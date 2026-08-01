[< back](debugging.md)

---

# ✅ Best Practices

This guide covers recommended practices for creating high-quality Sprout libraries.

---

## 📐 Design Principles

### 1. Keep It Simple
- One library = one purpose
- Clear, descriptive method names
- Consistent naming conventions

### 2. Be Consistent
- Use `snake_case` for method names (matching Sprout style)
- Use same parameter order for similar methods
- Follow standard patterns

### 3. Handle Errors Gracefully
- Always validate input
- Return meaningful error messages
- Don't crash the interpreter

---

## 🔧 Method Design

### Good Example
```csharp
public static double Add(double a, double b)
{
    return a + b;
}

public static double Divide(double a, double b)
{
    if (b == 0)
        throw new Exception("Division by zero");
    return a / b;
}
```

### Bad Example
```csharp
public static double add(double a, double b) // wrong naming
{
    return a / b; // unexpected error
}
```

---

## 📝 Error Handling

### Use Meaningful Error Messages
```csharp
if (!File.Exists(path))
    throw new Exception($"File not found: {path}");
```

### Return Errors as Results
```csharp
public static string SafeRead(string path)
{
    try
    {
        return File.ReadAllText(path);
    }
    catch (Exception ex)
    {
        return $"Error: {ex.Message}";
    }
}
```

---

## 🧹 Code Organization

### Structure Your Library
```csharp
using System;
using System.Collections.Generic;

namespace MyLibrary
{
    public class MyLibrary
    {
        // Constants
        private const double PI = 3.14159;

        // Public methods
        public static double Add(double a, double b) { ... }
        public static double Sub(double a, double b) { ... }

        // Helper methods
        private static void Validate(double value) { ... }
    }
}
```

---

## 📊 Working with Sprout Types

### Check Types Before Access
```csharp
public static double GetNumber(SproutValue value)
{
    if (value.Type != SproutValue.ValueType.Number)
        return 0;
    return value.AsNumber();
}
```

### Handle Null Values
```csharp
public static string SafeString(SproutValue value)
{
    if (value.Type == SproutValue.ValueType.Null)
        return "null";
    return value.AsString();
}
```

---

## 🚀 Performance Tips

### Use Appropriate Types
```csharp
// ✅ Good
public static double Sum(List<double> numbers)

// ❌ Avoid (unless necessary)
public static double Sum(object numbers)
```

### Avoid Heavy Operations in Simple Methods
```csharp
// ✅ Good
public static int Add(int a, int b) => a + b;

// ❌ Avoid
public static int Add(int a, int b)
{
    // heavy logging, database calls, etc.
    return a + b;
}
```

---

## 📝 Documentation

### Add Comments
```csharp
/// <summary>
/// Adds two numbers.
/// </summary>
/// <param name="a">First number</param>
/// <param name="b">Second number</param>
/// <returns>Sum of a and b</returns>
public static double Add(double a, double b)
{
    return a + b;
}
```

---

## 🧪 Testing

### Test Your Library
Create test scripts for your library:

```sprout
import at "./lib"
import MyLibrary

out "Testing MyLibrary:"
out MyLibrary("Add", 5, 3)  # Expected: 8
out MyLibrary("Sub", 10, 4) # Expected: 6
```

---

## 📋 Summary

| Principle | Example |
|-----------|---------|
| Simple | One library = one purpose |
| Consistent | Use same patterns |
| Error handling | Check input, return meaningful errors |
| Documentation | Add comments and examples |
| Testing | Test all methods |

---

**Next: [Publishing](publishing.md)** 🚀
