[< back](functions.md) | [next >](try.md)

---

# 📦 Arrays and Dictionaries

Sprout supports **arrays** (ordered lists) and **dictionaries** (key-value pairs).

---

## Arrays

### Creating Arrays

```sprout
arr = [10, 20, 30, 40, 50]
mixed = [1, "two", 3.0, true]
empty = []
```

### Accessing Elements

Use square brackets `[]` to access elements by index:

```sprout
arr = [10, 20, 30, 40, 50]
out str arr[0]   # 10
out str arr[4]   # 50
```

### Modifying Elements

```sprout
arr = [10, 20, 30, 40, 50]
arr[0] = 100
out str arr[0]   # 100
```

### Array Length

Use `len()` to get the length:

```sprout
arr = [10, 20, 30, 40, 50]
out "Length: " + str len(arr)  # 5
```

### Iterating Over Arrays

```sprout
arr = [10, 20, 30, 40, 50]
for i to len(arr) {
    out str arr[i]
}
# Output: 10, 20, 30, 40, 50
```

---

## Dictionaries

### Creating Dictionaries

```sprout
person = {name = "Alex", age = 25, city = "New York"}
empty = {}
```

### Accessing Values

Use square brackets `[]` with string keys:

```sprout
person = {name = "Alex", age = 25}
out person["name"]   # "Alex"
out person["age"]    # 25
```

### Modifying Values

```sprout
person = {name = "Alex", age = 25}
person["age"] = 26
out str person["age"]   # 26
```

### Adding New Keys

```sprout
person = {name = "Alex"}
person["country"] = "USA"
out person["country"]   # "USA"
```

### Iterating Over Dictionaries

```sprout
person = {name = "Alex", age = 25, city = "New York"}
keys = ["name", "age", "city"]  # Храним ключи в массиве

for i to len(keys) {
    key = keys[i]
    out (key + ": " + person[key])
}
```

---

## Nested Structures

### Arrays in Arrays

```sprout
matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]

out str matrix[0][0]   # 1
out str matrix[1][2]   # 6
```

### Arrays in Dictionaries

```sprout
person = {name = "Alex", hobbies = ["reading", "coding", "gaming"]}

out person["hobbies"][0]   # "reading"
```

### Dictionaries in Arrays

```sprout
users = [{name = "Alice", age = 30}, {name = "Bob", age = 25}]

out users[0]["name"]   # "Alice"
out users[1]["age"]    # 25
```

### Dictionaries in Dictionaries

```sprout
person = {name = "Alex", address = {city = "New York", zip = 10001}}

out person["address"]["city"]   # "New York"
```

### Combined Nested Structures

```sprout
data = {users = [{name = "Alice", score = 85}, {name = "Bob", score = 92}]}

sum = 0
for i to len(data["users"]) {
    sum = (sum + data["users"][i]["score"])
}
out ("Average score: " + str (sum / len(data["users"])))
```

---

## 🧪 Examples

### Sum of Array

```sprout
arr = [1, 2, 3, 4, 5]
sum = 0
for i to len(arr) {
    sum = (sum + arr[i])
}
out ("Sum: " + str sum)
# Output: 15
```

### Dictionary of User Info

```sprout
user = {username = "alex123", email = "alex@example.com", age = 25}

out ("Username: " + user["username"])
out ("Email: " + user["email"])
```

---

## 📋 Summary

| Type | Syntax | Access | Modify |
|------|--------|--------|--------|
| Array | `[1, 2, 3]` | `arr[i]` | `arr[i] = value` |
| Dictionary | `{key = value}` | `dict["key"]` | `dict["key"] = value` |

### Important Notes

- Array indices start at **0**
- Dictionary keys are **strings**
- Use square brackets `[]` for access
- Nested structures are fully supported

---

[< back](functions.md) | [next >](try.md)
