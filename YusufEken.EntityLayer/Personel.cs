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
    [Table("tblPersoneller")]

    public class Personel :BaseModel
    {
        public string Unvan { get; set; }
        public string ImageUrl { get; set; }
        public string PerAd { get; set; }
        public string PerSoyad { get; set; }
        public string PerEposta { get; set; }
        public int DahiliNo { get; set; }
        public int BolumId { get; set; }
        [ForeignKey("BolumId")]
        public virtual Bolum Bolum { get; set; }

    }
}
