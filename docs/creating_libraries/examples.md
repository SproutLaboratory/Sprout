[< back](index.md)

---

# 📝 Library Examples

This page provides ready-to-use library examples for Sprout.

---

## 🔧 Math Library

### Code
```csharp
using System;
using System.Collections.Generic;

public class MathLib
{
    public static double Add(double a, double b) => a + b;
    public static double Sub(double a, double b) => a - b;
    public static double Mul(double a, double b) => a * b;
    public static double Div(double a, double b) => b != 0 ? a / b : throw new Exception("Division by zero");
    public static double Pow(double a, double b) => Math.Pow(a, b);
    public static double Sqrt(double a) => Math.Sqrt(a);
    public static double Sin(double a) => Math.Sin(a);
    public static double Cos(double a) => Math.Cos(a);
    public static double Tan(double a) => Math.Tan(a);
    public static double Pi() => Math.PI;
    public static double E() => Math.E;
}
```

### Usage
```sprout
import at "./lib"
import MathLib

out str MathLib("Add", 5, 3)   # 8
out str MathLib("Pi")          # 3.14159
out str MathLib("Sin", 0)      # 0
```

---

## 📁 File Library

### Code
```csharp
using System;
using System.IO;
using System.Collections.Generic;

public class FileLib
{
    public static string Read(string path)
    {
        if (!File.Exists(path))
            return $"File not found: {path}";
        return File.ReadAllText(path);
    }

    public static string Write(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
            return $"File written: {path}";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public static string Append(string path, string content)
    {
        try
        {
            File.AppendAllText(path, content);
            return $"Content appended: {path}";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public static string List(string path)
    {
        if (!Directory.Exists(path))
            return $"Directory not found: {path}";
        
        var files = Directory.GetFiles(path);
        return string.Join("\n", files);
    }
}
```

### Usage
```sprout
import at "./lib"
import FileLib

out FileLib("Write", "test.txt", "Hello World")
out FileLib("Read", "test.txt")
out FileLib("List", "./")
```

---

## 🔤 String Library

### Code
```csharp
using System;
using System.Collections.Generic;

public class StringLib
{
    public static string Reverse(string text)
    {
        char[] chars = text.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    public static string ToUpper(string text) => text.ToUpper();
    public static string ToLower(string text) => text.ToLower();
    public static int Length(string text) => text.Length;
    
    public static string Join(List<string> strings, string separator = " ")
    {
        return string.Join(separator, strings);
    }

    public static List<string> Split(string text, string separator)
    {
        var result = new List<string>();
        foreach (var part in text.Split(new[] { separator }, StringSplitOptions.None))
            result.Add(part);
        return result;
    }
}
```

### Usage
```sprout
import at "./lib"
import StringLib

out StringLib("Reverse", "hello")      # olleh
out StringLib("Join", ["a", "b"], ",") # a,b
out StringLib("Length", "hello")       # 5
```

---

## 🌐 HTTP Library

### Code
```csharp
using System.Net.Http;
using System.Threading.Tasks;

public class HttpLib
{
    private static readonly HttpClient client = new HttpClient();

    public static string Get(string url)
    {
        try
        {
            var task = client.GetStringAsync(url);
            task.Wait();
            return task.Result;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public static string Post(string url, string data)
    {
        try
        {
            var content = new StringContent(data);
            var task = client.PostAsync(url, content);
            task.Wait();
            var response = task.Result;
            var readTask = response.Content.ReadAsStringAsync();
            readTask.Wait();
            return readTask.Result;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}
```

### Usage
```sprout
import at "./lib"
import HttpLib

out HttpLib("Get", "https://api.example.com/data")
out HttpLib("Post", "https://api.example.com/submit", "{\"key\":\"value\"}")
```

---

## 📊 CSV Library

### Code
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class CsvLib
{
    public static string ToCsv(List<List<string>> data, string separator = ",")
    {
        var lines = new List<string>();
        foreach (var row in data)
        {
            lines.Add(string.Join(separator, row));
        }
        return string.Join("\n", lines);
    }

    public static List<List<string>> FromCsv(string text, string separator = ",")
    {
        var result = new List<List<string>>();
        var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var row = new List<string>();
            foreach (var cell in line.Split(new[] { separator }, StringSplitOptions.None))
                row.Add(cell);
            result.Add(row);
        }
        return result;
    }
}
```

### Usage
```sprout
import at "./lib"
import CsvLib

data = [["Name", "Age"], ["Alex", "25"], ["Bob", "30"]]
csv = CsvLib("ToCsv", data)
out csv
```

---

## 📋 Summary

| Library | Purpose | Key Methods |
|---------|---------|-------------|
| MathLib | Mathematics | Add, Sub, Mul, Div, Pow, Sqrt |
| FileLib | File operations | Read, Write, Append, List |
| StringLib | String manipulation | Reverse, Upper, Lower, Join, Split |
| HttpLib | HTTP requests | Get, Post |
| CsvLib | CSV handling | ToCsv, FromCsv |

---

**Choose a library that fits your needs and start using it today!** 🚀
