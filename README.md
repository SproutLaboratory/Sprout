<p align="center">
  <img src="https://img.icons8.com/color/96/000000/leaf.png" width="80">
</p>

<h1 align="center">🌱 Sprout v1.0.1</h1>

<p align="center">
  <strong>Интерпретируемый язык программирования на C#</strong><br>
  <em>Простой, гибкий, расширяемый</em>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/version-3.0.0-brightgreen?style=for-the-badge">
  <img src="https://img.shields.io/badge/platform-Windows-blue?style=for-the-badge">
  <img src="https://img.shields.io/badge/license-MIT-green?style=for-the-badge">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet">
</p>

---

## 📖 О языке

**Sprout** — это интерпретируемый язык программирования, написанный на **C#**.  
Он создан для быстрого написания скриптов, автоматизации задач и легкого расширения функциональности.

### 🎯 Ключевые особенности

- 🧠 **Динамическая типизация** — пишите код без объявления типов
- 📦 **Поддержка C# библиотек** — подключайте готовые DLL
- 🖥️ **Интерактивный режим** — выполняйте код построчно в консоли
- 🧩 **Встроенные модули** (в разработке):
  - Математика
  - Работа с изображениями
  - Видео и аудио конвертация
  - Архивация файлов
  - И многое другое

---

## 🛠️ Сборка из исходников

### Требования
- [.NET SDK 10.0](https://dotnet.microsoft.com/download) или выше

### Команды для сборки

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

## документация
- [Документация](docs.SUMMARY.md)
