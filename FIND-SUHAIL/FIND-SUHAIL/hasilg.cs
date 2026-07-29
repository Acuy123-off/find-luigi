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
    public partial class hasilg : Form
    {
        public hasilg()
        {
            InitializeComponent();
        }

        private void Hasilg_Load(object sender, EventArgs e)
        {

            SwitchRoom(new leaderboard());
        }

        public void SwitchRoom(leaderboard nextRoom)
        {
            // 1. Clear the old room from the panel
            panel1.Controls.Clear();

            // 2. Make the new room stretch to fit the window
            nextRoom.Dock = DockStyle.Fill;

            // 3. Put the new room inside the panel
            panel1.Controls.Add(nextRoom);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            keychecker r = new keychecker();
            r.Show();
            this.Close();
        }
    }
}
