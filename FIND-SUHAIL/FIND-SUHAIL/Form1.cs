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
using MySql.Data.MySqlClient;
namespace FIND_SUHAIL
{
    public partial class Form1 : Form
    {
        public static Form1 instance;

        private string connectionString = "Server=localhost;Port=3306;Database=luigi2-db;Uid=root;Pwd=;";
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


        int waktuTersisa = 120; // 120 detik = 2 menit

        public static class GlobalData
        {
            // Variabel ini tidak akan hilang meskipun Form ditutup/buka
            public static int SkorTotal = 0;

            // Variabel penampung Username yang diisi dari Form Input sebelumnya
            public static string CurrentUsername { get; set; } = "Player_Anonymous";
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
            waktuTersisa = 60; // Riset ke 2 menit
            timeleft.Start(); // Mulai timer
            waktuna();
            ResetLevel();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        // 3. Gunakan Event Paint Form (Klik Petir di Properties Form -> Cari 'Paint')
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //skorarara.Text = GlobalData.SkorTotal.ToString();
            // Di sinilah keajaiban terjadi. PNG akan merender transparansi dengan benar.


            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var logo in daftarLogo)
            {
                e.Graphics.DrawImage(logo.Gambar, logo.X, logo.Y, logo.Width, logo.Height);
            }

            string textSkor = "Skor: " + GlobalData.SkorTotal.ToString();

            // Format waktu agar tampil MM:SS (Contoh: 01:55)
            TimeSpan t = TimeSpan.FromSeconds(waktuTersisa);
            string textTimer = string.Format("Waktu: {0:D2}:{1:D2}", t.Minutes, t.Seconds);

            string textUser = "Player: " + GlobalData.CurrentUsername;

            using (Font myFont = new Font("Arial", 16, FontStyle.Bold))
            {

                // Gambar Timer di bawah skor (koordinat Y ditambah)
                e.Graphics.DrawString(textTimer, myFont, Brushes.Yellow, 10, 40);

                e.Graphics.DrawString(textSkor, myFont, Brushes.White, 10, 10);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //for (int i = daftarLogo.Count - 1; i >= 0; i--)
            //{
            //    var logo = daftarLogo[i];
            //    RectangleF area = new RectangleF(logo.X, logo.Y, logo.Width, logo.Height);

            //    if (area.Contains(e.Location))
            //    {
            //        if (logo.IsTarget)
            //        {
            //            SoundPlayer j = new SoundPlayer(Properties.Resources.luigi_woaaahh_scream1);
            //            j.Play();
            //            ////skore++;
            //            //info.instance.skor += 1;
            //            GlobalData.SkorTotal += 1;
            //            skorarara.Text = GlobalData.SkorTotal.ToString();
            //            MessageBox.Show("Kamu menemukan Luigi!, skormu :" + GlobalData.SkorTotal);
            //            //this.Refresh();
            //            Form1 r = new Form1();
            //            r.Show();
            //            this.Close();
            //            // Aksi khusus untuk target...
            //        }
            //        else
            //        {
            //            // Ini jika user klik yang salah (Clone)
            //            // Kamu bisa kosongkan atau beri penalti
            //        }
            //        break;
            //    }
            //}
            // LANGKAH 1: Cek khusus untuk Target (Luigi) dulu
            // Kita cari apakah ada Target yang terkena klik
            foreach (var logo in daftarLogo)
            {
                if (logo.IsTarget)
                {
                    RectangleF area = new RectangleF(logo.X, logo.Y, logo.Width, logo.Height);
                    if (area.Contains(e.Location))
                    {
                        // Jika kena target, langsung jalankan fungsi menang dan KELUAR dari method
                        BerhasilMenang();
                        return; // Berhenti di sini, jangan cek yang lain
                    }
                } else
                {

                }
            }

            // LANGKAH 2: Jika kode sampai di sini, berarti tidak ada target yang kena klik.
            // Baru kita cek apakah ada Clone (salah klik) yang terkena.
            for (int i = daftarLogo.Count - 1; i >= 0; i--)
            {
                var logo = daftarLogo[i];
                if (!logo.IsTarget) // Hanya cek yang bukan target
                {
                    RectangleF area = new RectangleF(logo.X, logo.Y, logo.Width, logo.Height);
                    if (area.Contains(e.Location))
                    {
                        // Logika jika user klik clone (yang salah)
                        // MessageBox.Show("Itu bukan Luigi!");
                        waktuTersisa = waktuTersisa - 5;
                        break;
                    }
                }
            }
        }

        private void BerhasilMenang()
        {
            SoundPlayer j = new SoundPlayer(Properties.Resources.luigi_woaaahh_scream1);
            j.Play();

            GlobalData.SkorTotal += 1;
            timeleft.Stop();
            MessageBox.Show("Kamu menemukan Luigi!, skormu :" + GlobalData.SkorTotal);
            waktuTersisa = 60; // Riset ke 2 menit
            timeleft.Start();

            //Form1 r = new Form1();
            //r.Show();
            //this.Close();
            ResetLevel();
        }

        private void waktuna()
        {

        }
        private void ResetLevel()
        {
            // 1. Bersihkan list yang lama supaya memori lega
            daftarLogo.Clear();

            //skorarara.Text = GlobalData.SkorTotal.ToString();
            //axWindowsMediaPlayer1.settings.setMode("loop", true);
            //axWindowsMediaPlayer1.URL = Application.StartupPath + "\\ty.mp3";
            //axWindowsMediaPlayer1.Ctlcontrols.play();

            Random rng = new Random();
            double jumlahCloneBase = 1 + (GlobalData.SkorTotal * 2);
            double jumlahCloneBase2 = 1 + (GlobalData.SkorTotal * 1.5);
                        double jumlahCloneBase3 = 1 + GlobalData.SkorTotal;

            // Ambil gambar dari PictureBox yang kamu maksud (Entitas Berbeda)
            Image gambarTarget = Image.FromFile("luigi.png");/*pictureBox2.Image*/;
            // Ambil gambar untuk Clone
            Image gambarClone = Image.FromFile("yoshi.png");
            Image gambarClone2 = Image.FromFile("wario.png");
            Image gambarClone3 = Image.FromFile("mario.png");
            Image gambarClone4 = Image.FromFile("waluigi.png");
            Image gambarClone5 = Image.FromFile("miku.png");
            //if (GlobalData.SkorTotal == 2)
            //{
            Image    ridh = Image.FromFile("ridh.png");
            Image    suha = Image.FromFile("suha.png");
            Image    syam = Image.FromFile("syam.png");
            Image    hayk = Image.FromFile("hayk.png");
            //}

            if (GlobalData.SkorTotal >= 30)
            {
                Color myColor = ColorTranslator.FromHtml("#50B040");
                this.BackColor = myColor;
            }
            if (GlobalData.SkorTotal >= 20)
            {
                this.WindowState = FormWindowState.Maximized;
                jumlahCloneBase = 1 + (GlobalData.SkorTotal * 3);
                jumlahCloneBase2 = 1 + (GlobalData.SkorTotal * 2.5);
                //Color myColor = ColorTranslator.FromHtml("#50B040");
                //this.BackColor = myColor;
                for (int i = 0; i < jumlahCloneBase3; i++)
                {
                    LogoBergerak baru = new LogoBergerak();
                    baru.X = rng.Next(0, this.ClientSize.Width - 60);
                    baru.Y = rng.Next(0, this.ClientSize.Height - 60);
                    baru.SpeedX = rng.Next(3, 6);
                    baru.SpeedY = rng.Next(3, 6);


                    baru.SpeedX = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);
                    baru.SpeedY = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);

                    baru.Gambar = gambarClone4;
                    baru.IsTarget = false;

                    daftarLogo.Add(baru);
                }
            }
            if (GlobalData.SkorTotal >= 5)
            {
                for (int i = 0; i < jumlahCloneBase2; i++)
                {
                    LogoBergerak baru = new LogoBergerak();
                    baru.X = rng.Next(0, this.ClientSize.Width - 60);
                    baru.Y = rng.Next(0, this.ClientSize.Height - 60);
                    baru.SpeedX = rng.Next(3, 6);
                    baru.SpeedY = rng.Next(3, 6);


                    baru.SpeedX = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);
                    baru.SpeedY = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);

                    baru.Gambar = gambarClone5;
                    baru.IsTarget = false;

                    daftarLogo.Add(baru);
                }
            }
            //else if (GlobalData.SkorTotal >= 3)
            //{
            //    this.WindowState = FormWindowState.Maximized;
            //    jumlahCloneBase = 1 + (GlobalData.SkorTotal * 3.2);
            //    jumlahCloneBase2 = 1 + (GlobalData.SkorTotal * 2.7);
            //    gambarClone2 = (Properties.Resources.wamario);
            //    gambarClone3 = Image.FromFile("wawario.png");

            //    Color myColor = ColorTranslator.FromHtml("#FFFFFF");
            //    this.BackColor = myColor;
            //}
            for (int i = 0; i < jumlahCloneBase2; i++)
            {
                LogoBergerak baru = new LogoBergerak();
                baru.X = rng.Next(0, this.ClientSize.Width - 60);
                baru.Y = rng.Next(0, this.ClientSize.Height - 60);
                baru.SpeedX = rng.Next(3, 6);
                baru.SpeedY = rng.Next(3, 6);


                baru.SpeedX = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);
                baru.SpeedY = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);

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


                baru.SpeedX = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);
                baru.SpeedY = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);

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

                baru.SpeedX = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);
                baru.SpeedY = rng.Next(3, 6) * (rng.Next(0, 2) == 0 ? 1 : -1);

                baru.Gambar = gambarClone3;
                baru.IsTarget = false;

                daftarLogo.Add(baru);
            }
            this.Invalidate();
        }



        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void skor_Click(object sender, EventArgs e)
        {

        }

        private void Timer3_Tick(object sender, EventArgs e)
        {
            waktuTersisa--;
            if (waktuTersisa <= 0)
            {
                timeleft.Stop(); // Hentikan timer permainan
                daftarLogo.Clear();
                MessageBox.Show("Waktu Habis! Game Over.");

                // PANGGIL LOGIKA SIMPAN LEADERBOARD DI SINI

                SimpanSkorKeMySQL(GlobalData.CurrentUsername, GlobalData.SkorTotal);

                // Reset Skor setelah data terkirim
                GlobalData.SkorTotal = 0;

                // Berpindah ke Form Menu Utama
                if (loading.instance != null)
                {
                    loading.instance.Close();
                }
                hasilg r = new hasilg();
                r.Show();
                this.Close();
            }
            this.Invalidate();
        }

        // Fungsi baru untuk mengirim data ke database
        private void SimpanSkorKeMySQL(string username, int skorAkhir)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Leaderboard (username, highScore) VALUES (?User, ?Score)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?User", username);
                    cmd.Parameters.AddWithValue("?Score", skorAkhir);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Skor Anda berhasil dicatatkan di Papan Peringkat!", "Leaderboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal terhubung ke database untuk update leaderboard: " + ex.Message, "Koneksi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

    }
}
