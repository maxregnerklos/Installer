﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Rectify11ControlCenter
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            OSname.Text = Rectify11ControlCenter.Controls.osV;
            username.Text = Rectify11ControlCenter.Controls.userN;
            pcname.Text = Rectify11ControlCenter.Controls.CumterName;
            themeApplied.Text = Rectify11ControlCenter.Controls.theme();
            r11Ver.Text = Rectify11ControlCenter.Controls.r11Version;
            deskImg.Image = Rectify11ControlCenter.Controls.deskimg();
            themesSec.Text = Rectify11ControlCenter.Controls.themeSection;
            miscSec.Text = Rectify11ControlCenter.Controls.miscSection;
            checkBox2.Text = Rectify11ControlCenter.Controls.mfeChexbox;
            button1.Text = Rectify11ControlCenter.Controls.applyButton;
            for (int i = 0; i < Rectify11ControlCenter.Controls.themefiles.Length; i++)
            {
                comboBox1.Items.Add(Path.GetFileNameWithoutExtension(Rectify11ControlCenter.Controls.themefiles[i].FullName));
            }
            comboBox1.SelectedItem = Rectify11ControlCenter.Controls.appliedthemefile();
            addroundedCorners();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {

        }

        private void addroundedCorners()
        {
            Rectangle r = new Rectangle(0, 0, deskImg.Width, deskImg.Height);
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            int d = 11;
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            deskImg.Region = new Region(gp);

            Rectangle r2 = new Rectangle(0, 0, themePrev.Width, themePrev.Height);
            System.Drawing.Drawing2D.GraphicsPath gp2 = new System.Drawing.Drawing2D.GraphicsPath();
            int d2 = 11;
            gp2.AddArc(r2.X, r2.Y, d2, d2, 180, 90);
            gp2.AddArc(r2.X + r2.Width - d2, r2.Y, d2, d2, 270, 90);
            gp2.AddArc(r2.X + r2.Width - d2, r2.Y + r2.Height - d2, d2, d2, 0, 90);
            gp2.AddArc(r2.X, r2.Y + r2.Height - d2, d2, d2, 90, 90);
            themePrev.Region = new Region(gp2);
        }
    }
}
