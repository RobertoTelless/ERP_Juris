using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class SubsecaoRepository : RepositoryBase<SUBSECAO>, ISubsecaoRepository
    {
        public SUBSECAO CheckExist(SUBSECAO item, Int32? idAss)
        {
            IQueryable<SUBSECAO> query = Db.SUBSECAO;
            query = query.Where(p => p.SUSE_NM_NOME == item.SUSE_NM_NOME);
            query = query.Where(p => p.SECA_CD_ID == item.SECA_CD_ID);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.FirstOrDefault();
        }

        public SUBSECAO GetItemById(Int32 id)
        {
            IQueryable<SUBSECAO> query = Db.SUBSECAO;
            query = query.Where(p => p.SUSE_CD_ID == id);
            return query.FirstOrDefault();
        }

        public List<SUBSECAO> GetAllItens(Int32 idAss)
        {
            IQueryable<SUBSECAO> query = Db.SUBSECAO.Where(p => p.SUSE_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<SUBSECAO> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<SUBSECAO> query = Db.SUBSECAO;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<SUBSECAO> GetBySecao(Int32 idSecao)
        {
            IQueryable<SUBSECAO> query = Db.SUBSECAO.Where(p => p.SUSE_IN_ATIVO == 1);
            query = query.Where(p => p.SECA_CD_ID == idSecao);
            return query.ToList();
        }

        public List<SUBSECAO> ExecuteFilter(Int32 idSecao, String nome, String descricao, Int32 idAss)
        {
            List<SUBSECAO> lista = new List<SUBSECAO>();
            IQueryable<SUBSECAO> query = Db.SUBSECAO;
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.SUSE_NM_NOME.Contains(nome));
            }
            if (!String.IsNullOrEmpty(descricao))
            {
                query = query.Where(p => p.SUSE_DS_DESCRICAO.Contains(descricao));
            }
            if (idSecao != 0)
            {
                query = query.Where(p => p.SECA_CD_ID == idSecao);
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.SUSE_NM_NOME);
                lista = query.ToList<SUBSECAO>();
            }
            return lista;
        }
    }
}
