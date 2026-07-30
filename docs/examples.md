[< back](README.md)
# 📝 Examples

Here you will find ready-to-use Sprout scripts for various tasks.

---

## 🟢 Basic Examples

### 1. Hello World
```sprout
out "Hello, World!"
```

### 2. Variables and Math
```sprout
a = 10
b = 20
sum = a + b
out "Sum: " + str sum
```

### 3. Input from User
```sprout
name = input "What is your name?"
out "Hello, " + name + "!"
```

---

## 🔄 Loops and Conditions

### 4. For Loop
```sprout
for i to 10 {
    out str i
}
```

### 5. While Loop
```sprout
x = 0
while x < 5 {
    out str x
    x = x + 1
    global(x)
}
```

### 6. If/Elif/Else
```sprout
age = 18

if age < 18 {
    out "You are a minor"
} elif age == 18 {
    out "You are exactly 18"
} else {
    out "You are an adult"
}
```

---

## 🧮 Functions

### 7. Simple Function
```sprout
function add(a, b) {
    return send a + b
}

out str add(5, 3)  # 8
```

### 8. Function with Return Run
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

---

## 📦 Arrays and Dictionaries

### 9. Arrays
```sprout
arr = [10, 20, 30, 40, 50]
out "First element: " + str arr`0`
out "Last element: " + str arr`4`
out "Length: " + str len(arr)

for i to 4 {
    out str arr`i`
}
```

### 10. Dictionaries
```sprout
person = {name = "Alex", age = 25, city = "New York"}
out person`"name"`
out person`"age"`
```

### 11. Nested Structures
```sprout
users = [{name = "Alice", age = 30},{name = "Bob", age = 25}]

out users`0``"name"`  # Alice
```

---

## 📁 Working with Files

### 12. Import Libraries
```sprout
import at "./lib"
import MyLib

content = MyLib("Add", 1, 2)
out content
```

---

## 🧪 Error Handling

### 13. Try/Catch
```sprout
try {
    result = 10 / 0
} catch(e) {
    out "Error: " + str e
}
```

---

## 🎨 More Examples

### 14. Fibonacci
```sprout
function fib(n) {
    if n <= 1 {
        return send n
    }
    return send fib(n - 1) + fib(n - 2)
}

for i to 10 {
    out str fib(i)
}
```

### 15. Sum of Array
```sprout
arr = [1, 2, 3, 4, 5]
sum = 0
for i to len(arr) {
    sum = sum + arr`i`
    global(sum)
}
out "Sum: " + str sum
```

### 16. Factorial
```sprout
function factorial(n) {
    if n <= 1 {
        return send 1
    }
    return send n * factorial(n - 1)
}

out str factorial(5)  # 120
```

### 17. Prime Checker
```sprout
function is_prime(n) {
    if n < 2 {
        return send false
    }
    for i to n {
        if i > 1 and i < n and n % i == 0 {
            return send false
        }
    }
    return send true
}

out str is_prime(7)   # true
out str is_prime(10)  # false
```

---
**More examples coming soon!** 🌱
