using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //VERİTABANI BAĞLANTISINI YAPTIM...
        OleDbConnection baglanti;
        OleDbCommand komut;
        OleDbDataAdapter da;

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'kasavtDataSet.kasa' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.kasaTableAdapter.Fill(this.kasavtDataSet.kasa);
            KisiListele();
        }

        //KİŞİLERİ LİSTELEMEK İÇİN BİR FONKSİYON OLUŞTURUYORUZ...
        void KisiListele()
        {
            baglanti = new OleDbConnection("Provider=Microsoft.ACE.OleDb.12.0;Data Source=C:\\Users\\win10\\Desktop\\kasahesap\\kasahesap\\WindowsFormsApp1\\bin\\Debug\\kasavt.accdb");
            baglanti.Open();
            da = new OleDbDataAdapter("Select *From kasa", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();


        }

        //GÜNLÜK GELİRİ LİSTBOX'A ATADIKTAN SONRA GÜN SONU TOPLAMINA ve KASA TOPLAMINA EKLİYORUZ...

        private void button1_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(textBox2.Text);
            double topgelir = Convert.ToDouble(textBox8.Text);
            double topgider = Convert.ToDouble(textBox9.Text);
            double kasatoplam = Convert.ToDouble(textBox5.Text);
            double gunstop = Convert.ToDouble(textBox10.Text);
            double gunskasa = Convert.ToDouble(textBox11.Text);
            double oncguntop = Convert.ToDouble(textBox12.Text);
            double kasafazla = Convert.ToDouble(textBox6.Text);
            double kasaeksik = Convert.ToDouble(textBox7.Text);

            topgelir = x + topgelir;

            textBox8.Text = topgelir.ToString();
            listBox1.Items.Add(textBox1.Text);
            listBox3.Items.Add(textBox2.Text);
            textBox1.Text = "";
            textBox2.Text = "";

            gunstop = topgelir - topgider;
            textBox10.Text = gunstop.ToString();

            kasatoplam = oncguntop + gunstop;
            textBox5.Text = kasatoplam.ToString();
        }

        //GÜNLÜK GİDERİ LİSTBOX'A ATADIKTAN SONRA GÜN SONU TOPLAMINDAN ve KASA TOPLAMINDAN ÇIKARIYORUZ...

        private void button2_Click(object sender, EventArgs e)
        {
            double y = Convert.ToDouble(textBox4.Text);
            double topgider = Convert.ToDouble(textBox9.Text);
            double topgelir = Convert.ToDouble(textBox8.Text);
            double kasatoplam = Convert.ToDouble(textBox5.Text);
            double gunstop = Convert.ToDouble(textBox10.Text);
            double gunskasa = Convert.ToDouble(textBox11.Text);
            double oncguntop = Convert.ToDouble(textBox12.Text);


            topgider = y + topgider;

            textBox9.Text = topgider.ToString();
            listBox2.Items.Add(textBox3.Text);
            listBox4.Items.Add(textBox4.Text);
            textBox3.Text = "";
            textBox4.Text = "";


            gunstop = topgelir - topgider;
            textBox10.Text = gunstop.ToString();

            kasatoplam = oncguntop + gunstop;
            textBox5.Text = kasatoplam.ToString();
        }

        //KASA EKSİĞİ ve FAZLASINI HESAPLIYORUZ(VARSA)...
        private void button5_Click(object sender, EventArgs e)
        {
            double kasafazla = Convert.ToDouble(textBox6.Text);
            double kasaeksik = Convert.ToDouble(textBox7.Text);
            double gunstop = Convert.ToDouble(textBox10.Text);
            double gunskasa = Convert.ToDouble(textBox11.Text);
            double oncguntop = Convert.ToDouble(textBox12.Text);
            kasafazla = gunskasa - gunstop;
            kasaeksik = gunskasa - gunstop;

            if (gunskasa > gunstop)
            {
                kasafazla = gunskasa - gunstop;
                kasaeksik = 0;

            }
            else if (gunskasa < gunstop)
            {
                kasaeksik = gunstop - gunskasa;
                kasafazla = 0;
            }
            else
            {
                kasafazla = 0;
                kasaeksik = 0;
            }

            textBox6.Text = kasafazla.ToString();
            textBox7.Text = kasaeksik.ToString();
        }

        //VERİTABANINA EKLENECEK TEXTBOX ve LİSTBOXLARI TANIMLIYORUZ...
        private void button6_Click(object sender, EventArgs e)
        {
            string sorgu = "Insert into kasa (tarih,gelir_acıklama,gelir_tutar,toplam_gelir,gider_acıklama,gider_tutar,toplam_gider,günsonu_toplam,kasa_toplam) values (@tarih,@gelir_acıklama,@gelir_tutar,@toplam_gelir,@gider_acıklama,@gider_tutar,@toplam_gider,@günsonu_toplam,@kasa_toplam)";
            komut = new OleDbCommand(sorgu, baglanti);

            komut.Parameters.AddWithValue("@tarih", dateTimePicker1.Value);           
            komut.Parameters.Add("@gelir_acıklama", listBox1.Items.Cast<string>().Aggregate("", (q, x) => q + " " + x.ToString().Trim()));            
            komut.Parameters.Add("@gelir_tutar", listBox3.Items.Cast<string>().Aggregate("", (q, x) => q + " " + x.ToString().Trim()));           
            komut.Parameters.AddWithValue("@toplam_gelir", textBox8.Text);          
            komut.Parameters.Add("@gider_acıklama", listBox2.Items.Cast<string>().Aggregate("", (q, x) => q + " " + x.ToString().Trim()));          
            komut.Parameters.Add("@gider_tutar", listBox4.Items.Cast<string>().Aggregate("", (q, x) => q + " " + x.ToString().Trim()));         
            komut.Parameters.AddWithValue("@toplam_gider", textBox9.Text);
            komut.Parameters.AddWithValue("@günsonu_toplam", textBox10.Text);
            komut.Parameters.AddWithValue("@kasa_toplam", textBox5.Text);

            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            KisiListele();
            MessageBox.Show("Kayıt Eklendi");
        }

        //TARİH DEĞİŞTİĞİNDE İÇERİKLERİ SIFIRLIYORUZ...

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            double oncguntop = Convert.ToDouble(textBox12.Text);
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            textBox8.Text = "0";
            textBox9.Text = "0";
            textBox10.Text = "0";
            textBox11.Text = "0";
            textBox6.Text = "0";
            textBox7.Text = "0";
            //oncguntop = bir önceki günün toplamına ekleyerek gösterilecek. 
        }

        //VERİTABANINDAN SİLİNECEK VERİLERİ SİLİYORUZ...
        private void button3_Click(object sender, EventArgs e)
        {
            string sorgu = "Delete From kasa Where tarih=@tarih";
            komut = new OleDbCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@tarih", dataGridView1.CurrentRow.Cells[1].Value);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            KisiListele();
        }
    }
    
}
