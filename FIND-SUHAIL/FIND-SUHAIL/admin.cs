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
    public partial class admin : Form
    {
        string connectionString = "Server=localhost;Port=3306;Database=luigi2-db;Uid=root;Pwd=;";
        public admin()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private string GenerateRandomKey(int lengthPerBlock, int blockCount)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            StringBuilder keyBuilder = new StringBuilder();

            for (int i = 0; i < blockCount; i++)
            {
                char[] block = new char[lengthPerBlock];
                for (int j = 0; j < lengthPerBlock; j++)
                {
                    block[j] = validChars[random.Next(validChars.Length)];
                }
                keyBuilder.Append(new string(block));

                if (i < blockCount - 1)
                {
                    keyBuilder.Append("-");
                }
            }
            return keyBuilder.ToString();
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            // 1. Generate key acak
            string newKey = GenerateRandomKey(5, 4);
            txtGeneratedKey.Text = newKey;

            // 2. Simpan ke Database MySQL
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // Menggunakan tanda tanya (?) sebagai parameter khas MySQL
                string query = "INSERT INTO ProductKeys (ProductKey, IsUsed) VALUES (?Key, 0)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?Key", newKey);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Key baru berhasil dibuat dan disimpan ke penyimpananku!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menyimpan ke penyimpananku: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGeneratedKey.Text))
            {
                Clipboard.SetText(txtGeneratedKey.Text);
                MessageBox.Show("Key berhasil disalin ke clipboard!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "FutureTurtle723")
            {
                MessageBox.Show("halo Acuy123");
                txtGeneratedKey.Visible = true;
                btnGenerate.Visible = true;
                btnCopy.Visible = true;
            } else if (textBox1.Text == "Haykal7780>ha784")
            {
                MessageBox.Show("halo Avalon The Great");
                txtGeneratedKey.Visible = true;
                btnGenerate.Visible = true;
                btnCopy.Visible = true;
            } else if (textBox1.Text == "aa")
            {
                MessageBox.Show("halo Fay.Flourite");
                txtGeneratedKey.Visible = true;
                btnGenerate.Visible = true;
                btnCopy.Visible = true;
            }
        }
    }
}
