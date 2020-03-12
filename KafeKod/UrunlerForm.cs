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
        private KafeContext db;
        public UrunlerForm(KafeContext kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;
            dgvUrunler.DataSource = new BindingSource(db.Urunler.OrderBy(x => x.UrunAd).ToList(), null);
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAd.Text.Trim();

            if (urunAd == "")
            {
                MessageBox.Show("Lütfen Bir Ürün Adı Giriniz.");
                return;
            }

            db.Urunler.Add(new Urun
            {
                UrunAd = urunAd,
                BirimFiyat = nudBirimFiyat.Value
            });
            db.SaveChanges();
            dgvUrunler.DataSource = new BindingSource(db.Urunler.OrderBy(x => x.UrunAd).ToList(), null);

        }

        private void dgvUrunler_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Lütfen Geçerli Bir Değer Giriniz");
        }

        private void dgvUrunler_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Ürün Adı Düzenliyorsa
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


        // databound item satırla ilişkili nesneyi saklar

        private void dgvUrunler_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            Urun urun = (Urun)e.Row.DataBoundItem;

            if (urun.SiparisDetaylar.Count > 0)
            {
                MessageBox.Show("Bu ürün geçmiş siparişlerle ilişkili olduğu için silinemez");
                e.Cancel = true;
                return;
            }

            db.Urunler.Remove(urun);
            db.SaveChanges();
        }

        private void UrunlerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            txtUrunAd.Focus(); // datagridview'deki son değişikliğin kaydedilmesini tetiklemek için
        }

        private void dgvUrunler_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            db.SaveChanges();
        }
    }
}
