using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using ModelServices.Interfaces.Repositories;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class EmpresaRepository : RepositoryBase<EMPRESA>, IEmpresaRepository
    {
        public EMPRESA GetItemById(Int32 id)
        {
            IQueryable<EMPRESA> query = Db.EMPRESA;
            query = query.Where(p => p.ASSI_CD_ID == id);
            return query.FirstOrDefault();
        }

        public List<EMPRESA> GetAllItems()
        {
            IQueryable<EMPRESA> query = Db.EMPRESA;
            return query.ToList();
        }
    }
}
 