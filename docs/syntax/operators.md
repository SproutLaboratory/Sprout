[< back](variables.md) | [next >](output%20and%20input.md)

---

# ➕ Operators

Sprout supports arithmetic, comparison, logical, and special operators.

---

## 📊 Arithmetic Operators

| Operator | Description | Example |
|----------|-------------|---------|
| `+` | Addition | `(5 + 3)` → `8` |
| `-` | Subtraction | `(10 - 4)` → `6` |
| `*` | Multiplication | `(6 * 7)` → `42` |
| `/` | Division | `(15 / 3)` → `5` |
| `%` | Modulo (remainder) | `(10 % 3)` → `1` |
| `**` | Exponentiation | `(2 ** 10)` → `1024` |

### Examples
```sprout
x = (5 + 3)
y = (10 - 4)
z = (6 * 7)
div = (15 / 3)
mod = (10 % 3)
pow = (2 ** 10)

out str x   # 8
out str pow # 1024
```

**Important:** All arithmetic operations must be wrapped in parentheses `()`.

---

## 🔍 Comparison Operators

| Operator | Description | Example |
|----------|-------------|---------|
| `==` | Equal (with conversion) | `(5 == 5)` → `true` |
| `!=` | Not equal | `(5 != 3)` → `true` |
| `>` | Greater than | `(10 > 5)` → `true` |
| `<` | Less than | `(5 < 10)` → `true` |
| `>=` | Greater than or equal | `(10 >= 10)` → `true` |
| `<=` | Less than or equal | `(5 <= 10)` → `true` |
| `?=` | Strict equal | `(5 ?= 5)` → `true`, `("5" ?= 5)` → `false` |

### Examples
```sprout
x = 10
y = 20

out str (x == y)   # false
out str (x != y)   # true
out str (x < y)    # true
out str (x >= 10)  # true
```

---

## 🧠 Logical Operators

| Operator | Description | Example |
|----------|-------------|---------|
| `and` | Logical AND | `(true and false)` → `false` |
| `or` | Logical OR | `(true or false)` → `true` |
| `not` | Logical NOT | `(not true)` → `false` |

### Examples
```sprout
a = true
b = false

out str (a and b)   # false
out str (a or b)    # true
out str (not a)     # false
```

---

## 🎯 Special Operators

### Negation (`-`)
Unary negation for numbers:
```sprout
x = 5
y = -x  # -5
out str y
```

### Index (`[]`)
Access elements in arrays, dictionaries, and strings:
```sprout
arr = [10, 20, 30]
out str arr[0]   # 10

dict = {name = "Alex"}
out dict["name"]   # "Alex"

text = "Hello"
out text[0]      # "H"
```

### String Multiplication (`*`)
Repeat a string:
```sprout
s = ("abc" * 3)   # "abcabcabc"
out s
```

### String Concatenation (`+`)
Join strings:
```sprout
out ("Hello" + " " + "World")  # "Hello World"
```

---

## 📋 Operator Precedence

From highest to lowest:

| Precedence | Operators |
|------------|-----------|
| 1 | `**` |
| 2 | `-` (unary), `not` |
| 3 | `*`, `/`, `%` |
| 4 | `+`, `-` |
| 5 | `==`, `!=`, `>`, `<`, `>=`, `<=`, `?=`, `??` |
| 6 | `and` |
| 7 | `or` |

### Example
```sprout
result = (2 + (3 * (4 ** 2)))
# 2 + (3 * 16)
# 2 + 48
# 50
```

---

## 🧪 Type Coercion

Sprout automatically converts types when needed:

```sprout
out ("Number: " + str 42)    # "Number: 42"
out ("Pi: " + str 3.14)      # "Pi: 3.14"
```

### Important: Using `str` with expressions
```sprout
out ("Sum: " + str (5 + 3))   # ✅ "Sum: 8"
out ("Sum: " + str 5 + 3)     # ❌ "Sum: 5" + 3 → error
```

---

[next >](output%20and%20input.md)
