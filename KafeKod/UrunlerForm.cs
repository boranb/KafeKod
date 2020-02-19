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

namespace KafeKod
{
    public partial class UrunlerForm : Form
    {
        private KafeVeri db;
        private BindingList<Urun> blUrunler;
        public UrunlerForm(KafeVeri kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;
            blUrunler = new BindingList<Urun>(db.Urunler);
            dgvUrunler.DataSource = blUrunler;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAd.Text.Trim();

            if (urunAd == "")
            {
                MessageBox.Show("Lütfen Bir Ürün Adı Giriniz.");
                return;
            }

            blUrunler.Add(new Urun
            {
                UrunAd = urunAd,
                BirimFiyat = nudBirimFiyat.Value
            });
            db.Urunler.Sort();

        }

        private void dgvUrunler_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Lütfen Geçerli Bir Değer Giriniz");
        }

        private void dgvUrunler_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (e.FormattedValue.ToString().Trim() == "")
                {
                    dgvUrunler.Rows[e.RowIndex].ErrorText = "Ürün Ad Boş Geçilemez!";
                    e.Cancel = true;
                }
                else
                {
                    dgvUrunler.Rows[e.RowIndex].ErrorText = "";
                }
            }
        }
    }
}
