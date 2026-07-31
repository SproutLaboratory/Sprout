[< back](global-local.md) | [next >](index.md)

---

# 📚 Standard Library

Sprout includes a built-in standard library with common functions for string manipulation, math, arrays, and dictionaries.

---

## 🔤 String Functions

| Function | Description | Example |
|----------|-------------|---------|
| `len(str)` | Returns the length of a string | `len("hello")` → `5` |
| `upper(str)` | Converts string to uppercase | `upper("hello")` → `"HELLO"` |
| `lower(str)` | Converts string to lowercase | `lower("HELLO")` → `"hello"` |
| `capitalize(str)` | Capitalizes the first letter | `capitalize("hello")` → `"Hello"` |
| `title(str)` | Capitalizes each word | `title("hello world")` → `"Hello World"` |
| `strip(str)` | Removes whitespace from both ends | `strip("  hello  ")` → `"hello"` |
| `lstrip(str)` | Removes whitespace from the left | `lstrip("  hello")` → `"hello"` |
| `rstrip(str)` | Removes whitespace from the right | `rstrip("hello  ")` → `"hello"` |
| `replace(str, old, new)` | Replaces occurrences of a substring | `replace("hello", "l", "x")` → `"hexxo"` |
| `split(str, sep)` | Splits a string into an array | `split("a,b,c", ",")` → `["a", "b", "c"]` |
| `join(sep, arr)` | Joins an array into a string | `join(", ", ["a", "b", "c"])` → `"a, b, c"` |
| `contains(str, sub)` | Checks if string contains substring | `contains("hello", "ell")` → `true` |
| `find(str, sub)` | Finds the position of a substring | `find("hello", "l")` → `2` |
| `count(str, sub)` | Counts occurrences of a substring | `count("hello", "l")` → `2` |
| `isdigit(str)` | Checks if all characters are digits | `isdigit("123")` → `true` |
| `isalpha(str)` | Checks if all characters are letters | `isalpha("abc")` → `true` |
| `isalnum(str)` | Checks if all characters are alphanumeric | `isalnum("abc123")` → `true` |
| `isspace(str)` | Checks if all characters are whitespace | `isspace("   ")` → `true` |
| `islower(str)` | Checks if all characters are lowercase | `islower("hello")` → `true` |
| `isupper(str)` | Checks if all characters are uppercase | `isupper("HELLO")` → `true` |
| `to_string(obj)` | Converts any value to a string | `to_string(42)` → `"42"` |

---

## 🧮 Math Functions

| Function | Description | Example |
|----------|-------------|---------|
| `len(arr)` | Returns the length of an array or dictionary | `len([1,2,3])` → `3` |
| `min(arr)` | Returns the minimum value in an array | `min([1,2,3])` → `1` |
| `max(arr)` | Returns the maximum value in an array | `max([1,2,3])` → `3` |
| `sum_arr(arr)` | Returns the sum of all numbers in an array | `sum_arr([1,2,3])` → `6` |
| `abs(num)` | Returns the absolute value | `abs(-5)` → `5` |
| `round(num)` | Rounds to the nearest integer | `round(3.7)` → `4` |
| `floor(num)` | Rounds down to the nearest integer | `floor(3.7)` → `3` |
| `ceil(num)` | Rounds up to the nearest integer | `ceil(3.2)` → `4` |
| `pow(base, exp)` | Raises a number to a power | `pow(2, 10)` → `1024` |
| `sqrt(num)` | Returns the square root | `sqrt(16)` → `4` |
| `sin(num)` | Returns the sine of a number (radians) | `sin(0)` → `0` |
| `cos(num)` | Returns the cosine of a number (radians) | `cos(0)` → `1` |
| `tan(num)` | Returns the tangent of a number (radians) | `tan(0)` → `0` |
| `random(min, max)` | Returns a random number between min and max | `random(1, 10)` → `7.5` |
| `random_int(min, max)` | Returns a random integer between min and max | `random_int(1, 10)` → `7` |

---

## 📦 Array Functions

| Function | Description | Example |
|----------|-------------|---------|
| `len(arr)` | Returns the length of an array | `len([1,2,3])` → `3` |
| `append(arr, value)` | Adds an element to the end of an array | `append(arr, 4)` |
| `insert(arr, index, value)` | Inserts an element at a specific index | `insert(arr, 1, 5)` |
| `pop(arr, index)` | Removes and returns an element at index | `pop(arr, 0)` |
| `remove(arr, value)` | Removes all occurrences of a value | `remove(arr, 5)` |
| `find_index(arr, value)` | Finds the index of a value | `find_index(arr, 3)` → `2` |
| `sort(arr)` | Sorts the array in place | `sort(arr)` |
| `reverse(arr)` | Reverses the array in place | `reverse(arr)` |
| `clear(arr)` | Removes all elements from the array | `clear(arr)` |

---

## 📖 Dictionary Functions

| Function | Description | Example |
|----------|-------------|---------|
| `len(dict)` | Returns the number of keys in a dictionary | `len({a:1, b:2})` → `2` |
| `keys(dict)` | Returns an array of all keys | `keys(person)` → `["name", "age"]` |
| `values(dict)` | Returns an array of all values | `values(person)` → `["Alex", 25]` |
| `items(dict)` | Returns an array of key-value pairs | `items(person)` → `[["name", "Alex"], ["age", 25]]` |
| `get(dict, key, default)` | Gets a value by key, returns default if not found | `get(person, "city", "Unknown")` |
| `set(dict, key, value)` | Sets a value by key | `set(person, "city", "New York")` |
| `has_key(dict, key)` | Checks if a key exists | `has_key(person, "name")` → `true` |
| `remove_key(dict, key)` | Removes a key from the dictionary | `remove_key(person, "age")` |
| `dict_copy(dict)` | Creates a copy of the dictionary | `dict_copy(person)` |
| `merge(dict1, dict2)` | Merges two dictionaries | `merge(person, extra)` |

---

## 🧪 Type Checking Functions

| Function | Description | Example |
|----------|-------------|---------|
| `is_string(value)` | Checks if value is a string | `is_string("hello")` → `true` |
| `is_number(value)` | Checks if value is a number | `is_number(42)` → `true` |
| `is_array(value)` | Checks if value is an array | `is_array([1,2,3])` → `true` |
| `is_dict(value)` | Checks if value is a dictionary | `is_dict({a:1})` → `true` |
| `is_bool(value)` | Checks if value is a boolean | `is_bool(true)` → `true` |
| `is_null(value)` | Checks if value is null | `is_null(null)` → `true` |

---

## 🎯 Examples

### String Manipulation

```sprout
text = "  Hello World!  "
out upper(text)          # "  HELLO WORLD!  "
out lower(text)          # "  hello world!  "
out strip(text)          # "Hello World!"
out replace(text, "World", "Sprout")  # "  Hello Sprout!  "
```

### Array Operations

```sprout
arr = [5, 2, 8, 1, 9]
sort(arr)
out str arr              # [1, 2, 5, 8, 9]
append(arr, 10)
out str arr              # [1, 2, 5, 8, 9, 10]
out str pop(arr, 0)      # 1
```

### Dictionary Operations

```sprout
person = {name = "Alex", age = 25}
keys = keys(person)
out str keys             # ["name", "age"]
out str has_key(person, "name")  # true
remove_key(person, "age")
out str person           # {name: Alex}
```

---

## 📋 Summary

Sprout's standard library provides essential functions for common operations. Use them to simplify your code and avoid writing repetitive logic.

---

[< back](global-local.md) | [next >](index.md)
