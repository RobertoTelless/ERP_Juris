using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class ForoRepository : RepositoryBase<FORO>, IForoRepository
    {
        public FORO CheckExist(FORO item, Int32? idAss)
        {
            IQueryable<FORO> query = Db.FORO;
            query = query.Where(p => p.FORO_NM_NOME == item.FORO_NM_NOME);
            query = query.Where(p => p.CICO_CD_ID == item.CICO_CD_ID);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.FirstOrDefault();
        }

        public FORO GetItemById(Int32 id)
        {
            IQueryable<FORO> query = Db.FORO;
            query = query.Where(p => p.FORO_CD_ID == id);
            query = query.Include(p => p.VARA);
            return query.FirstOrDefault();
        }

        public List<FORO> GetAllItens(Int32 idAss)
        {
            IQueryable<FORO> query = Db.FORO.Where(p => p.FORO_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<FORO> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<FORO> query = Db.FORO;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<FORO> GetByCidade(Int32 idCidade)
        {
            IQueryable<FORO> query = Db.FORO.Where(p => p.FORO_IN_ATIVO == 1);
            query = query.Where(p => p.CICO_CD_ID == idCidade);
            return query.ToList();
        }

        public List<FORO> ExecuteFilter(Int32 idCidade, String nome, String descricao, String bairro, Int32 uf, Int32 idAss)
        {
            List<FORO> lista = new List<FORO>();
            IQueryable<FORO> query = Db.FORO;
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.FORO_NM_NOME.Contains(nome));
            }
            if (!String.IsNullOrEmpty(descricao))
            {
                query = query.Where(p => p.FORO_DS_DESCRICAO.Contains(nome));
            }
            if (idCidade != 0)
            {
                query = query.Where(p => p.CICO_CD_ID == idCidade);
            }
            if (!String.IsNullOrEmpty(bairro))
            {
                query = query.Where(p => p.FORO_NM_BAIRRO.Contains(bairro));
            }
            if (uf != 0)
            {
                query = query.Where(p => p.UF_CD_ID == uf);
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.FORO_NM_NOME);
                query = query.Include(p => p.VARA);
                lista = query.ToList<FORO>();
            }
            return lista;
        }
    }
}
