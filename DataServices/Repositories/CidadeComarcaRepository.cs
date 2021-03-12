using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class CidadeComarcaRepository : RepositoryBase<CIDADE_COMARCA>, ICidadeComarcaRepository
    {
        public CIDADE_COMARCA CheckExist(CIDADE_COMARCA item, Int32? idAss)
        {
            IQueryable<CIDADE_COMARCA> query = Db.CIDADE_COMARCA;
            query = query.Where(p => p.CICO_NM_NOME == item.CICO_NM_NOME);
            return query.FirstOrDefault();
        }

        public CIDADE_COMARCA GetItemById(Int32 id)
        {
            IQueryable<CIDADE_COMARCA> query = Db.CIDADE_COMARCA;
            query = query.Where(p => p.CICO_CD_ID == id);
            query = query.Include(p => p.FORO);
            return query.FirstOrDefault();
        }

        public List<CIDADE_COMARCA> GetAllItens(Int32 idAss)
        {
            IQueryable<CIDADE_COMARCA> query = Db.CIDADE_COMARCA.Where(p => p.CICO_IN_ATIVO == "1");
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<CIDADE_COMARCA> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<CIDADE_COMARCA> query = Db.CIDADE_COMARCA;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<CIDADE_COMARCA> ExecuteFilter(String nome, Int32 uf, Int32 idAss)
        {
            List<CIDADE_COMARCA> lista = new List<CIDADE_COMARCA>();
            IQueryable<CIDADE_COMARCA> query = Db.CIDADE_COMARCA;
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.CICO_NM_NOME.Contains(nome));
            }
            if (uf != 0)
            {
                query = query.Where(p => p.UF_CD_ID == uf);
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.CICO_NM_NOME);
                lista = query.ToList<CIDADE_COMARCA>();
            }
            return lista;
        }
    }
}
