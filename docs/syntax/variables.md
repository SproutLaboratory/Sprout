[< back](comments.md) | [next >](operators.md)

---

# 📦 Variables

Sprout uses **dynamic typing** — you don't need to declare variable types. The type is determined automatically based on the value.

---

## Declaring Variables

```sprout
x = 10           # Number
name = "Sprout"  # String
is_ready = true  # Boolean
arr = [1, 2, 3]  # Array
dict = {a = 1, b = 2}  # Dictionary
```

---

## Changing Values

You can change a variable's value at any time:

```sprout
x = 10
x = "Now I'm a string"
x = [1, 2, 3]
```

---

## Naming Rules

- Letters (a-z, A-Z)
- Numbers (0-9) — but not as the first character
- Underscore (`_`)
- Case-sensitive (`x` and `X` are different)

**Valid names:**
```sprout
name
user_name
_name
user1
x
```

**Invalid names:**
```sprout
1user    # Can't start with a number
my-name  # Hyphens are not allowed
@name    # Special characters are not allowed
```

---

## Scope

### Local Scope (default)
Variables declared inside functions and loops are **local** by default:

```sprout
function test() {
    x = 10      # Local variable
    out str x   # 10
}
out str x       # Error: 'x' is not defined
```

### Global Scope
To make a variable global, use the `global()` function:

```sprout
function test() {
    x = 10
    global(x)   # Now x is global
}
test()
out str x       # 10
```

### Local Scope (inside loops)
Variables inside loops are local by default:

```sprout
for i to 5 {
    x = i
}
out str x       # Error: 'x' is not defined
```

To make them global, use `global(x)` inside the loop.

---

## Examples

### Basic Usage
```sprout
name = "Alex"
age = 25
out "My name is " + name + ", I'm " + str age + " years old"
```

### Reassigning
```sprout
value = 10
out str value   # 10
value = "Hello"
out value       # Hello
```

### Working with Arrays
```sprout
arr = [1, 2, 3, 4, 5]
arr[0] = 100
out str arr[0]  # 100
```

### Working with Dictionaries
```sprout
person = {name = "Alex", age = 25}
person[age] = 26
out str person[age]  # 26
```

---

## 🔄 Type Conversion

| Function | Syntax | Description | Example |
|----------|--------|-------------|---------|
| `str` | `str(x)` or `str x` | Convert to string | `str 42` → `"42"` |
| `int` | `int(x)` or `int x` | Convert to integer | `int 3.14` → `3` |
| `float` | `float(x)` or `float x` | Convert to float | `float "3"` → `3.0` |
| `bool` | `bool(x)` or `bool x` | Convert to boolean | `bool 1` → `true` |

Both syntaxes are valid:
```sprout
out str 42       # "42"
out str(42)      # "42"
x = int 3.14     # 3
y = int(3.14)    # 3
```

---

[next >](operators.md)
