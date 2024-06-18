using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace araç_kiralama_formu
{
    public partial class AracEkle : Form
    {
        SQLiteConnection con = new SQLiteConnection("Data Source=arackiralam.db;Version=3;");

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif";
            openFileDialog.Title = "Resim Seç";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string dosyaYolu = openFileDialog.FileName;
                pictureBox1.Image = Image.FromFile(dosyaYolu);
            }
        }

        public AracEkle()
        {
            InitializeComponent();
        }
       
            

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            durum.SelectedIndex = 0;
        }
        private bool plakaKontrol(string plaka)
        {
            DataSet ds = new DataSet();
            using (SQLiteDataAdapter da = new SQLiteDataAdapter("select * from arackiralama where plaka = '" + plaka + "' LIMIT 1", con))
            {
                da.Fill(ds);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return false;
            } else
            {
                return true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                if (plaka.Text.Length > 0 && marka.Text.Length > 0 && uretimyili.Text.Length > 0 &&
                    km.Text.Length > 0 && renk.Text.Length > 0 && yakitturu.Text.Length > 0 &&
                    kiraucreti.Text.Length > 0 && model.Text.Length > 0)
                {
                    con.Open();
                    if (plakaKontrol(plaka.Text.Trim()))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] resimData = ms.ToArray();

                            string eklequery = "INSERT INTO arackiralama(plaka, marka, uretimyili, km, renk, yakitturu, kiraucreti, resim, durum, model) VALUES (@p2, @p3, @p4, @p5, @p6, @p7, @p8, @resimData, @p10, @p11)";
                            SQLiteCommand command = new SQLiteCommand(eklequery, con);
                            command.Parameters.AddWithValue("@p2", plaka.Text);
                            command.Parameters.AddWithValue("@p3", marka.Text);
                            command.Parameters.AddWithValue("@p4", uretimyili.Text);
                            command.Parameters.AddWithValue("@p5", km.Text);
                            command.Parameters.AddWithValue("@p6", renk.Text);
                            command.Parameters.AddWithValue("@p7", yakitturu.Text);
                            command.Parameters.AddWithValue("@p8", kiraucreti.Text);
                            command.Parameters.AddWithValue("@resimData", resimData);
                            command.Parameters.AddWithValue("@p10", durum.Text);
                            command.Parameters.AddWithValue("@p11", model.Text);

                            command.ExecuteNonQuery();
                            MessageBox.Show("Araç bilgileri başarıyla kaydedildi.");
                            button4_Click(null, null);
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Bu araç daha önce eklendi!");
                    }
                    con.Close();

                } else
                {
                    MessageBox.Show("Bilgileri eksiksiz giriniz!");
                }

            }
            else
            {
                MessageBox.Show("Önce bir resim seçmelisiniz.");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            plaka.Text = "";
            marka.Text = "";
            model.Text = "";
            uretimyili.Text = "";
            km.Text = "";
            renk.Text = "";
            yakitturu.Text = "";
            kiraucreti.Text = "";
            durum.SelectedIndex = 0;
            pictureBox1.Image = null;
        }
    }
}
