using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YusufEken.EntityLayer;

namespace YusufEken.DataAccessLayer
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Personel> Personeller { get; set; }
        public DbSet<Donem> Donemler { get; set; }

        public DatabaseContext() : base("YusuffEkennDB")
        {
            Database.SetInitializer(new DatabaseInitializer());
        }   
    }
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            base.Seed(context);

            var donem1 = new Donem { DonemAd = "1. Yarıyıl" };
            var donem2 = new Donem { DonemAd = "2. Yarıyıl" };

            context.Donemler.Add(donem1);
            context.Donemler.Add(donem2);

            var bolum1 = new Bolum { BolumAd = "Bilgisayar Programcılığı" };
            var bolum2 = new Bolum { BolumAd = "Grafik Tasarım" };

            context.Bolumler.Add(bolum1);
            context.Bolumler.Add(bolum2);

            var per1 = new Personel
            {
                Unvan = "Öğr. Gör.",
                PerAd = "Bilal Abdüsselam",
                PerSoyad = "İbalar",
                PerEposta = "bilal@hotmail.com",
                DahiliNo = 1,
                ImageUrl = "",
                Bolum = bolum1
            };

            var per2 = new Personel
            {
                Unvan = "Doç. Dr.",
                PerAd = "Furkan",
                PerSoyad = "Kara",
                PerEposta = "furkan@gmail.com",
                DahiliNo = 2,
                ImageUrl = "",
                Bolum = bolum2
            };

            context.Personeller.Add(per1);
            context.Personeller.Add(per2);

            var ders1 = new Ders
            {
                DersAd = "Programlama Temelleri",
                Teori = 2,
                Uygulama = 2,
                Kredi = 3,
                AKTS = 5,
                Personel = per1,
                Bolum = bolum1,
                Donem = donem1
            };

            var ders2 = new Ders
            {
                DersAd = "Web Tasarımı",
                Teori = 2,
                Uygulama = 1,
                Kredi = 3,
                AKTS = 4,
                Personel = per2,
                Bolum = bolum2,
                Donem = donem2
            };

            context.Dersler.Add(ders1);
            context.Dersler.Add(ders2);

            var kullanici1 = new Kullanici
            {
                Ad = "Yusuf",
                Soyad = "Eken",
                Eposta = "yusuf@gmail.com",
                Parola = "123",
                ParolaTekrar = "123"
            };

            context.Kullanicilar.Add(kullanici1);
        }
    }
}