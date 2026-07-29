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
    public partial class info : Form
    {
        public static info instance;
        public Label lbl;
        public Int32 skor;
        public info()
        {
            InitializeComponent();
            instance = this;
            lbl = skrplr;
            skor = 0;
            label1.Text = skor.ToString();
        }

        private void info_Load(object sender, EventArgs e)
        {

        }
        int score = 0;

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
