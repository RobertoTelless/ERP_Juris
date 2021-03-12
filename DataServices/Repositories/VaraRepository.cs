using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class VaraRepository : RepositoryBase<VARA>, IVaraRepository
    {
        public VARA CheckExist(VARA item, Int32? idAss)
        {
            IQueryable<VARA> query = Db.VARA;
            query = query.Where(p => p.VARA_NM_NOME == item.VARA_NM_NOME);
            query = query.Where(p => p.FORO_CD_ID == item.FORO_CD_ID);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.FirstOrDefault();
        }

        public VARA GetItemById(Int32 id)
        {
            IQueryable<VARA> query = Db.VARA;
            query = query.Where(p => p.VARA_CD_ID == id);
            return query.FirstOrDefault();
        }

        public List<VARA> GetAllItens(Int32 idAss)
        {
            IQueryable<VARA> query = Db.VARA.Where(p => p.VARA_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<VARA> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<VARA> query = Db.VARA;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<VARA> GetByForo(Int32 idForo)
        {
            IQueryable<VARA> query = Db.VARA.Where(p => p.VARA_IN_ATIVO == 1);
            query = query.Where(p => p.FORO_CD_ID == idForo);
            return query.ToList();
        }

        public List<VARA> ExecuteFilter(Int32 idForo, String nome, String descricao, String juiz, String cidade, String bairro, Int32 uf, Int32 idAss)
        {
            List<VARA> lista = new List<VARA>();
            IQueryable<VARA> query = Db.VARA;
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.VARA_NM_NOME.Contains(nome));
            }
            if (!String.IsNullOrEmpty(descricao))
            {
                query = query.Where(p => p.VARA_DS_DESCRICAO.Contains(nome));
            }
            if (idForo != 0)
            {
                query = query.Where(p => p.FORO_CD_ID == idForo);
            }
            if (!String.IsNullOrEmpty(bairro))
            {
                query = query.Where(p => p.VARA_NM_BAIRRO.Contains(bairro));
            }
            if (uf != 0)
            {
                query = query.Where(p => p.UF_CD_ID == uf);
            }
            if (!String.IsNullOrEmpty(cidade))
            {
                query = query.Where(p => p.VARA_NM_CIDADE.Contains(cidade));
            }
            if (!String.IsNullOrEmpty(juiz))
            {
                query = query.Where(p => p.VARA_NM_JUIZ.Contains(juiz));
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.VARA_NM_NOME);
                lista = query.ToList<VARA>();
            }
            return lista;
        }
    }
}
