using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;
using CrossCutting;

namespace DataServices.Repositories
{
    public class FornecedorContatoRepository : RepositoryBase<FORNECEDOR_CONTATO>, IFornecedorContatoRepository
    {
        public List<FORNECEDOR_CONTATO> GetAllItens(Int32 idAss)
        {
            return Db.FORNECEDOR_CONTATO.ToList();
        }

        public FORNECEDOR_CONTATO GetItemById(Int32 id)
        {
            IQueryable<FORNECEDOR_CONTATO> query = Db.FORNECEDOR_CONTATO.Where(p => p.FOCO_CD_ID == id);
            return query.FirstOrDefault();
        }
    }
}
