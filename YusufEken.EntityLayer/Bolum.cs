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
    [Table("tblBolumler")]

    public class Bolum: BaseModel
    {
        public string BolumAd { get; set; }

        public virtual ICollection<Personel> Personeller{ get; set; }
        public virtual ICollection<Ders> Dersler { get; set; }

        public Bolum()
        {
            this.Personeller = new HashSet<Personel>();
            this.Dersler = new HashSet<Ders>();
        }
    }
}
