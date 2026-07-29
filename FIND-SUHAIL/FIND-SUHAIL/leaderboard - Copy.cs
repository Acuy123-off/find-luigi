using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace FIND_SUHAIL
{
    public partial class leaderboard : UserControl
    {
        private string connectionString = "server=localhost;port=3306;username=root;password=;database=luigi2-db;";
        public leaderboard()
        {
            InitializeComponent();
            KustomisasiLeaderboard();
            MuatDataLeaderboard();
        }

        private void leaderboard_Load(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();

            if (parentForm != null)
            {
                // Jika dibuka dari Form MENU
                if (parentForm is menu mainForm)
                {
                    // Lakukan sesuatu khusus untuk Form Menu jika ada
                    // Contoh: mainForm.TeksStatus = "Melihat Leaderboard";
                }
                // Jika dibuka dari Form HASILG
                else if (parentForm is hasilg hasilForm)
                {
                    // Lakukan sesuatu khusus untuk Form Hasilg jika ada
                }
            }
        }

        private void MuatDataLeaderboard()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Query untuk mengambil skor tertinggi (diurutkan dari yang terbesar)
                    string query = "SELECT username AS 'Nama Pemain', highscore AS 'Skor Tertinggi' FROM leaderboard ORDER BY highscore DESC LIMIT 10";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Masukkan data ke DataGridView
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat leaderboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void KustomisasiLeaderboard()
        {
            // 1. Pengaturan Warna Dasar (Tema Gelap)
            dataGridView1.BackgroundColor = Color.Black;
            dataGridView1.ForeColor = Color.White;
            dataGridView1.GridColor = Color.DarkGreen; // Garis pembatas warna hijau Luigi
            dataGridView1.BorderStyle = BorderStyle.None;

            // 2. Pengaturan Header (Judul Kolom)
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.LimeGreen; // Teks Hijau Neon
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeight = 40;

            // 3. Pengaturan Baris/Row Data
            dataGridView1.DefaultCellStyle.BackColor = Color.Black;
            dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkGreen; // Warna saat baris diklik
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Yellow;
            dataGridView1.RowTemplate.Height = 35;

            // 4. Hilangkan border dan komponen pengganggu bawaan Windows
            dataGridView1.RowHeadersVisible = false; // Menghilangkan kolom kosong paling kiri
            dataGridView1.AllowUserToAddRows = false; // Menghilangkan baris kosong terbawah
            dataGridView1.ReadOnly = true;

            // Auto-size agar kolom memenuhi lebar DataGridView
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
