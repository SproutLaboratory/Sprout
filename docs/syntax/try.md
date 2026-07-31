[< back](arrays-and-dicts.md) | [next >](import.md)

---

# 🧪 Error Handling

Sprout provides `try`/`catch` for handling runtime errors gracefully.

---

## Syntax

```sprout
try {
    # code that might throw an error
} catch(e) {
    # code to handle the error
}
```

---

## Examples

### Basic Try/Catch

```sprout
try {
    result = (10 / 0) # "∞"
} catch(e) {
    out ("Error: " + str e)
}
```

### Handling Missing Variables

```sprout
try {
    out x  # x is not defined
} catch(e) {
    out ("Error: " + str e)
}
```

### Handling Invalid Operations

```sprout
try {
    x = int("abc")
} catch(e) {
    out ("Failed to convert: " + str e)
}
```

---

## 📋 Summary

| Keyword | Description |
|---------|-------------|
| `try` | Wraps code that might throw an error |
| `catch(e)` | Catches the error and provides access to the error message via `e` |

---

## 🧪 Complete Example

```sprout
function safe_divide(a, b) local {
    try {
        return send a / b
    } catch(e) {
        out ("Error: " + str e)
        return send 0
    }
}

out str safe_divide(10, 2)   # "102"
out str safe_divide(10, 0)   # "∞"
out str safe_divide(10, "s")   # "Error: Оператор '/' не поддерживается для числа и строки"
```

---

[< back](arrays-and-dicts.md) | [next >](import.md)
