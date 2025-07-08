using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YusufEken.EntityLayer.Base;

namespace YusufEken.EntityLayer
{
    [Table("tblKullanıcılar")]
    public class Kullanici :BaseModel
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Eposta { get; set; }
        public string Parola { get; set; }

        [NotMapped]
        public string ParolaTekrar { get; set; }

        [NotMapped]
        public string AdSoyad { get { return this.Ad + " " + this.Soyad; } }

    }
}
