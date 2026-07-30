[< back](loops.md) | [next >](arrays-and-dicts.md)

---

# 🔧 Functions

Functions in Sprout are defined using the `function` keyword. They can accept parameters, return values, and have scope control.

---

## Defining Functions

### Basic Syntax

```sprout
function name(parameters) {
    # code
}
```

### Example

```sprout
function greet(name) {
    out "Hello, " + name + "!"
}

greet("Alex")  # Hello, Alex!
```

---

## Parameters

Functions can accept multiple parameters:

```sprout
function add(a, b) {
    return send a + b
}

result = add(5, 3)
out str result  # 8
```

---

## Return Values

### `return send` — Returns a Value Immediately

```sprout
function square(x) {
    return send x * x
}

out str square(4)  # 16
```

### `return run` — Executes a Block and Returns the Result

```sprout
function calculate(a, b) {
    return run {
        result = a + b
        result = result * 2
        return send result
    }
}

out str calculate(5, 3)  # 16
```

### Difference Between `return send` and `return run`

| Keyword | Description |
|---------|-------------|
| `return send` | Returns a value immediately, stops execution |
| `return run` | Executes a block of code, then returns the result |

---

## Scope

### Local Functions (default)

Functions are **local** by default:

```sprout
function test() {
    out "Inside test"
}

test()  # Works
```

### Global Functions

Use the `global` keyword to make a function global:

```sprout
function test() global {
    out "Global function"
}

test()  # Works anywhere
```

### Explicit Local Functions

You can also explicitly mark a function as `local`:

```sprout
function test() local {
    out "Local function"
}

test()  # Works inside the same scope
```

### Function Scope Example

```sprout
function outer() {
    function inner() local {
        out "Inner function"
    }
    inner()  # ✅ Works
}

inner()  # ❌ Error: 'inner' is not defined
```

---

## 📋 Summary

| Scope | Syntax | Description |
|-------|--------|-------------|
| Local (default) | `function name(params) { ... }` | Function is local to its scope |
| Local (explicit) | `function name(params) local { ... }` | Explicitly local |
| Global | `function name(params) global { ... }` | Function is global, accessible everywhere |

---

[< back](loops.md) | [next >](arrays-and-dicts.md)
