[< back](import.md) | [next >](stdlib.md)

---

# 🌐 Global and Local Variables

Sprout provides `global()` and `local()` functions to control variable scope.

---

## Default Behavior

By default, variables inside functions and loops are **local**:

```sprout
function test() {
    x = 10          # Local variable
    out str x       # 10
}
test()
out str x           # Error: 'x' is not defined
```

---

## `global()` — Make a Variable Global

Use `global(variable)` to make a variable accessible outside the current scope:

```sprout
function test() {
    x = 10
    global(x)       # Now x is global
}
test()
out str x           # 10
```

### Inside Loops

```sprout
for i to 5 {
    x = i
    global(x)       # Make x global
}
out str x           # 4
```

---

## `local()` — Make a Variable Local

Use `local(variable)` to make a global variable local:

```sprout
x = 10              # Global variable

function test() {
    local(x)        # Make x local
    x = 20
    out str x       # 20
}
test()
out str x           # 10 (global value unchanged)
```

---

## Scope Rules

| Scope | Default | How to change |
|-------|---------|---------------|
| Function | Local | `global()` to make global |
| Loop | Local | `global()` to make global |
| Top-level | Global | `local()` to make local |

---

## Examples

### Global in Function

```sprout
counter = 0

function increment() {
    counter = (counter + 1)
    global(counter)
}

increment()
increment()
out str counter      # 2
```

### Local in Top-Level

```sprout
x = 10               # Global by default
local(x)             # Make it local (only visible in this scope)

function test() {
    out str x        # Error: 'x' is not defined here
}
```

---

## 📋 Summary

| Function | Description |
|----------|-------------|
| `global(variable)` | Makes a variable global |
| `local(variable)` | Makes a variable local |

---

**Note:** `global()` and `local()` only affect variables in the current scope. Use them carefully to avoid confusion.

---

[< back](import.md) | [next >](stdlib.md)
