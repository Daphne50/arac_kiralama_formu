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

namespace araç_kiralama_formu
{
    public partial class AracYonetim : Form
    {
        public AracYonetim()
        {
            InitializeComponent();
        }
        SQLiteConnection con = new SQLiteConnection("Data Source=arackiralam.db;Version=3;");

        private void listele(string kosullar = "")
        {
            con.Open();
            using (SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT * FROM arackiralama " + kosullar, con))
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            con.Close();
        }

        private void AracYonetim_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            listele();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                listele(" where durum = '" + comboBox1.SelectedItem.ToString() + "'");
            }
            else
            {
                listele();
            }
            
        }

        private void seciliKaydiSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("delete from arackiralama where id = @p1", con))
                {
                    cmd.Parameters.AddWithValue("@p1", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                MessageBox.Show("Seçili kayıt silindi!");
                listele();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows[0].Cells[9].Value.ToString() == "Bosta")
            {
                comboBox2.SelectedIndex = 0;
            } else
            {
                comboBox2.SelectedIndex = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && comboBox2.SelectedItem != null)
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("update arackiralama set durum = @durum where id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@durum", comboBox2.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();

                }
                con.Close();
                listele();
                comboBox2.SelectedItem = null;
            } else
            {
                MessageBox.Show("Önce bir kayıt seçiniz!");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AracEkle aekle = new AracEkle();
            aekle.ShowDialog();
        }
    }
}
