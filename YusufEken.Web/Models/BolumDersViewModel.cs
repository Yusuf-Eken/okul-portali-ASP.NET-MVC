using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YusufEken.EntityLayer;

namespace YusufEken.Web.Models
{
    public class BolumDersViewModel
    {
        public Donem Donem { get; set; }
        public List<Ders> Dersler { get; set; }
        public int ToplamKredi { get; set; }
        public int ToplamAKTS { get; set; }
    }
}