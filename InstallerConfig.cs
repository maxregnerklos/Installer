using System;
using System.Drawing;

public class InstallerConfig
{
    // Previous Rectify11 constants
    public const string PRODUCT_NAME = "Windows 12 UI Changer";
    public const string PRODUCT_VERSION = "1.0.0";
    public const string MIN_WINDOWS_VERSION = "10.0.22000.0"; // Windows 11 minimum
    public const string RECOMMENDED_WINDOWS_VERSION = "10.0.22621.0"; // Windows 11 22H2

    public static readonly Color PrimaryAccentColor = Color.FromArgb(0, 120, 212); // Windows 12 blue
    public static readonly Color SecondaryAccentColor = Color.FromArgb(32, 32, 32);

    // New Windows 12 specific configurations
    public static class UISettings
    {
        public const bool EnableCenteredTaskbar = true;
        public const bool EnableRoundedCorners = true;
        public const double DefaultCornerRadius = 8.0;
        public const bool EnableMicaEffect = true;
        
        // Additional settings
        public const string DefaultFont = "Segoe UI Variable";
        public const int DefaultFontSize = 9;
        public const int TaskbarIconSize = 24;
        public const double DefaultTransparency = 0.8;
        public const string ThemeName = "Windows12";
    }

    public static class Paths
    {
        public const string AssetsFolder = @"C:\Windows12UIChanger\Assets";
        public const string BackupFolder = @"C:\Windows12UIChanger\Backup";
        public const string LogsFolder = @"C:\Windows12UIChanger\Logs";
    }
} 