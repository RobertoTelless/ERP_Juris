using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class RegiaoJusticaRepository : RepositoryBase<REGIAO_JUSTICA>, IRegiaoJusticaRepository
    {
        public REGIAO_JUSTICA CheckExist(REGIAO_JUSTICA item, Int32? idAss)
        {
            IQueryable<REGIAO_JUSTICA> query = Db.REGIAO_JUSTICA;
            query = query.Where(p => p.REJU_NM_NOME == item.REJU_NM_NOME);
            query = query.Where(p => p.TIJU_CD_ID == item.TIJU_CD_ID);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.FirstOrDefault();
        }

        public REGIAO_JUSTICA GetItemById(Int32 id)
        {
            IQueryable<REGIAO_JUSTICA> query = Db.REGIAO_JUSTICA;
            query = query.Where(p => p.REJU_CD_ID == id);
            query = query.Include(p => p.SECAO);
            return query.FirstOrDefault();
        }

        public List<REGIAO_JUSTICA> GetAllItens(Int32 idAss)
        {
            IQueryable<REGIAO_JUSTICA> query = Db.REGIAO_JUSTICA.Where(p => p.REJU_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<REGIAO_JUSTICA> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<REGIAO_JUSTICA> query = Db.REGIAO_JUSTICA;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<REGIAO_JUSTICA> GetByTipo(Int32 idTipo)
        {
            IQueryable<REGIAO_JUSTICA> query = Db.REGIAO_JUSTICA.Where(p => p.REJU_IN_ATIVO == 1);
            query = query.Where(p => p.TIJU_CD_ID == idTipo);
            return query.ToList();
        }

        public List<REGIAO_JUSTICA> ExecuteFilter(Int32 idTipo, String nome, String descricao, Int32 idAss)
        {
            List<REGIAO_JUSTICA> lista = new List<REGIAO_JUSTICA>();
            IQueryable<REGIAO_JUSTICA> query = Db.REGIAO_JUSTICA;
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.REJU_NM_NOME.Contains(nome));
            }
            if (!String.IsNullOrEmpty(descricao))
            {
                query = query.Where(p => p.REJU_DS_DESCRICAO.Contains(descricao));
            }
            if (idTipo != 0)
            {
                query = query.Where(p => p.TIJU_CD_ID == idTipo);
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.REJU_NM_NOME);
                query = query.Include(p => p.SECAO);
                lista = query.ToList<REGIAO_JUSTICA>();
            }
            return lista;
        }
    }
}
