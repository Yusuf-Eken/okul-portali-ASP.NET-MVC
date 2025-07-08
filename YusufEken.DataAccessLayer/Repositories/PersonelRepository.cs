using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YusufEken.DataAccessLayer.Repositories.Base;
using YusufEken.EntityLayer;

namespace YusufEken.DataAccessLayer.Repositories
{
    public class PersonelRepository : Repository<Personel>, IPersonelRepository
    {
        public PersonelRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
