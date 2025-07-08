using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YusufEken.DataAccessLayer.Repositories.Base;
using YusufEken.EntityLayer;

namespace YusufEken.DataAccessLayer.Repositories
{
    public class DersRepository : Repository<Ders>, IDersRepository
    {
        public DersRepository(DatabaseContext context) : base(context) { }
    }
}
