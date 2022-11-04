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
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
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
        {   //VERİTABANINDA YAPTIĞIMIZ PROSEDÜRÜ MODEL'DEN UPDATE ETTİK VE ONLARI PROSEDÜR TUŞUNDAN LİSTELETTİK.
            dataGridView1.DataSource = db.NOTLISTESI();
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {   //TXT.AD BLOĞUNA YAZDIĞIMIZ DEĞERİ X ADINDA SALLAMASYON BİR ŞEY UYDURDUK O DEĞERE BAĞLADIK ONUNLA ARAMA YAPACAĞIZ ÇÜNKÜ VE TXT'YE YAZDIĞIMIZ ŞEYİ 'AD' KISMINDA ARATIYORUZ
            dataGridView1.DataSource = db.TBLOGRENCI.Where(x => x.AD == TxtAd.Text & x.SOYAD == TxtSoyad.Text).ToList();
        }

        private void TxtAd_TextChanged(object sender, EventArgs e)
        {   //Txtad kutucuğuna girilecek değeri string olarak atadık. Sonra LINQ sorgusuyla 'Contains' metodu kullanarak o kelimeyi içeriyorsa listele dedik
            string aranan = TxtAd.Text;
            var degerler = from item in db.TBLOGRENCI
                           where item.AD.Contains(aranan)
                           select item;
            dataGridView1.DataSource = degerler.ToList();

        }

        private void BtnLinqEntity_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {   //RADIO BUTTON SEÇİLİ OLDUĞUNDA ADA GÖRE SIRALIYOR
                List<TBLOGRENCI> liste1 = db.TBLOGRENCI.OrderBy(p => p.AD).ToList();
                dataGridView1.DataSource = liste1;
            }

            if (radioButton2.Checked == true)
            {   //RADIO BUTTON SEÇİLİ OLDUĞUNDA ADA GÖRE TERSTEN SIRALIYOR
                List<TBLOGRENCI> liste2 = db.TBLOGRENCI.OrderByDescending(p => p.AD).ToList();
                dataGridView1.DataSource = liste2;
            }

            if (radioButton3.Checked == true)
            {   //İlk 3 değeri getiren sorgu
                List<TBLOGRENCI> liste3 = db.TBLOGRENCI.OrderBy(p => p.AD).Take(3).ToList();
                dataGridView1.DataSource = liste3;
            }

            if(radioButton4.Checked == true)
            {   //ID=5 olanı getiren sorgu
                List<TBLOGRENCI> liste4 = db.TBLOGRENCI.Where(p => p.ID == 5).ToList();
                dataGridView1.DataSource = liste4;
            }

            if (radioButton5.Checked == true)
            {   //A ile başlayanları getiren sorgu
                List<TBLOGRENCI> liste5 = db.TBLOGRENCI.Where(p => p.AD.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste5;
            }

            if (radioButton6.Checked == true)
            {   //A ile başlayanları getiren sorgu
                List<TBLOGRENCI> liste6 = db.TBLOGRENCI.Where(p => p.AD.EndsWith("a")).ToList();
                dataGridView1.DataSource = liste6;
            }

            if (radioButton7.Checked == true)
            {   //Değer var mı yok mu onu kontrol ediyor
                bool deger = db.TBLDERSLER.Any();
                MessageBox.Show(deger.ToString(), "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (radioButton8.Checked == true)
            {   //Count yapıyor 
                int toplam = db.TBLOGRENCI.Count();
                MessageBox.Show(toplam.ToString(), "Toplam Öğrenci Sayısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}







