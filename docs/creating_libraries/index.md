[< back](../SUMMARY.md)

---

# 📦 Creating Libraries

Welcome to the guide for creating libraries for Sprout!

---

## 📚 What's Inside

| Topic | Description |
|-------|-------------|
| [Quick Start](getting-started.md) | Create your first library |
| [C# Basics](csharp-basics.md) | What you need to know about C# |
| [Creating Methods](creating-methods.md) | How to add functions for Sprout |
| [SproutValue](working-with-sproutvalue.md) | Working with Sprout data types |
| [External Libraries](using-external-libs.md) | Using NuGet packages |
| [Debugging](debugging.md) | How to debug libraries |
| [Best Practices](best-practices.md) | Recommendations and tips |
| [Publishing](publishing.md) | How to share your library |
| [Examples](examples.md) | Ready-to-use library examples |

---

## 🔧 Requirements

- .NET SDK 10.0 or higher
- Basic knowledge of C#

---

## 📁 Library Structure

```
MyLibrary/
├── MyLibrary.cs
├── MyLibrary.csproj
└── bin/
    └── Release/
        └── net10.0/
            └── MyLibrary.dll
```

---

## 🚀 Quick Example

```csharp
using System;
using System.Collections.Generic;

namespace MyLibrary
{
    public class MyLibrary
    {
        public static string SayHello(string name)
        {
            return $"Hello, {name}!";
        }
    }
}
```

---

## 💡 How Libraries Work

Sprout libraries are regular C# class libraries (`.dll` files). They are loaded dynamically using `import` and expose methods that can be called from Sprout code.

### Naming Convention

- The class name should match the library name
- Methods should be `public` and `static`
- Method names should be descriptive and intuitive

---

**Choose a topic from the menu to get started.** 🚀
