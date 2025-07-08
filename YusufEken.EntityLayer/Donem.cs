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
    [Table("tblDonemler")]

    public class Donem:BaseModel
    {
        public string DonemAd { get; set; }

        public virtual ICollection<Ders> Dersler { get; set; }

        public Donem()
        {
            this.Dersler = new HashSet<Ders>();
        }
    }
}
