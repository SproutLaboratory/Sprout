[< back](if%20elif%20and%20else.md) | [next >](functions.md)

---

# 🔄 Loops

Sprout supports `for`, `while`, `repeat`, and `break` for loop control.

---

## `for` — Loop with Counter

The `for` loop iterates from 0 to a specified value.

### Syntax

```sprout
for variable to end {
    # code
}
```

### Examples

```sprout
# Loop from 0 to 5
for i to 5 {
    out str i
}
# Output: 0, 1, 2, 3, 4, 5
```

### With step

```sprout
# Loop with step 2
for i to 10, 2 {
    out str i
}
# Output: 0, 2, 4, 6, 8, 10
```

### Using with Arrays

```sprout
arr = [10, 20, 30, 40, 50]
for i to len(arr) - 1 {
    out str arr`i`
}
# Output: 10, 20, 30, 40, 50
```

### Using `len(arr)` (automatically handles last index)

```sprout
arr = [10, 20, 30, 40, 50]
for i to len(arr) {
    out str arr`i`
}
# Output: 10, 20, 30, 40, 50
```

---

## `while` — Loop with Condition

The `while` loop continues while the condition is true.

### Syntax

```sprout
while condition {
    # code
}
```

### Examples

```sprout
x = 0
while x < 5 {
    out str x
    x = x + 1
    global(x)
}
# Output: 0, 1, 2, 3, 4
```

### With Arrays

```sprout
arr = [1, 2, 3, 4, 5]
i = 0
while i < len(arr) {
    out str arr`i`
    i = i + 1
    global(i)
}
# Output: 1, 2, 3, 4, 5
```

---

## `repeat` — Loop Fixed Number of Times

The `repeat` loop executes a block a specific number of times.

### Syntax

```sprout
repeat count times variable {
    # code
}
```

### Examples

```sprout
# Repeat 5 times
repeat 5 times i {
    out str i
}
# Output: 0, 1, 2, 3, 4
```

### With Arrays

```sprout
arr = [10, 20, 30]
repeat 3 times i {
    out str arr`i`
}
# Output: 10, 20, 30
```

---

## `break` — Exit Loop

Use `break` to exit a loop early.

### Examples

```sprout
# Break when condition is met
x = 0
while true {
    out str x
    x = x + 1
    global(x)
    if x == 5 {
        break
    }
}
# Output: 0, 1, 2, 3, 4
```

### In `for` Loop

```sprout
for i to 10 {
    if i == 5 {
        break
    }
    out str i
}
# Output: 0, 1, 2, 3, 4
```

---

## 📋 Summary

| Loop | Syntax | Description |
|------|--------|-------------|
| `for` | `for i to end { ... }` | Iterates from 0 to end |
| `for` with step | `for i to end, step { ... }` | Iterates with step value |
| `while` | `while condition { ... }` | Continues while condition is true |
| `repeat` | `repeat count times i { ... }` | Repeats a fixed number of times |
| `break` | `break` | Exits the loop immediately |

---

## 🧪 Examples

### Sum of Array

```sprout
arr = [1, 2, 3, 4, 5]
sum = 0
for i to len(arr) - 1 {
    sum = sum + arr`i`
}
out "Sum: " + str sum
# Output: Sum: 15
```

### Even Numbers

```sprout
for i to 10, 2 {
    out str i
}
# Output: 0, 2, 4, 6, 8, 10
```

### Reverse Loop

```sprout
# Using while to count down
i = 5
while i >= 0 {
    out str i
    i = i - 1
    global(i)
}
# Output: 5, 4, 3, 2, 1, 0
```

---

[< back](if%20elif%20and%20else.md) | [next >](functions.md)
