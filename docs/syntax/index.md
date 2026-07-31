[< back](../SUMMARY.md) | [next >](comments.md)

# 📘 Sprout Syntax

Welcome to the **Sprout Syntax** documentation. This section covers everything you need to know about the Sprout programming language.

---

## 📚 What's Inside

| Topic | Description |
|-------|-------------|
| [Comments](comments.md) | Single-line and multi-line comments |
| [Variables](variables.md) | Declaring and using variables |
| [Operators](operators.md) | Arithmetic, comparison, and logical operators |
| [Output and Input](output%20and%20input.md) | `out` and `input` |
| [Conditional Statements](if%20elif%20and%20else.md) | `if`, `elif`, `else` |
| [Loops](loops.md) | `for`, `while`, `repeat`, `break` |
| [Functions](functions.md) | Defining and calling functions |
| [Arrays and Dictionaries](arrays-and-dicts.md) | Working with collections |
| [Error Handling](try.md) | `try` / `catch` |
| [Importing Libraries](import.md) | `import at` and `import` |
| [Global Variables](global-local.md) | `global()` and `local()` |
| [Standard Library](stdlib.md) | Built-in functions |

---

## 🌱 Quick Example

```sprout
# Hello World
out "Hello, Sprout!"

# Variables
name = "Alex"
age = 25

# Function
function greet(name) {
    return send "Hello, " + name + "!"
}

out greet(name)

# Array
arr = [1, 2, 3, 4, 5]
sum = 0
for i to len(arr) {
    sum = sum + arr[i]
    global(sum)
}
out "Sum: " + str sum
```

---

**Choose a topic from the menu to get started.** 🚀

[next >](comments.md)
