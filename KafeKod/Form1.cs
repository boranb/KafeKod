using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KafeKod.Data;
using KafeKod.Properties;

namespace KafeKod
{
    public partial class Form1 : Form
    {
        private KafeVeri db;
        private int masaAdet = 20;

        public Form1()
        {
            db = new KafeVeri();
            OrnekVerileriYukle();
            InitializeComponent();
            MasalariOlustur();
        }

        private void OrnekVerileriYukle()
        {
            db.Urunler = new List<Urun>()
            {
                new Urun(){UrunAd = "Kola", BirimFiyat = 6.99m},
                new Urun(){UrunAd = "Çay", BirimFiyat = 9.99m}
            };
        }

        private void MasalariOlustur()
        {

            #region ListView Imajlarının Hazırlanması
            ImageList il = new ImageList();
            il.Images.Add("bos", Properties.Resources.masabos);
            il.Images.Add("dolu", Properties.Resources.masadolu);
            il.ImageSize = new Size(64, 64);
            lvwMasalar.LargeImageList = il;
            #endregion



            ListViewItem lvi;

            for (int masaNo = 1; masaNo <= masaAdet; masaNo++)
            {
                lvi = new ListViewItem("Masa " + masaNo);
                lvi.Tag = masaNo;
                lvi.ImageKey = "bos";
                lvwMasalar.Items.Add(lvi);
            }
        }

        private void lvwMasalar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var lvi = lvwMasalar.SelectedItems[0];
                    lvwMasalar.SelectedItems[0].ImageKey = "dolu";

                Siparis sip;
                // masada doluysa olanı al, boşsa yeni oluştur
                if (lvi.Tag is Siparis)
                {
                    sip = (Siparis)lvi.Tag;
                }
                else
                {
                    sip = new Siparis();
                    sip.MasaNo = (int) lvi.Tag;
                    sip.AcilisZamani = DateTime.Now;
                    lvi.Tag = sip;
                    db.AktifSiparisler.Add(sip);
                }


                SiparisForm frmSiparis = new SiparisForm(db,sip);
                frmSiparis.ShowDialog();


                if (sip.Durum != SiparisDurum.Aktif)
                {
                    lvi.Tag = sip.MasaNo;
                    lvi.ImageKey = "bos";
                    db.AktifSiparisler.Remove(sip);
                    db.GecmisSiparisler.Add(sip);
                }
            }
        }

        public void MasaTasi(int kaynak, int hedef)
        {
            foreach (ListViewItem masa in lvwMasalar.Items)
            {
                if ((int)masa.Tag == kaynak)
                    masa.ImageKey = "bos";

                if ((int)masa.Tag == hedef)
                    masa.ImageKey = "dolu";
            }
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            var frm = new GecmisSiparislerForm(db);
            frm.ShowDialog();
        }
    }
}

