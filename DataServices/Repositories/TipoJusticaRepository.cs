using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class TipoJusticaRepository : RepositoryBase<TIPO_JUSTICA>, ITipoJusticaRepository
    {
        public TIPO_JUSTICA CheckExist(TIPO_JUSTICA item, Int32? idAss)
        {
            IQueryable<TIPO_JUSTICA> query = Db.TIPO_JUSTICA;
            query = query.Where(p => p.TIJU_NM_NOME == item.TIJU_NM_NOME);
            return query.FirstOrDefault();
        }

        public TIPO_JUSTICA GetItemById(Int32 id)
        {
            IQueryable<TIPO_JUSTICA> query = Db.TIPO_JUSTICA;
            query = query.Where(p => p.TIJU_CD_ID == id);
            query = query.Include(p => p.REGIAO_JUSTICA);
            return query.FirstOrDefault();
        }

        public List<TIPO_JUSTICA> GetAllItens(Int32 idAss)
        {
            IQueryable<TIPO_JUSTICA> query = Db.TIPO_JUSTICA.Where(p => p.TIJU_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<TIPO_JUSTICA> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<TIPO_JUSTICA> query = Db.TIPO_JUSTICA;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<TIPO_JUSTICA> ExecuteFilter(String nome, String descricao, Int32 idAss)
        {
            List<TIPO_JUSTICA> lista = new List<TIPO_JUSTICA>();
            IQueryable<TIPO_JUSTICA> query = Db.TIPO_JUSTICA;
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.TIJU_NM_NOME.Contains(nome));
            }
            if (!String.IsNullOrEmpty(descricao))
            {
                query = query.Where(p => p.TIJU_DS_DESCRICAO.Contains(descricao));
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.TIJU_NM_NOME);
                query = query.Include(p => p.REGIAO_JUSTICA);
                lista = query.ToList<TIPO_JUSTICA>();
            }
            return lista;
        }
    }
}
