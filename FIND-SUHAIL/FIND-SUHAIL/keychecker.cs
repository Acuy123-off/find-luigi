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
using System.Media;
namespace FIND_SUHAIL
{
    public partial class keychecker : Form
    {
        string connectionString = "Server=localhost;Port=3306;Database=luigi2-db;Uid=root;Pwd=;";
        private SoundPlayer hoverSound = new SoundPlayer(@"hover.wav");
        private SoundPlayer clickSound = new SoundPlayer(@"click.wav");
        public keychecker()
        {
            InitializeComponent();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            admin r = new admin();
            r.Show();
        }

        private void Keychecker_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            clickSound.Play();
            string inputKey = txtinputKey.Text.Trim();

            if (string.IsNullOrEmpty(inputKey))
            {
                MessageBox.Show("Silakan masukkan key terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // Query mengecek status key menggunakan parameter MySQL (?Key)
                string selectQuery = "SELECT IsUsed FROM ProductKeys WHERE ProductKey = ?Key";

                using (MySqlCommand cmdSelect = new MySqlCommand(selectQuery, conn))
                {
                    cmdSelect.Parameters.AddWithValue("?Key", inputKey);

                    try
                    {
                        conn.Open();
                        object result = cmdSelect.ExecuteScalar();

                        if (result != null) // Jika key ditemukan
                        {
                            // Di MySQL, TINYINT dibaca sebagai angka (byte/int), atau boolean secara implisit
                            int isUsed = Convert.ToInt32(result);

                            if (isUsed == 1)
                            {
                                SoundPlayer j = new SoundPlayer(Properties.Resources.luigi_woaaahh_scream1);
                                j.Play();
                                MessageBox.Show("Maaf, Key ini sudah pernah digunakan!", "Aktivasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                // Update status key menjadi sudah digunakan (IsUsed = 1)
                                string updateQuery = "UPDATE ProductKeys SET IsUsed = 1 WHERE ProductKey = ?Key";
                                using (MySqlCommand cmdUpdate = new MySqlCommand(updateQuery, conn))
                                {
                                    cmdUpdate.Parameters.AddWithValue("?Key", inputKey);
                                    cmdUpdate.ExecuteNonQuery();
                                }

                                MessageBox.Show("Aktivasi Sukses! Terima kasih telah mengaktifkan aplikasi.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // LANJUTKAN: Buka Form Utama Aplikasi Anda di sini
                                 loading mainForm = new loading();
                                 mainForm.Show();
                                 this.Hide();
                            }
                        }
                        else
                        {
                            SoundPlayer j = new SoundPlayer(Properties.Resources.luigi_woaaahh_scream1);
                            j.Play();
                            MessageBox.Show("Key tidak valid atau tidak terdaftar!", "Aktivasi Gagal", MessageBoxButtons.OK);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan koneksi MySQL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            clickSound.Play();
            txtinputKey.Text = "";
        }

        private void Button1_MouseEnter(object sender, EventArgs e)
        {
            hoverSound.Play();
        }

        private void Button2_MouseEnter(object sender, EventArgs e)
        {
            hoverSound.Play();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            // 1. Cek apakah clipboard saat ini berisi data teks atau tidak
            if (Clipboard.ContainsText())
            {
                // 2. Ambil teks dari clipboard dan masukkan ke TextBox Anda
                // Ganti 'txtInputKey' dengan nama TextBox yang Anda gunakan
                txtinputKey.Text = Clipboard.GetText();
            }
            else
            {
                // Jika clipboard kosong atau berisi selain teks (seperti gambar/file)
                MessageBox.Show("Clipboard kosong atau tidak berisi teks yang valid!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
