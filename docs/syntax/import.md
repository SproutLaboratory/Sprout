[< back](try.md) | [next >](global-local.md)

---

# 📦 Importing Libraries

Sprout allows you to import external C# libraries (`.dll` files) to extend functionality.

---

## Setting the Library Path

Use `import at` to set the folder where libraries are stored:

```sprout
import at "./lib"
```

This tells Sprout to look for `.dll` files in the `lib` folder.

---

## Importing a Library

Use `import` to load a specific library:

```sprout
import MathLib
```

This loads `MathLib.dll` from the library path.

---

## Using Library Functions

Once imported, you can call functions from the library using the library name and function name:

```sprout
out str MathLib("Add", 5, 3)    # 8
out str MathLib("Pow", 2, 10)   # 1024
```

---

## Examples

### Example 1: Math Library

```sprout
import at "./lib"
import MathLib

out str MathLib("Add", 10, 20)     # 30
out str MathLib("Sin", 0)          # 0
out str MathLib("Pi")              # 3.14159
```

### Example 2: Image Converter

```sprout
import at "./lib"
import ImageConverter

out ImageConverter("Convert", "./image.png", "./image.jpg")
out ImageConverter("GetInfo", "./image.png")
```

### Example 3: Video Converter

```sprout
import at "./lib"
import VideoConverter

out VideoConverter("Convert", "./video.mov", "./video.mp4")
out VideoConverter("GetInfo", "./video.mp4")
```

---

## 📋 Summary

| Keyword | Description |
|---------|-------------|
| `import at` | Sets the library search path |
| `import` | Loads a library by name |
| `LibraryName("FunctionName", ...)` | Calls a function from the library |

---

## ⚠️ Important Notes

1. Libraries must be compiled as `.dll` files
2. The library must be placed in the path specified by `import at`
3. The library name must match the `.dll` filename (without extension)
4. Function names are case-sensitive

---

[< back](try.md) | [next >](global-local.md)
