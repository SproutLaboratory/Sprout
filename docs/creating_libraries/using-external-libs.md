[< back](working-with-sproutvalue.md)

---

# 📦 External Libraries (NuGet)

This guide explains how to use NuGet packages in your Sprout libraries.

---

## 📦 Adding a NuGet Package

### Using Command Line

```bash
dotnet add package Newtonsoft.Json
```

### Using .csproj

```xml
<ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
</ItemGroup>
```

---

## 🔧 Example: Using Newtonsoft.Json

### Install the package
```bash
dotnet add package Newtonsoft.Json
```

### Use it in your library
```csharp
using Newtonsoft.Json;

public class JsonLib
{
    public static string ToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T FromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
```

---

## 🔧 Example: Using System.Text.Json

```csharp
using System.Text.Json;

public class JsonLib
{
    public static string ToJson(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T FromJson<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }
}
```

---

## 🔧 Example: Using System.IO

```csharp
using System.IO;

public class FileLib
{
    public static string Read(string path)
    {
        if (!File.Exists(path))
            return "File not found";
        return File.ReadAllText(path);
    }

    public static void Write(string path, string content)
    {
        File.WriteAllText(path, content);
    }
}
```

---

## 🔧 Example: Using System.Net.Http

```csharp
using System.Net.Http;
using System.Threading.Tasks;

public class HttpLib
{
    private static readonly HttpClient client = new HttpClient();

    public static string Get(string url)
    {
        var task = client.GetStringAsync(url);
        task.Wait();
        return task.Result;
    }

    public static string Post(string url, string data)
    {
        var content = new StringContent(data);
        var task = client.PostAsync(url, content);
        task.Wait();
        var response = task.Result;
        var readTask = response.Content.ReadAsStringAsync();
        readTask.Wait();
        return readTask.Result;
    }
}
```

---

## 📋 Summary

| Package | Purpose | Example |
|---------|---------|---------|
| `Newtonsoft.Json` | JSON serialization | `JsonConvert.SerializeObject()` |
| `System.Text.Json` | JSON serialization (built-in) | `JsonSerializer.Serialize()` |
| `System.IO` | File operations | `File.ReadAllText()` |
| `System.Net.Http` | HTTP requests | `HttpClient.GetStringAsync()` |

---

**Next: [Debugging](debugging.md)** 🚀
