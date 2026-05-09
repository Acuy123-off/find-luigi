using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIND_SUHAIL
{
    public partial class loading : Form
    {
        public loading()
        {
            InitializeComponent();
        }

        private void waktutunggu_Tick(object sender, EventArgs e)
        {
            waktutunggu.Stop();
            this.Hide(); // Closes the form after 3 seconds
            menu r = new menu();
            r.Show();
        }

        private void loading_Load(object sender, EventArgs e)
        {
            waktutunggu.Interval = 3000; // 3 seconds
            waktutunggu.Start();
            axWindowsMediaPlayer1.settings.setMode("loop", true);
            axWindowsMediaPlayer1.URL = Application.StartupPath + "\\ty.mp3";
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
    }
}
