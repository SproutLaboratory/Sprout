[< back](output%20and%20input.md) | [next >](loops.md)

---

# 🔀 Conditional Statements

Sprout supports `if`, `elif`, and `else` for conditional execution.

---

## Syntax

```sprout
if condition {
    # code to execute if condition is true
} elif condition {
    # code to execute if first condition is false and this condition is true
} else {
    # code to execute if all conditions are false
}
```

---

## 📝 Examples

### Basic If

```sprout
age = 18

if age >= 18 {
    out "You are an adult"
}
```

### If/Else

```sprout
age = 16

if age >= 18 {
    out "You are an adult"
} else {
    out "You are a minor"
}
```

### If/Elif/Else

```sprout
score = 85

if score >= 90 {
    out "Grade: A"
} elif score >= 80 {
    out "Grade: B"
} elif score >= 70 {
    out "Grade: C"
} else {
    out "Grade: F"
}
```

---

## 🔍 Conditions

Conditions can be any expression that returns a boolean value:

```sprout
x = 10
y = 20

if x < y {
    out "x is less than y"
}

if x == y {
    out "x equals y"
} elif x > y {
    out "x is greater than y"
} else {
    out "x is less than y"
}
```

### Using Logical Operators

```sprout
age = 25
has_license = true

if age >= 18 and has_license {
    out "You can drive"
}

if age >= 18 or age == 17 {
    out "Almost adult"
}

if not has_license {
    out "You need a license"
}
```

---

## 🧪 More Examples

### Nested Conditions

```sprout
age = 20
country = "USA"

if age >= 18 {
    if country == "USA" {
        out "You can vote in USA"
    } elif country == "UK" {
        out "You can vote in UK"
    } else {
        out "You are adult"
    }
} else {
    out "You are minor"
}
```

### Checking Multiple Conditions

```sprout
x = 5

if x > 0 and x < 10 {
    out "x is between 0 and 10"
} elif x >= 10 {
    out "x is 10 or greater"
} else {
    out "x is 0 or negative"
}
```

### Using with Arrays

```sprout
arr = [1, 2, 3, 4, 5]
len_arr = len(arr)

if len_arr > 0 {
    out "Array has " + str len_arr + " elements"
    out "First element: " + str arr[0]
} else {
    out "Array is empty"
}
```

---

## 📋 Summary

| Statement | Description |
|-----------|-------------|
| `if` | Executes block if condition is true |
| `elif` | Executes block if previous conditions are false and this condition is true |
| `else` | Executes block if all previous conditions are false |

---

**Important:** `elif` and `else` are optional. You can use `if` alone or with any combination of `elif` and `else`.

---


[< back](output%20and%20input.md) | [next >](loops.md)

