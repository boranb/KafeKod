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
        private KafeVeri db;
        private Siparis siparis;
        private BindingList<SiparisDetay> blSiparisDetaylar;


        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            db = kafeVeri;
            this.siparis = siparis;
            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            InitializeComponent();
            MasaNoGuncelle();
            TutarGuncelle();
            cboUrun.DataSource = db.Urunler; //.OrderBy(x=>x.UrunAd).ToList();
            //cboUrun.SelectedItem = null; açılışta ürün seçili gelmesi için boş bıraktık;
            dgvSiparisDetaylari.DataSource = blSiparisDetaylar;
        }

        private void TutarGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
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

            Urun secili = (Urun)cboUrun.SelectedItem;

            var sd = new SiparisDetay()
            {
                UrunAd = secili.UrunAd,
                BirimFiyat = secili.BirimFiyat,
                Adet = (int)nudAdet.Value
            };
            blSiparisDetaylar.Add(sd);
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
                siparis.OdenenTutar = siparis.ToplamTutar();
                Close();
            }
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
             
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
                blSiparisDetaylar.Remove(sipDetay);
            }
            TutarGuncelle();
        }
    }
}
