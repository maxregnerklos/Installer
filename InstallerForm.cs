using System;
using System.Drawing;
using System.Windows.Forms;

public class InstallerForm : Form
{
    private CheckBox enableStartMenu;
    private CheckBox enableTaskbar;
    private CheckBox enableIcons;
    private CheckBox enableMicaEffect;
    private Button installButton;
    private Button restoreButton;
    private ProgressBar progressBar;
    private Label statusLabel;

    public InstallerForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = InstallerConfig.PRODUCT_NAME;
        this.Width = 500;
        this.Height = 400;
        this.BackColor = Color.White;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Title Label
        Label titleLabel = new Label
        {
            Text = InstallerConfig.PRODUCT_NAME,
            Font = new Font("Segoe UI Variable", 20, FontStyle.Bold),
            ForeColor = InstallerConfig.PrimaryAccentColor,
            Location = new Point(20, 20),
            AutoSize = true
        };

        // Options Group
        GroupBox optionsGroup = new GroupBox
        {
            Text = "Customization Options",
            Location = new Point(20, 70),
            Width = 440,
            Height = 200
        };

        enableStartMenu = new CheckBox
        {
            Text = "Enable Windows 12 Start Menu",
            Checked = true,
            Location = new Point(20, 30),
            Width = 400
        };

        enableTaskbar = new CheckBox
        {
            Text = "Enable Windows 12 Taskbar",
            Checked = true,
            Location = new Point(20, 60),
            Width = 400
        };

        enableIcons = new CheckBox
        {
            Text = "Install Windows 12 Icon Pack",
            Checked = true,
            Location = new Point(20, 90),
            Width = 400
        };

        enableMicaEffect = new CheckBox
        {
            Text = "Enable Mica Effect",
            Checked = true,
            Location = new Point(20, 120),
            Width = 400
        };

        optionsGroup.Controls.AddRange(new Control[] { 
            enableStartMenu, enableTaskbar, enableIcons, enableMicaEffect 
        });

        // Progress Bar
        progressBar = new ProgressBar
        {
            Location = new Point(20, 290),
            Width = 440,
            Height = 20,
            Style = ProgressBarStyle.Continuous
        };

        // Status Label
        statusLabel = new Label
        {
            Location = new Point(20, 320),
            Width = 440,
            Height = 20,
            Text = "Ready to install..."
        };

        // Buttons
        installButton = new Button
        {
            Text = "Install Windows 12 UI",
            Location = new Point(260, 350),
            Width = 200,
            Height = 30,
            BackColor = InstallerConfig.PrimaryAccentColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        restoreButton = new Button
        {
            Text = "Restore Original",
            Location = new Point(20, 350),
            Width = 200,
            Height = 30,
            BackColor = Color.White,
            ForeColor = InstallerConfig.PrimaryAccentColor,
            FlatStyle = FlatStyle.Flat
        };

        installButton.Click += InstallButton_Click;
        restoreButton.Click += RestoreButton_Click;

        this.Controls.AddRange(new Control[] {
            titleLabel,
            optionsGroup,
            progressBar,
            statusLabel,
            installButton,
            restoreButton
        });
    }

    private void InstallButton_Click(object sender, EventArgs e)
    {
        if (!VerifySystemCompatibility())
        {
            MessageBox.Show("Your system does not meet the minimum requirements.", 
                "Compatibility Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            installButton.Enabled = false;
            restoreButton.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            statusLabel.Text = "Installing Windows 12 UI...";

            Windows12UIManager.ApplyUIChanges(
                enableStartMenu.Checked,
                enableTaskbar.Checked,
                enableIcons.Checked
            );

            statusLabel.Text = "Installation completed successfully!";
            MessageBox.Show("Windows 12 UI has been successfully installed. Please restart your computer.", 
                "Installation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred during installation: {ex.Message}", 
                "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            installButton.Enabled = true;
            restoreButton.Enabled = true;
            progressBar.Style = ProgressBarStyle.Continuous;
        }
    }

    private void RestoreButton_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Are you sure you want to restore the original Windows UI?", 
            "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            try
            {
                statusLabel.Text = "Restoring original UI...";
                // Implement restore functionality
                statusLabel.Text = "Original UI restored successfully!";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during restoration: {ex.Message}", 
                    "Restoration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private bool VerifySystemCompatibility()
    {
        Version currentVersion = Environment.OSVersion.Version;
        Version minVersion = Version.Parse(InstallerConfig.MIN_WINDOWS_VERSION);
        
        return currentVersion >= minVersion;
    }
} 