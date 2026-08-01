[< back](index.md)

---

# 🚀 Quick Start — Creating Your First Library

This guide will walk you through creating your first Sprout library step by step.

---

## 📁 Step 1: Create a C# Class Library Project

```bash
dotnet new classlib -f net10.0 -n MyLibrary
cd MyLibrary
```

This creates a new class library project targeting .NET 10.0.

---

## 📝 Step 2: Write Your First Code

Open `MyLibrary.cs` and replace the content:

```csharp
using System;
using System.Collections.Generic;

namespace MyLibrary
{
    public class MyLibrary
    {
        public static string Hello(string name)
        {
            return $"Hello, {name}!";
        }

        public static int Add(int a, int b)
        {
            return a + b;
        }
    }
}
```

---

## 🛠️ Step 3: Build the Library

```bash
dotnet build -c Release
```

Your library will be created at:
```
bin/Release/net10.0/MyLibrary.dll
```

---

## 📦 Step 4: Use It in Sprout

### 4.1 Copy the DLL to your `lib` folder

```bash
copy bin\Release\net10.0\MyLibrary.dll C:\path\to\sprout\lib\
```

### 4.2 Create a test script

Create `test.sprout`:

```sprout
import at "./lib"
import MyLibrary

out MyLibrary("Hello", "Alex")
out str MyLibrary("Add", 5, 3)
```

### 4.3 Run the script

```bash
Sprout.exe test.sprout
```

You should see:
```
Hello, Alex!
8
```

---

## 📋 Step 5: Add More Methods

```csharp
public static string Concat(List<string> strings)
{
    return string.Join("", strings);
}

public static bool IsEven(int n)
{
    return n % 2 == 0;
}
```

---

## 🧪 Complete Example

**MyLibrary.cs:**
```csharp
using System;
using System.Collections.Generic;

namespace MyLibrary
{
    public class MyLibrary
    {
        public static string Hello(string name)
        {
            return $"Hello, {name}!";
        }

        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static string Concat(List<string> strings)
        {
            return string.Join("", strings);
        }

        public static bool IsEven(int n)
        {
            return n % 2 == 0;
        }
    }
}
```

**test.sprout:**
```sprout
import at "./lib"
import MyLibrary

out MyLibrary("Hello", "World")
out str MyLibrary("Add", 10, 20)
out MyLibrary("Concat", "Hello", " ", "World")
out str MyLibrary("IsEven", 4)  # true
out str MyLibrary("IsEven", 5)  # false
```

---

## 🎯 What's Next?

- [C# Basics](csharp-basics.md) — learn more about C# for Sprout
- [Creating Methods](creating-methods.md) — understand method signatures
- [SproutValue](working-with-sproutvalue.md) — work with dynamic types

---

**Your first library is ready!** 🎉
