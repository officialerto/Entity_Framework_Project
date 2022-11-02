using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//SQL İLE BAĞLANTI KURACAĞIMIZ İÇİN İLK KÜTÜPHANEYİ EKLEDİK
using System.Data.SqlClient;

namespace EntityFrameworkAppSon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities();
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnDersListesi_Click(object sender, EventArgs e)
        {
            //SQL Connection bağlantısı Project -> Add New Data Source'dan alınıyor.
            SqlConnection baglanti = new SqlConnection(@"Data Source=.;Initial Catalog=DbSinavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select * from tbldersler", baglanti);
            //SqlCommand bizim veritabanı üzerinde yapmak istediğimiz işlemlerin ADO tarafında belirtilmesini sağlayan sınıftır
            SqlDataAdapter da = new SqlDataAdapter(komut);
            //SqlDataAdapter veri almak ve kaydetmek için ve SQL Server arasında bir DataSet köprü görevi görür
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void BtnOgrenciListele_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TBLOGRENCI.ToList();
        }

        private void BtnNotListesi_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = db.TBLNOTLAR.ToList();
            //LINQ SORGUSU İLE AŞAĞIDAKİLERİ GÖSTERİYORUZ
            var query = from item in db.TBLNOTLAR
                        select new
                        {
                            item.NOTID,
                            item.OGR,
                            item.DERS,
                            item.SINAV1,
                            item.SINAV2,
                            item.SINAV3,
                            item.ORTALAMA,
                            item.DURUM
                        };

            dataGridView1.DataSource = query.ToList();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //Databasedeki tblöğrencinin içinde t adında bir nesne türetildi
            TBLDERSLER d = new TBLDERSLER();
            TBLOGRENCI t = new TBLOGRENCI();
            //TxtAd kutucuğuna yazılanı türetilen nesneye bağlı olan AD'a aktar.
            t.AD = TxtAd.Text;
            //TxtSoyad kutucuğuna yazılanı türetilen nesneye bağlı olan SOYAD'a aktar.
            t.SOYAD = TxtSoyad.Text;

            d.DERSAD = TxtDersAd.Text;
            //Ekleme işlemi
            db.TBLOGRENCI.Add(t);
            db.TBLDERSLER.Add(d);
            //Kaydetme işlemi
            db.SaveChanges();
            //Mesaj kutusu veriyor.
            MessageBox.Show("Öğrenci Listeye Eklenmiştir.");
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {   //ÖĞRENCİ SİLME İŞLEMİ
            int id = Convert.ToInt32(TxtOgrenciID.Text);
            var x = db.TBLOGRENCI.Find(id);
            db.TBLOGRENCI.Remove(x);
            //Silme işlemi
            db.SaveChanges();
            MessageBox.Show("Silindi kanka!");
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {   //ÖĞRENCİ GÜNCELLEME İŞLEMİ
            int id = Convert.ToInt32(TxtOgrenciID.Text);
            var x = db.TBLOGRENCI.Find(id);
            x.AD = TxtAd.Text;
            x.SOYAD = TxtSoyad.Text;
            x.FOTOGRAF = TxtFoto.Text;
            db.SaveChanges();
            MessageBox.Show("Güncellendi hacı!");
        }

        private void BtnProsedur_Click(object sender, EventArgs e)
        {

        }
    }
}
