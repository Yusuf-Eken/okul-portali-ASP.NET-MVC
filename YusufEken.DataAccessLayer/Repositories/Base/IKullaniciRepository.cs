using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YusufEken.EntityLayer;

namespace YusufEken.DataAccessLayer.Repositories.Base
{
    public interface IKullaniciRepository : IRepository<Kullanici>
    {
        bool Login(string eposta, string parola);
    }
}
