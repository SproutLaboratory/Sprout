using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using Microsoft.Win32;

namespace SproutInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // ===== РЕГИСТРАЦИЯ РАСШИРЕНИЯ .sprout (КРОССПЛАТФОРМЕННО) =====
            RegisterFileType();

            // ===== УСТАНАВЛИВАЕМ РАБОЧИЙ КАТАЛОГ В ПАПКУ С ПРОГРАММОЙ =====
            string exeDir = AppContext.BaseDirectory;
            if (!string.IsNullOrEmpty(exeDir) && Directory.Exists(exeDir))
            {
                Directory.SetCurrentDirectory(exeDir);
                Console.WriteLine($"📁 Working directory: {exeDir}");
            }

            bool enableLogging = args.Contains("-log");
            string codeParam = null;
            string scriptFile = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-log") continue;
                else if (args[i] == "-code" && i + 1 < args.Length)
                {
                    codeParam = args[i + 1];
                    i++;
                }
                else if (!args[i].StartsWith("-"))
                {
                    scriptFile = args[i];
                }
            }

            var interpreter = new Interpreter();
            if (enableLogging)
            {
                interpreter.EnableLogging();
                Console.WriteLine("📋 LOGGING ENABLED");
            }

            if (!string.IsNullOrEmpty(codeParam))
            {
                try
                {
                    if (enableLogging) Console.WriteLine($"📝 Executing preliminary code (-code):");
                    interpreter.Execute(codeParam);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error in preliminary code: {ex.Message}");
                    if (enableLogging) Console.WriteLine($"StackTrace: {ex.StackTrace}");
                    return;
                }
            }

            if (!string.IsNullOrEmpty(scriptFile))
            {
                // Make absolute path
                string filePath = scriptFile;
                if (!Path.IsPathRooted(filePath))
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                }
                filePath = Path.GetFullPath(filePath);

                // Change working directory to script folder
                string scriptDir = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(scriptDir) && Directory.Exists(scriptDir))
                {
                    Directory.SetCurrentDirectory(scriptDir);
                    Console.WriteLine($"📁 Script directory: {scriptDir}");
                }

                if (File.Exists(filePath))
                {
                    try
                    {
                        string code = File.ReadAllText(filePath);
                        Console.WriteLine($"🌱 Executing script: {Path.GetFileName(filePath)}");
                        Console.WriteLine(new string('-', 40));
                        var result = interpreter.Execute(code);
                        Console.WriteLine(new string('-', 40));
                        if (result != null && result.Type != SproutValue.ValueType.Null)
                            Console.WriteLine("=> " + result.ToString());
                        Console.WriteLine($"✅ Script executed");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error: {ex.Message}");
                        if (enableLogging) Console.WriteLine($"StackTrace: {ex.StackTrace}");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"❌ File not found: {filePath}");
                    return;
                }
            }

            Console.WriteLine("███████╗██████╗ ██████╗  ██████╗ ██╗   ██╗████████╗");
            Console.WriteLine("██╔════╝██╔══██╗██╔══██╗██╔═══██╗██║   ██║╚══██╔══╝");
            Console.WriteLine("███████╗██████╔╝██████╔╝██║   ██║██║   ██║   ██║");
            Console.WriteLine("╚════██║██╔═══╝ ██╔══██╗██║   ██║██║   ██║   ██║");
            Console.WriteLine("███████║██║     ██║  ██║╚██████╔╝╚██████╔╝   ██║");
            Console.WriteLine("╚══════╝╚═╝     ╚═╝  ╚═╝ ╚═════╝  ╚═════╝    ╚═╝");
            Console.WriteLine("🌱 Sprout Interpreter v3.0");
            Console.WriteLine("Type 'exit' to quit");

            while (true)
            {
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                if (input == "exit") break;

                try
                {
                    var result = interpreter.Execute(input);
                    if (result != null && result.Type != SproutValue.ValueType.Null)
                        Console.WriteLine("=> " + result.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // ===== КРОССПЛАТФОРМЕННАЯ РЕГИСТРАЦИЯ РАСШИРЕНИЯ .sprout =====
        static void RegisterFileType()
        {
            try
            {
                string extension = ".sprout";
                string progId = "SproutScript";
                string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                if (OperatingSystem.IsWindows())
                {
                    RegisterWindows(extension, progId, exePath);
                }
                else if (OperatingSystem.IsLinux())
                {
                    RegisterLinux(extension, progId, exePath);
                }
                else
                {
                    Console.WriteLine("⚠️ Unsupported OS. Skipping file registration.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Failed to register file type: {ex.Message}");
            }
        }

        // ===== РЕГИСТРАЦИЯ ДЛЯ WINDOWS =====
        private static void RegisterWindows(string extension, string progId, string exePath)
        {
            try
            {
                // Проверяем, зарегистрировано ли расширение
                using (var key = Registry.ClassesRoot.OpenSubKey(extension))
                {
                    if (key != null && key.GetValue(null) as string == progId)
                    {
                        Console.WriteLine($"✅ Extension {extension} already registered in Windows.");
                        return;
                    }
                }

                // Проверяем права администратора
                bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
                    .IsInRole(WindowsBuiltInRole.Administrator);

                if (!isAdmin)
                {
                    Console.WriteLine("⚠️ If you want .sprout files to show custom icons,");
                    Console.WriteLine("   please run the program as Administrator.");
                    Console.WriteLine();
                    return;
                }

                // 1. Create ProgID
                using (var key = Registry.ClassesRoot.CreateSubKey(progId))
                {
                    key.SetValue(null, "Sprout Script");
                }

                // 2. Set icon (taken from EXE)
                using (var key = Registry.ClassesRoot.CreateSubKey(progId + "\\DefaultIcon"))
                {
                    key.SetValue(null, $"\"{exePath}\",0");
                }

                // 3. Register extension
                using (var key = Registry.ClassesRoot.CreateSubKey(extension))
                {
                    key.SetValue(null, progId);
                }

                // 4. Double-click opens file in Sprout
                using (var key = Registry.ClassesRoot.CreateSubKey(progId + "\\Shell\\Open\\Command"))
                {
                    key.SetValue(null, $"\"{exePath}\" \"%1\"");
                }

                Console.WriteLine($"✅ Extension {extension} registered in Windows.");
                Console.WriteLine($"   Icon: {exePath},0");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Failed to register in Windows: {ex.Message}");
            }
        }

        // ===== РЕГИСТРАЦИЯ ДЛЯ LINUX (через XDG MIME) =====
        private static void RegisterLinux(string extension, string progId, string exePath)
        {
            try
            {
                string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string localApps = Path.Combine(home, ".local", "share", "applications");
                string mimePackages = Path.Combine(home, ".local", "share", "mime", "packages");
                string icons = Path.Combine(home, ".local", "share", "icons", "hicolor", "256x256", "mimetypes");

                Directory.CreateDirectory(localApps);
                Directory.CreateDirectory(mimePackages);
                Directory.CreateDirectory(icons);

                // 1. Create .desktop file
                string desktopFile = Path.Combine(localApps, "sprout.desktop");
                string desktopContent = $@"[Desktop Entry]
Type=Application
Name=Sprout
Comment=Sprout Programming Language
Exec={exePath} %f
Icon=sprout
Terminal=true
MimeType=text/x-sprout;
Categories=Development;Programming;
";
                File.WriteAllText(desktopFile, desktopContent);

                // 2. Create MIME type definition
                string mimeFile = Path.Combine(mimePackages, "text-x-sprout.xml");
                string mimeContent = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<mime-info xmlns=""http://www.freedesktop.org/standards/shared-mime-info"">
  <mime-type type=""text/x-sprout"">
    <comment>Sprout source file</comment>
    <glob pattern=""*.sprout""/>
  </mime-type>
</mime-info>";
                File.WriteAllText(mimeFile, mimeContent);

                // 3. Create icon symlink (if icon file exists)
                string iconSource = Path.Combine(AppContext.BaseDirectory, "icon.png");
                string iconDest = Path.Combine(icons, "text-x-sprout.png");
                if (File.Exists(iconSource))
                {
                    File.Copy(iconSource, iconDest, true);
                }

                // 4. Update MIME database
                Console.WriteLine("🔄 Updating MIME database...");
                RunProcess("update-mime-database", home + "/.local/share/mime");

                // 5. Update desktop database
                Console.WriteLine("🔄 Updating desktop database...");
                RunProcess("update-desktop-database", home + "/.local/share/applications");

                Console.WriteLine($"✅ Extension {extension} registered in Linux.");
                Console.WriteLine($"   Desktop file: {desktopFile}");
                Console.WriteLine($"   MIME file: {mimeFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Failed to register in Linux: {ex.Message}");
            }
        }

        // ===== ВСПОМОГАТЕЛЬНЫЙ МЕТОД ДЛЯ ЗАПУСКА ПРОЦЕССОВ =====
        private static void RunProcess(string command, string arguments)
        {
            try
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = command,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Command '{command}' failed: {ex.Message}");
            }
        }
    }
}