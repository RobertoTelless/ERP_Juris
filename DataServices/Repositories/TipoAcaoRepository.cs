using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class TipoAcaoRepository : RepositoryBase<TIPO_ACAO>, ITipoAcaoRepository
    {
        public TIPO_ACAO CheckExist(TIPO_ACAO item, Int32? idAss)
        {
            IQueryable<TIPO_ACAO> query = Db.TIPO_ACAO;
            query = query.Where(p => p.TIAC_NM_NOME == item.TIAC_NM_NOME);
            return query.FirstOrDefault();
        }

        public TIPO_ACAO GetItemById(Int32 id)
        {
            IQueryable<TIPO_ACAO> query = Db.TIPO_ACAO;
            query = query.Where(p => p.TIAC_CD_ID == id);
            query = query.Include(p => p.ASSINANTE);
            return query.FirstOrDefault();
        }

        public List<TIPO_ACAO> GetAllItens(Int32 idAss)
        {
            IQueryable<TIPO_ACAO> query = Db.TIPO_ACAO.Where(p => p.TIAC_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<TIPO_ACAO> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<TIPO_ACAO> query = Db.TIPO_ACAO;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<TIPO_ACAO> ExecuteFilter(String nome, String descricao, Int32 idAss)
        {
            List<TIPO_ACAO> lista = new List<TIPO_ACAO>();
            IQueryable<TIPO_ACAO> query = Db.TIPO_ACAO;
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.TIAC_NM_NOME.Contains(nome));
            }
            if (!String.IsNullOrEmpty(descricao))
            {
                query = query.Where(p => p.TIAC_DS_DESCRICAO.Contains(descricao));
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.TIAC_NM_NOME);
                lista = query.ToList<TIPO_ACAO>();
            }
            return lista;
        }
    }
}
