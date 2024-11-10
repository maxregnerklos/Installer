using System;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;

public class Windows12UIManager
{
    private const string BACKUP_PATH = @"C:\Windows12UIChanger\Backup";
    private const string ASSETS_PATH = @"C:\Windows12UIChanger\Assets";

    public static void ApplyUIChanges(bool enableStartMenu, bool enableTaskbar, bool enableIcons)
    {
        try
        {
            CreateBackup();

            if (enableTaskbar)
            {
                ApplyTaskbarChanges();
            }

            if (enableStartMenu)
            {
                ApplyStartMenuChanges();
            }

            if (enableIcons)
            {
                ApplyIconPackChanges();
            }

            ApplySystemWideChanges();
            RestartExplorer();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error applying changes: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            RestoreBackup();
        }
    }

    private static void CreateBackup()
    {
        if (!Directory.Exists(BACKUP_PATH))
        {
            Directory.CreateDirectory(BACKUP_PATH);
        }

        // Backup registry keys
        using (var process = new System.Diagnostics.Process())
        {
            process.StartInfo.FileName = "reg";
            process.StartInfo.Arguments = $"export HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer {BACKUP_PATH}\\explorer.reg /y";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        // Backup system files
        File.Copy(@"C:\Windows\SystemResources\Windows.UI.Shell.dll", 
            Path.Combine(BACKUP_PATH, "Windows.UI.Shell.dll.backup"), true);
    }

    private static void ApplyTaskbarChanges()
    {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true))
        {
            if (key != null)
            {
                // Taskbar alignment and appearance
                key.SetValue("TaskbarAl", 1);
                key.SetValue("TaskbarCornerRadius", InstallerConfig.UISettings.DefaultCornerRadius);
                key.SetValue("TaskbarTransparency", 1);
                
                // Additional taskbar customizations
                key.SetValue("ShowTaskViewButton", 0);
                key.SetValue("TaskbarDa", 0); // Disable widgets
                key.SetValue("TaskbarMn", 0); // Disable chat
                
                // Apply Windows 12 style taskbar settings
                key.SetValue("TaskbarAnimation", 1);
                key.SetValue("TaskbarSmallIcons", 0);
                key.SetValue("EnableTranslucentBackground", 1);
            }
        }

        // Apply taskbar visual assets
        string taskbarAssetsPath = Path.Combine(ASSETS_PATH, "Taskbar");
        if (Directory.Exists(taskbarAssetsPath))
        {
            File.Copy(Path.Combine(taskbarAssetsPath, "taskbar.dll"), 
                @"C:\Windows\SystemResources\Taskbar.dll", true);
        }
    }

    private static void ApplyStartMenuChanges()
    {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true))
        {
            if (key != null)
            {
                // Start menu customizations
                key.SetValue("Start_ShowClassicMode", 0);
                key.SetValue("Start_Layout", 1); // Windows 12 style layout
                key.SetValue("Start_PowerButtonAction", 1);
                
                // Apply rounded corners
                key.SetValue("StartCornerRadius", InstallerConfig.UISettings.DefaultCornerRadius);
            }
        }

        // Replace Start menu assets
        string startMenuAssetsPath = Path.Combine(ASSETS_PATH, "StartMenu");
        if (Directory.Exists(startMenuAssetsPath))
        {
            File.Copy(Path.Combine(startMenuAssetsPath, "StartUI.dll"), 
                @"C:\Windows\SystemResources\StartUI.dll", true);
        }
    }

    private static void ApplyIconPackChanges()
    {
        string iconPackPath = Path.Combine(ASSETS_PATH, "Icons");
        if (Directory.Exists(iconPackPath))
        {
            // Update system icons
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons", true))
            {
                if (key != null)
                {
                    // Apply new system icons
                    Directory.GetFiles(iconPackPath, "*.ico").ToList().ForEach(iconFile =>
                    {
                        string iconIndex = Path.GetFileNameWithoutExtension(iconFile);
                        key.SetValue(iconIndex, iconFile);
                    });
                }
            }
        }
    }

    private static void ApplySystemWideChanges()
    {
        // Apply Mica effect
        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\DWM", true))
        {
            if (key != null)
            {
                key.SetValue("UseMica", 1);
                key.SetValue("MicaOpacity", 100);
                key.SetValue("UseAcrylic", 1);
                key.SetValue("ColorizationGlassAttribute", 1);
            }
        }

        // Apply system fonts
        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", true))
        {
            if (key != null)
            {
                key.SetValue("Segoe UI (TrueType)", "");
                key.SetValue("Segoe UI Variable (TrueType)", "SegoeUIVariable.ttf");
            }
        }

        // Apply visual styles
        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\ThemeManager", true))
        {
            if (key != null)
            {
                key.SetValue("ThemeActive", "1");
                key.SetValue("DllName", Path.Combine(ASSETS_PATH, "Themes", "Windows12.msstyles"));
            }
        }
    }

    private static void RestartExplorer()
    {
        foreach (var process in System.Diagnostics.Process.GetProcessesByName("explorer"))
        {
            process.Kill();
        }
        System.Diagnostics.Process.Start("explorer.exe");
    }

    private static void RestoreBackup()
    {
        if (Directory.Exists(BACKUP_PATH))
        {
            // Restore registry
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = "reg";
                process.StartInfo.Arguments = $"import {BACKUP_PATH}\\explorer.reg";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
            }

            // Restore system files
            if (File.Exists(Path.Combine(BACKUP_PATH, "Windows.UI.Shell.dll.backup")))
            {
                File.Copy(Path.Combine(BACKUP_PATH, "Windows.UI.Shell.dll.backup"),
                    @"C:\Windows\SystemResources\Windows.UI.Shell.dll", true);
            }
        }
    }
} 