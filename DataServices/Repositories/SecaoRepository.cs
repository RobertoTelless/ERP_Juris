using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class SecaoRepository : RepositoryBase<SECAO>, ISecaoRepository
    {
        public SECAO CheckExist(SECAO item, Int32? idAss)
        {
            IQueryable<SECAO> query = Db.SECAO;
            query = query.Where(p => p.SECA_NM_NOME == item.SECA_NM_NOME);
            query = query.Where(p => p.REJU_CD_ID == item.REJU_CD_ID);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.FirstOrDefault();
        }

        public SECAO GetItemById(Int32 id)
        {
            IQueryable<SECAO> query = Db.SECAO;
            query = query.Where(p => p.SECA_CD_ID == id);
            query = query.Include(p => p.SUBSECAO);
            return query.FirstOrDefault();
        }

        public List<SECAO> GetAllItens(Int32 idAss)
        {
            IQueryable<SECAO> query = Db.SECAO.Where(p => p.SECA_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<SECAO> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<SECAO> query = Db.SECAO;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<SECAO> GetByRegiao(Int32 idRegiao)
        {
            IQueryable<SECAO> query = Db.SECAO.Where(p => p.SECA_IN_ATIVO == 1);
            query = query.Where(p => p.REJU_CD_ID == idRegiao);
            return query.ToList();
        }

        public List<SECAO> ExecuteFilter(Int32 idRegiao, String nome, String descricao, Int32 idAss)
        {
            List<SECAO> lista = new List<SECAO>();
            IQueryable<SECAO> query = Db.SECAO;
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.SECA_NM_NOME.Contains(nome));
            }
            if (!String.IsNullOrEmpty(descricao))
            {
                query = query.Where(p => p.SECA_DS_DESCRICAO.Contains(descricao));
            }
            if (idRegiao != 0)
            {
                query = query.Where(p => p.REJU_CD_ID == idRegiao);
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.SECA_NM_NOME);
                query = query.Include(p => p.SUBSECAO);
                lista = query.ToList<SECAO>();
            }
            return lista;
        }
    }
}
