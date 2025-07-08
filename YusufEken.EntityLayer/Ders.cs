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
    [Table("tblDersler")]
    public class Ders : BaseModel
    {
        public string DersAd { get; set; }
        public int Teori { get; set; }
        public int Uygulama { get; set; }
        public int Kredi { get; set; }
        public int AKTS { get; set; }

        public int? PersonelId { get; set; }

        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        public int BolumId { get; set; }

        [ForeignKey("BolumId")]
        public virtual Bolum Bolum { get; set; }

        public int DonemId { get; set; }

        [ForeignKey("DonemId")]
        public virtual Donem Donem { get; set; }

        [NotMapped]
        public string TeoriUygulama { get { return this.Teori + " + " + this.Uygulama; } }
    }
}