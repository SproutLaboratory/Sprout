[< back](index.md) | [next >](variables.md)
# 💬 Comments

Sprout supports both single-line and multi-line comments.

---

## Single-line Comments

Use `#` for single-line comments:

```sprout
# This is a comment
x = 10  # This is also a comment
out "Hello"  # Comment after code
```

---

## Multi-line Comments

Use `###` for multi-line comments:

```sprout
###
This is a multi-line comment.
Everything inside will be ignored.
Great for explaining complex code.
###

x = 10
```

---

## Example

```sprout
# This program calculates the sum of an array
arr = [1, 2, 3, 4, 5]
sum = 0

###
Loop through each element
and add it to sum
###
for i to len(arr) {
    sum = sum + arr[i]
    global(sum)
}

out "Sum: " + str sum  # Print the result
```

---

## ✅ Summary

| Syntax | Type | Description |
|--------|------|-------------|
| `#` | Single-line | Comment until end of line |
| `### ... ###` | Multi-line | Everything between is ignored |

---
[next >](variables.md)
