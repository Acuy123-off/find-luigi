using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Drawing;

namespace FIND_SUHAIL
{
    public partial class Form1 : Form
    {
        public static Form1 instance;
        public Form1()
        {
            InitializeComponent();
            instance = this;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            

        }

        // Variables to track speed
        // untuk yang baca line ini, pasti lagi presentasi yaaa, dan pasti disuruh ngejelasin

        int speedlx = 5;
        int speedly = 5;
        int skore = 0;

        public static class GlobalData
        {
            // Variabel ini tidak akan hilang meskipun Form ditutup/buka
            public static int SkorTotal = 0;
        }

        private void Timer1_Tick_1(object sender, EventArgs e)
        {

            foreach (var logo in daftarLogo)
            {
                logo.Move(this.ClientSize);
            }
            // Paksa Form untuk menggambar ulang (memanggil event Paint)
            this.Invalidate();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {


    
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("jmbt");

            this.Hide();
        }
        // 1. Class Data untuk Logo (Tanpa PictureBox)
        public class LogoBergerak
        {
            public Image Gambar { get; set; }
            public float X, Y;
            public float SpeedX, SpeedY;
            public int Width = 60;
            public int Height = 60;

            // Penanda apakah ini entitas asli yang bisa diklik
            public bool IsTarget { get; set; }

            public void Move(Size clientSize)
            {
                X += SpeedX;
                Y += SpeedY;
                if (X <= 0 || X + Width >= clientSize.Width) SpeedX = -SpeedX;
                if (Y <= 0 || Y + Height >= clientSize.Height) SpeedY = -SpeedY;
            }
        }

        // 2. Di dalam Form Utama
        List<LogoBergerak> daftarLogo = new List<LogoBergerak>();

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //skorarara.Text = GlobalData.SkorTotal.ToString();
            //axWindowsMediaPlayer1.settings.setMode("loop", true);
            //axWindowsMediaPlayer1.URL = Application.StartupPath + "\\ty.mp3";
            //axWindowsMediaPlayer1.Ctlcontrols.play();

            Random rng = new Random();

            int jumlahCloneBase = 1 + (GlobalData.SkorTotal * 2);
            double jumlahCloneBase2 = 1 + (GlobalData.SkorTotal * 1.5);

            // Ambil gambar dari PictureBox yang kamu maksud (Entitas Berbeda)
            Image gambarTarget = Image.FromFile("luigi.png");/*pictureBox2.Image*/;
            // Ambil gambar untuk Clone
            Image gambarClone = Image.FromFile("yoshi.png");
            Image gambarClone2 = Image.FromFile("wario.png");
            Image gambarClone3 = Image.FromFile("mario.png");


            //if (GlobalData.SkorTotal == 2)
            //{
            //    gambarClone = Image.FromFile("ridh.png");
            //    gambarClone2 = Image.FromFile("suha.png");
            //    gambarClone3 = Image.FromFile("syam.png");
            //    gambarTarget = Image.FromFile("hayk.png");
            //}
            if (GlobalData.SkorTotal >= 1)
            {
                this.WindowState = FormWindowState.Maximized;
                jumlahCloneBase = 1 + (GlobalData.SkorTotal * 3);
                jumlahCloneBase2 = 1 + (GlobalData.SkorTotal * 2.5);
                Color myColor = ColorTranslator.FromHtml("#50B040");
                this.BackColor = myColor;
            }
            for (int i = 0; i < jumlahCloneBase2; i++)
            {
                LogoBergerak baru = new LogoBergerak();
                baru.X = rng.Next(0, this.ClientSize.Width - 60);
                baru.Y = rng.Next(0, this.ClientSize.Height - 60);
                baru.SpeedX = rng.Next(3, 6);
                baru.SpeedY = rng.Next(3, 6);


                if (i == 0) // Kita tentukan indeks ke-0 sebagai yang asli
                {
                    baru.Gambar = gambarTarget;
                    baru.IsTarget = true;
                }
                else // Sisanya adalah clone
                {
                    baru.Gambar = gambarClone;
                    baru.IsTarget = false;
                }

                daftarLogo.Add(baru);
            }
            for (int i = 0; i < jumlahCloneBase; i++)
            {
                LogoBergerak baru = new LogoBergerak();
                baru.X = rng.Next(0, this.ClientSize.Width - 60);
                baru.Y = rng.Next(0, this.ClientSize.Height - 60);
                baru.SpeedX = rng.Next(3, 6);
                baru.SpeedY = rng.Next(3, 6);


                    baru.Gambar = gambarClone2;
                    baru.IsTarget = false;

                daftarLogo.Add(baru);
            }
            for (int i = 0; i < jumlahCloneBase; i++)
            {
                LogoBergerak baru = new LogoBergerak();
                baru.X = rng.Next(0, this.ClientSize.Width - 60);
                baru.Y = rng.Next(0, this.ClientSize.Height - 60);
                baru.SpeedX = rng.Next(3, 6);
                baru.SpeedY = rng.Next(3, 6);


                baru.Gambar = gambarClone3;
                baru.IsTarget = false;

                daftarLogo.Add(baru);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        // 3. Gunakan Event Paint Form (Klik Petir di Properties Form -> Cari 'Paint')
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            skorarara.Text = GlobalData.SkorTotal.ToString();
            // Di sinilah keajaiban terjadi. PNG akan merender transparansi dengan benar.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var logo in daftarLogo)
            {
                e.Graphics.DrawImage(logo.Gambar, logo.X, logo.Y, logo.Width, logo.Height);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = daftarLogo.Count - 1; i >= 0; i--)
            {
                var logo = daftarLogo[i];
                RectangleF area = new RectangleF(logo.X, logo.Y, logo.Width, logo.Height);

                if (area.Contains(e.Location))
                {
                    if (logo.IsTarget)
                    {
                        SoundPlayer j = new SoundPlayer(Properties.Resources.luigi_woaaahh_scream1);
                        j.Play();
                        ////skore++;
                        //info.instance.skor += 1;
                        GlobalData.SkorTotal += 1;
                        skorarara.Text = GlobalData.SkorTotal.ToString();
                        MessageBox.Show("Kamu menemukan Luigi!, skormu :" + GlobalData.SkorTotal);
                        //this.Refresh();
                        Form1 r = new Form1();
                        r.Show();
                        this.Close();
                        // Aksi khusus untuk target...
                    }
                    else
                    {
                        // Ini jika user klik yang salah (Clone)
                        // Kamu bisa kosongkan atau beri penalti
                    }
                    break;
                }
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void skor_Click(object sender, EventArgs e)
        {

        }
    }
}
