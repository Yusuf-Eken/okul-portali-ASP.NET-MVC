using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YusufEken.DataAccessLayer.Repositories.Base;
using YusufEken.EntityLayer;

namespace YusufEken.DataAccessLayer.Repositories
{
    public class KullaniciRepository : Repository<Kullanici>, IKullaniciRepository
    {
        public KullaniciRepository(DatabaseContext context) : base(context)
        {
        }

        public override Kullanici Add(Kullanici item)
        {
            if (GetAll().Any(x => x.Eposta.Equals(item.Eposta, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"{item.Eposta} e-posta adresine sahip bir kullanıcı zaten mevcut!");
            return base.Add(item);
        }

        public override void Update(Kullanici item)
        {
            if (GetAll().Any(x => x.Id != item.Id && x.Eposta.Equals(item.Eposta, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"{item.Eposta} e-posta adresi başka bir kullanıcı tarafından kullanılıyor!");
            base.Update(item);
        }

        public bool Login(string eposta, string parola)
        {
            return GetAll().Any(x => x.Eposta.Equals(eposta, StringComparison.OrdinalIgnoreCase) && x.Parola == parola);
        }
    }
}
