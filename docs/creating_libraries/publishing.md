[< back](best-practices.md)

---

# 📤 Publishing

This guide covers how to share your Sprout libraries with others.

---

## 📦 Preparing Your Library

### 1. Build in Release Mode
```bash
dotnet build -c Release
```

### 2. Copy Dependencies
```bash
dotnet publish -c Release
```

### 3. Test Your Library
Create a test script and verify everything works.

---

## 📦 Sharing Options

### Option 1: Distribute as DLL
Share the `.dll` file and documentation.

**Files to share:**
- `MyLibrary.dll`
- `README.md` (usage instructions)
- Example scripts

---

### Option 2: Publish to NuGet
Share your library as a NuGet package.

#### Step 1: Create NuGet Package
```bash
dotnet pack -c Release
```

#### Step 2: Publish to NuGet
```bash
dotnet nuget push bin/Release/*.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

---

### Option 3: Share on GitHub

Create a repository for your library:

```
MyLibrary/
├── README.md
├── LICENSE
├── MyLibrary.cs
├── MyLibrary.csproj
├── examples/
│   └── test.sprout
└── docs/
    └── usage.md
```

---

## 📝 Documentation

### Include a README
```markdown
# MyLibrary

## Installation
Copy `MyLibrary.dll` to your `lib/` folder.

## Usage
```sprout
import at "./lib"
import MyLibrary
out MyLibrary("Hello", "World")
```

## Methods

### Hello(name)
Returns a greeting.

### Add(a, b)
Returns the sum of two numbers.

## License
MIT
```

---

## 🧪 Example Project

```
MyLibrary/
├── MyLibrary.cs
├── MyLibrary.csproj
├── README.md
├── LICENSE
├── examples/
│   ├── test.sprout
│   └── demo.sprout
└── docs/
    ├── usage.md
    └── api.md
```

---

## 📋 Checklist

Before publishing, make sure:

- ✅ Library builds without errors
- ✅ All methods are tested
- ✅ Documentation is complete
- ✅ License is included
- ✅ Example scripts are provided
- ✅ README explains how to use it

---

## 💡 Pro Tips

- **Version your library**: Use semantic versioning (v1.0.0, v1.1.0, v2.0.0)
- **Keep it up to date**: Update your library as Sprout evolves
- **Listen to feedback**: Improve based on user input
- **Share examples**: Show how to use your library effectively

---

## 🔗 Useful Links

- [NuGet Publishing Guide](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package)
- [GitHub Repository Guide](https://docs.github.com/en/repositories/creating-and-managing-repositories)
- [Semantic Versioning](https://semver.org/)

---

## 📋 Summary

| Method | Steps |
|--------|-------|
| Direct DLL | Build, copy, share |
| NuGet | Create package, push to NuGet |
| GitHub | Create repository, share link |

---

**Next: [Examples](examples.md)** 🚀
