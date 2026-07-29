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
        // Konstanta agar Form tidak mengambil fokus
        private const int WS_EX_NOACTIVATE = 0x08000000;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE;
                return cp;
            }
        }
        // Deklarasi player agar tidak membebani memori (reusable)
        private SoundPlayer hoverSound = new SoundPlayer(@"hover.wav");
        private SoundPlayer clickSound = new SoundPlayer(@"click.wav");
        public menu()
        {
            InitializeComponent();
            this.TopMost = true;

            // Jalankan fungsi pembuat keyboard instan
            BuatKeyboardInstan();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
    ControlStyles.UserPaint |
    ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            //play.MouseEnter += play_MouseEnter;
            //play.Click += play_Click;
        }
        private void BuatKeyboardInstan()
        {
            // Susunan tombol keyboard dalam bentuk Array
            string[] semuaTombol = {
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "Backspace",
                "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
                "A", "S", "D", "F", "G", "H", "J", "K", "L", "Enter",
                "Z", "X", "C", "V", "B", "N", "M", "Space"
            };

            // Looping untuk membuat tombol satu per satu secara otomatis
            foreach (string teks in semuaTombol)
            {
                Button btn = new Button();
                btn.Text = teks;

                // Mengatur ukuran tombol (bisa disesuaikan)
                if (teks == "Backspace" || teks == "Enter")
                {
                    btn.Size = new Size(90, 45); // Tombol khusus lebih lebar
                }
                else if (teks == "Space")
                {
                    btn.Size = new Size(200, 45); // Tombol spasi sangat lebar
                }
                else
                {
                    btn.Size = new Size(45, 45); // Tombol huruf standar (kotak)
                }

                // Menghubungkan event klik tombol ke fungsi logika
                btn.Click += Tombol_Click;

                // Masukkan tombol yang baru dibuat ke dalam FlowLayoutPanel
                layoutKeyboard.Controls.Add(btn);
            }
        }

        private void Tombol_Click(object sender, EventArgs e)
        {
            Button tombolDiKlik = (Button)sender;
            string input = tombolDiKlik.Text;

            if (input == "Backspace")
            {
                // JIKA BACKSPACE: Hapus 1 karakter terakhir di txtusername (jika tidak kosong)
                if (txtusername.Text.Length > 0)
                {
                    txtusername.Text = txtusername.Text.Substring(0, txtusername.Text.Length - 1);
                }
            }
            else if (input == "Space")
            {
                // JIKA SPACE: Tambahkan spasi karakter
                txtusername.Text += " ";
            }
            else
            {
                // JIKA HURUF/ANGKA: Langsung gabungkan teks tombol ke txtusername
                txtusername.Text += input;
            }

            // Opsional: Kembalikan kursor ke ujung kanan teks txtusername agar terlihat aktif
            txtusername.Focus();
            txtusername.SelectionStart = txtusername.Text.Length;
        }

        public static class PlayerData
        {
            // Variabel ini akan menyimpan nama player selama aplikasi berjalan
            public static string CurrentUsername { get; set; }
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
            SwitchRoom(new leaderboard());
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

        private void Play_MouseEnter_1(object sender, EventArgs e)
        {
            hoverSound.Play();
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            admin r = new admin();
            r.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string namaInput = txtusername.Text.Trim();

            if (string.IsNullOrEmpty(namaInput))
            {
                MessageBox.Show("Username tidak boleh kosong!", "Peringatan");
                return;
            }

            // Simpan ke variabel global
            FIND_SUHAIL.Form1.GlobalData.CurrentUsername = namaInput;
            label1.Visible = false;
            pictureBox1.Visible = true;
            panel1.Visible = true;
            play.Visible = true;
            button2.Visible = true;
            button1.Visible = false;
            txtusername.Visible = false;
            layoutKeyboard.Visible = false;
        }

        private void Txtusername_TextChanged(object sender, EventArgs e)
        {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
