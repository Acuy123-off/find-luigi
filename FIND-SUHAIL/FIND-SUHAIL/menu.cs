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

namespace FIND_SUHAIL
{
    public partial class menu : Form
    {

        // Deklarasi player agar tidak membebani memori (reusable)
        private SoundPlayer hoverSound = new SoundPlayer(@"hover.wav");
        private SoundPlayer clickSound = new SoundPlayer(@"click.wav");
        public menu()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
    ControlStyles.UserPaint |
    ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            //play.MouseEnter += play_MouseEnter;
            //play.Click += play_Click;
        }

        private void play_Click(object sender, EventArgs e)
        {
            clickSound.Play();
            Form1 r = new Form1();
            r.Show();
            this.Close();
        }

        public class LogoBergerak
        {
            public Image Gambar { get; set; }
            public float X, Y;
            public float SpeedX, SpeedY;
            public int Width = 80;
            public int Height = 80;

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
        List<LogoBergerak> daftarLogo = new List<LogoBergerak>();
        private void menu_Load(object sender, EventArgs e)
        {
            //info r = new info();
            //r.Show();

            Random rng = new Random();

            // Ambil gambar dari PictureBox yang kamu maksud (Entitas Berbeda)
            Image gambarTarget = Image.FromFile("luigi.png");
            Image gambarTarget2 = Image.FromFile("Find-Luigi-5-7-2026 (1).gif");
            for (int i = 0; i < 5; i++)
            {
                LogoBergerak baru = new LogoBergerak();
                baru.X = rng.Next(0, this.ClientSize.Width - 80);
                baru.Y = rng.Next(0, this.ClientSize.Height - 80);
                baru.SpeedX = rng.Next(3, 6);
                baru.SpeedY = rng.Next(3, 6);


                if (i == 0) // Kita tentukan indeks ke-0 sebagai yang asli
                {
                    baru.Gambar = gambarTarget;
                    baru.IsTarget = true;
                }
                else
                {
                    baru.Gambar = gambarTarget;
                    baru.IsTarget = false;
                }
                daftarLogo.Add(baru);
            }
        }

        private void menu_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var logo in daftarLogo)
            {
                e.Graphics.DrawImage(logo.Gambar, logo.X, logo.Y, logo.Width, logo.Height);
            }
        }

        private void menu_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = daftarLogo.Count - 1; i >= 0; i--)
            {
                var logo = daftarLogo[i];
                RectangleF area = new RectangleF(logo.X, logo.Y, logo.Width, logo.Height);

                if (area.Contains(e.Location))
                {
                    if (logo.IsTarget)
                    {
                        MessageBox.Show("Egg");
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var logo in daftarLogo)
            {
                logo.Move(this.ClientSize);
            }
            // Paksa Form untuk menggambar ulang (memanggil event Paint)
            this.Invalidate();
        }

        private void menu_MouseEnter(object sender, EventArgs e)
        {

        }

        private void play_MouseEnter(object sender, EventArgs e)
        {
            hoverSound.Play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clickSound.Play();
            Application.Exit();
        }

        private void menu_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            hoverSound.Play();
        }
    }
}
