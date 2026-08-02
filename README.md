<p align="center">
  <img src="https://img.icons8.com/color/96/000000/leaf.png" width="80">
</p>

<h1 align="center">🌱 Sprout v1.1.0</h1>

<p align="center">
  <strong>Interpreted programming language in C#</strong><br>
  <em>Simple, flexible, extensible</em>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/version-1.1.0-brightgreen?style=for-the-badge">
  <img src="https://img.shields.io/badge/platform-Windows-blue?style=for-the-badge">
  <img src="https://img.shields.io/badge/license-MIT-green?style=for-the-badge">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet">
</p>

---

## 📖 About the Language

**Sprout** is an interpreted programming language written in **C#**.  
It is designed for quick scripting, task automation, and easy extensibility.

### 🎯 Key Features

- 🧠 **Dynamic typing** — write code without declaring types
- 📦 **C# library support** — plug in ready-made DLLs
- 🖥️ **Interactive mode** — execute code line by line in the console
- 🧩 **Built-in modules** (in development):
  - Mathematics
  - Image processing
  - Video and audio conversion
  - File archiving
  - And much more

---

## 🛠️ Building from Source

### Requirements
- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or higher

### Build Commands

# Windows x64
```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

# Windows x86
```bash
dotnet publish -c Release -r win-x86 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

# Windows ARM64
```bash
dotnet publish -c Release -r win-arm64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

# Linux x64
```bash
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

# Linux ARM64
```bash
dotnet publish -c Release -r linux-arm64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

## Documentation
- [Documentation](docs/SUMMARY.md)
