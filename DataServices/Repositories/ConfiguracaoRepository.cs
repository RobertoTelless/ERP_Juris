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
    public class ConfiguracaoRepository : RepositoryBase<CONFIGURACAO>, IConfiguracaoRepository
    {
        public CONFIGURACAO GetItemById(Int32 id)
        {
            IQueryable<CONFIGURACAO> query = Db.CONFIGURACAO;
            query = query.Where(p => p.ASSI_CD_ID == id);
            return query.FirstOrDefault();
        }

        public List<CONFIGURACAO> GetAllItems()
        {
            IQueryable<CONFIGURACAO> query = Db.CONFIGURACAO;
            return query.ToList();
        }
    }
}
 