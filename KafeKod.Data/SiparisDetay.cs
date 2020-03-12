using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeKod.Data
{
    [Table("SiparisDetaylar")]
    public class SiparisDetay
    {
        public int Id { get; set; }

        [Required,MaxLength(50)]
        public string UrunAd { get; set; }

        public decimal BirimFiyat { get; set; }

        public int Adet { get; set; }
        public int UrunId { get; set; }    // bağlı olduğu foreign Id
        public int SiparisId { get; set; } // bağlı olduğu foreign Id
        public virtual Urun Urun { get; set; } // her detay bir ürüne ait olur
        public virtual Siparis Siparis { get; set; } // her detay bir siparişe bağlıdır

    }
}
