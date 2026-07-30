---

# 🚀 Quick Start

Welcome to Sprout! This guide will help you quickly install the language and write your first program.

---

## 📦 Download

Download the latest version of Sprout from the [Releases](https://github.com/SproutLaboratory/Sprout/releases) section.

Choose the appropriate file for your system:

| Platform | Architecture | File |
|----------|--------------|------|
| Windows  | x64          | `win_x64_Sprout.exe` |
| Linux    | x64          | `linux_x64_Sprout` |

---

## 🛠️ Installation

### Windows
1. Download `win_x64_Sprout.exe`
2. Create a folder, e.g., `C:\Sprout`
3. Place `Sprout.exe` in this folder
4. (Optional) Add the folder to `PATH` to run from anywhere

### Linux
1. Download `linux_x64_Sprout`
2. Make the file executable:
   ```bash
   chmod +x linux_x64_Sprout
   ```
3. Place it in a folder, e.g., `/home/user/Sprout/`

---

## 📝 Your First Program

1. Create a file named `hello.sprout`:

```sprout
# My first program
out "Hello, World!"
out "This is Sprout!"

# Variables
name = "Alex"
age = 25
out "My name is " + name + ", I'm " + str age + " years old"

# Arithmetic
x = 10
y = 20
out "x + y = " + str (x + y)
```

2. Run it:
```bash
# Windows
Sprout.exe hello.sprout

# Linux
./Sprout hello.sprout
```

You will see:
```
Hello, World!
This is Sprout!
My name is Alex, I'm 25 years old
x + y = 30
```

---

## 🖥️ Interactive Mode

Run Sprout without arguments:

```bash
Sprout.exe
```

Now you can enter code line by line:
```sprout
>> out "Hello"
Hello
>> x = 5
>> y = 3
>> out str(x + y)
8
>> exit
```

### Command-line Options

#### `-log` — Enable detailed logging
```bash
Sprout.exe -log
```

#### `-code` — Execute code before running a script
```bash
Sprout.exe -code "a = 1; b = 2;" test.sprout
```

**test.sprout**:
```sprout
out (a + b);
```
Output:
```
3
```

---

## 🎯 What's Next?

- [Language Syntax](syntax/index.md) — detailed description of all constructs
- [Examples](examples.md) — more ready-to-use scripts
- [Creating Libraries](creating_libraries/index.md) — how to extend Sprout with C#

---

## ❓ Having Issues?

Check:
- Is `Sprout.exe` in the folder?
- Are you in the same folder in the command line?
- Does your script file have the `.sprout` extension?
---

**Happy coding with Sprout!** 🌱
