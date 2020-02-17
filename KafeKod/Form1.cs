using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KafeKod.Properties;

namespace KafeKod
{
    public partial class Form1 : Form
    {
        private int masaAdet = 20;

        public Form1()
        {
            InitializeComponent();
            MasalariOlustur();
        }

        private void MasalariOlustur()
        {
            #region Resim Listesinin Tanımlanması
            ImageList resimListesi = new ImageList();
            resimListesi.Images.Add("bos", Resources.masabos);
            resimListesi.Images.Add("dolu", Resources.masadolu);
            resimListesi.ImageSize = new Size(64, 64);
            lvwMasalar.LargeImageList = resimListesi; 
            #endregion

            int masaNo;
            ListViewItem lvi;
            for (int i = 0; i < masaAdet; i++)
            {
                masaNo = i + 1;
                lvi = new ListViewItem("Masa" + masaNo);
                lvwMasalar.Items.Add(lvi);
                lvi.Tag = masaNo;
            }
        }
    }
}
