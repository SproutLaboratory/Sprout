<<<<<<< HEAD
[< back](operators.md) | [next >](if%20elif%20and%20else.md)

---

# 📤 Output and Input

Sprout provides simple functions for output (`out`) and input (`input`).

---

## 📤 Output

Use `out` to print text to the console:

```sprout
out "Hello, World!"
out 42
out str (5 + 3)
```

### Combining Text and Variables

```sprout
name = "Alex"
age = 25
out "My name is " + name + ", I'm " + str age + " years old"
```

### Using `str` with Expressions

```sprout
out "Sum: " + str (5 + 3)   # "Sum: 8"
```

**Important:** When using `str` with an expression, wrap the expression in parentheses:
```sprout
out "Result: " + str (10 / 2)   # ✅ Correct
out "Result: " + str 10 / 2     # ❌ Error
```

---

## 📥 Input

Use `input` to read user input from the console:

```sprout
name = input "What is your name?"
out "Hello, " + name + "!"
```

### Input with Type Conversion

You can specify the expected type:

```sprout
# Integer
age = input "Enter your age: " int
out "You are " + str age + " years old"

# Float
height = input "Enter your height: " float
out "Your height is " + str height

# Boolean
answer = input "Are you ready? " bool
out "Ready: " + str answer
```

### Without Prompt

```sprout
out "Enter your name:"
name = input
out "Hello, " + name
```

---

## 📋 Summary

| Function | Syntax | Description |
|----------|--------|-------------|
| `out` | `out expression` | Prints to console |
| `input` | `input "prompt"` | Reads user input as string |
| `input` | `input "prompt" int` | Reads user input as integer |
| `input` | `input "prompt" float` | Reads user input as float |
| `input` | `input "prompt" bool` | Reads user input as boolean |
| `input` | `input` | Reads user input without prompt |

---

## 🧪 Examples

### Simple Calculator
```sprout
out "Simple Calculator"
a = input "Enter first number: " float
b = input "Enter second number: " float
sum = a + b
out "Sum: " + str sum
```

### Greeting
```sprout
name = input "What's your name? "
out "Hello, " + name + "!"

age = input "How old are you? " int
out "You're " + str age + " years old"
```

---

[< back](operators.md) | [next >](if%20elif%20and%20else.md)
=======
[< back](operators.md) | [next >](if%20elif%20and%20else.md)

---

# 📤 Output and Input

Sprout provides simple functions for output (`out`) and input (`input`).

---

## 📤 Output

Use `out` to print text to the console:

```sprout
out "Hello, World!"
out 42
out str (5 + 3)
```

### Combining Text and Variables

```sprout
name = "Alex"
age = 25
out "My name is " + name + ", I'm " + str age + " years old"
```

### Using `str` with Expressions

```sprout
out "Sum: " + str (5 + 3)   # "Sum: 8"
```

**Important:** When using `str` with an expression, wrap the expression in parentheses:
```sprout
out "Result: " + str (10 / 2)   # ✅ Correct
out "Result: " + str 10 / 2     # ❌ Error
```

---

## 📥 Input

Use `input` to read user input from the console:

```sprout
name = input "What is your name?"
out "Hello, " + name + "!"
```

### Input with Type Conversion

You can specify the expected type:

```sprout
# Integer
age = input "Enter your age: " int
out "You are " + str age + " years old"

# Float
height = input "Enter your height: " float
out "Your height is " + str height

# Boolean
answer = input "Are you ready? " bool
out "Ready: " + str answer
```

### Without Prompt

```sprout
out "Enter your name:"
name = input
out "Hello, " + name
```

---

## 📋 Summary

| Function | Syntax | Description |
|----------|--------|-------------|
| `out` | `out expression` | Prints to console |
| `input` | `input "prompt"` | Reads user input as string |
| `input` | `input "prompt" int` | Reads user input as integer |
| `input` | `input "prompt" float` | Reads user input as float |
| `input` | `input "prompt" bool` | Reads user input as boolean |
| `input` | `input` | Reads user input without prompt |

---

## 🧪 Examples

### Simple Calculator
```sprout
out "Simple Calculator"
a = input "Enter first number: " float
b = input "Enter second number: " float
sum = a + b
out "Sum: " + str sum
```

### Greeting
```sprout
name = input "What's your name? "
out "Hello, " + name + "!"

age = input "How old are you? " int
out "You're " + str age + " years old"
```

---

[< back](operators.md) | [next >](if%20elif%20and%20else.md)
>>>>>>> 33e63f901c8b313c46cec7001a33dd0c6d98616c
