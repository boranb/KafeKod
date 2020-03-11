using KafeKod.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KafeKod
{
    public partial class SiparisForm : Form
    {
        public event EventHandler<MasaTasimaEventArgs> MasaTasiniyor;
        
        private KafeContext db;
        private Siparis siparis;


        public SiparisForm(KafeContext kafeVeri, Siparis siparis)
        {
            db = kafeVeri;
            this.siparis = siparis;
            InitializeComponent();
            dgvSiparisDetaylari.AutoGenerateColumns = false; // bütün sütunların otomatik yüklenmesini engeller
            MasaNolariYukle();
            MasaNoGuncelle();
            TutarGuncelle();
            cboUrun.DataSource = db.Urunler.ToList(); //.OrderBy(x=>x.UrunAd).ToList();
            //cboUrun.SelectedItem = null; açılışta ürün seçili gelmesi için boş bıraktık;
            dgvSiparisDetaylari.DataSource = siparis.SiparisDetaylar;
        }

        private void MasaNolariYukle()
        {
            cboMasaNo.Items.Clear();

            for (int i = 1; i <= Properties.Settings.Default.MasaAdet; i++)
            {
                if (!db.Siparisler.Any(x => x.MasaNo == i && x.Durum == SiparisDurum.Aktif))
                {
                    cboMasaNo.Items.Add(i);
                }
            }
        }

        private void TutarGuncelle()
        {
            lblOdemeTutari.Text = siparis.SiparisDetaylar.Sum(x => x.Adet * x.BirimFiyat).ToString("0.00") + "₺";
        }

        private void MasaNoGuncelle()
        {
            Text = "Masa " + siparis.MasaNo;
            lblMasaNo.Text = siparis.MasaNo.ToString("00");
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {

            if (cboUrun.SelectedItem == null)
            {
                MessageBox.Show("Lütfen Bir Ürün Seçiniz!");
                return;
            }

            Urun seciliUrun = (Urun)cboUrun.SelectedItem;

            var sd = new SiparisDetay()
            {
                UrunId = seciliUrun.Id,
                UrunAd = seciliUrun.UrunAd,
                BirimFiyat = seciliUrun.BirimFiyat,
                Adet = (int)nudAdet.Value
            };
            siparis.SiparisDetaylar.Add(sd);
            db.SaveChanges();
            dgvSiparisDetaylari.DataSource = new BindingSource(siparis.SiparisDetaylar,null);
            dgvSiparisDetaylari.DataSource = siparis.SiparisDetaylar;
            cboUrun.SelectedIndex = 0; // 0'ıncı index seçili gelmesi için boş gelmesi için // SelectedItem = null;
            nudAdet.Value = 1;
            TutarGuncelle();
        }

        private void btnAnaSayfa_Click(object sender, EventArgs e) => Close();

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Siparis İptal Edilecektir Onaylıyor Musunuz ?", "Siparis İptal Onayı"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Warning
                , MessageBoxDefaultButton.Button2);

            if (dr ==DialogResult.Yes)
            {
                siparis.Durum = SiparisDurum.Iptal;
                siparis.KapanisZamani = DateTime.Now;;
                db.SaveChanges();
                Close();
            }
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Odeme Alındıysa Masanın Hesabı Kapatılacaktır Onaylıyor Musunuz ?", "Masa Hesabı Kapatma Onayı"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Warning
                , MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                siparis.Durum = SiparisDurum.Odendi;
                siparis.KapanisZamani = DateTime.Now;;
                siparis.OdenenTutar = siparis.SiparisDetaylar.Sum(x => x.Adet * x.BirimFiyat);
                db.SaveChanges();
                Close();
            }
        }


        private void dgvSiparisDetaylari_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int rowIndex = dgvSiparisDetaylari.HitTest(e.X, e.Y).RowIndex;
                if (rowIndex > -1)
                {
                dgvSiparisDetaylari.ClearSelection();
                dgvSiparisDetaylari.Rows[rowIndex].Selected = true;
                cmsSiparisDetayClick.Show(MousePosition);
                }
            }
        }

        private void tsmiSiparisDetaySil_Click(object sender, EventArgs e)
        {
            if (dgvSiparisDetaylari.SelectedRows.Count > 0)
            {
                var seciliSatir = dgvSiparisDetaylari.SelectedRows[0];
                var sipDetay = (SiparisDetay) seciliSatir.DataBoundItem;
                siparis.SiparisDetaylar.Remove(sipDetay);
                db.SaveChanges();

            }
            TutarGuncelle();
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            if (cboMasaNo.SelectedItem == null)
            {
                MessageBox.Show("Lütfen Hedef Masa No'yu Seçiniz!");
                return;
            }

            int eskiMasaNo = siparis.MasaNo;
            int hedefMasaNo = (int) cboMasaNo.SelectedItem;


            if (MasaTasiniyor != null)
            {
                var args = new MasaTasimaEventArgs()
                {
                    TasinanSiparis = siparis,
                    EskiMasaNo = eskiMasaNo,
                    YeniMasaNo = hedefMasaNo
                };
                MasaTasiniyor(this, args);
            }

            siparis.MasaNo = hedefMasaNo;
            db.SaveChanges();
            MasaNoGuncelle();
            MasaNolariYukle();


        }

    }

    public class MasaTasimaEventArgs : EventArgs
    {
        public Siparis TasinanSiparis { get; set; }
        public int EskiMasaNo { get; set; }
        public int YeniMasaNo { get; set; }
    }

}
