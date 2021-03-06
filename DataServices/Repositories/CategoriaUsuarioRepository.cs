using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using System.Data.Entity;

namespace DataServices.Repositories
{
    public class CategoriaUsuarioRepository : RepositoryBase<CATEGORIA_USUARIO>, ICategoriaUsuarioRepository
    {
        public CATEGORIA_USUARIO GetItemById(Int32 id)
        {
            IQueryable<CATEGORIA_USUARIO> query = Db.CATEGORIA_USUARIO;
            query = query.Where(p => p.CAUS_CD_ID == id);
            return query.FirstOrDefault();
        }

        public CATEGORIA_USUARIO CheckExist(CATEGORIA_USUARIO item, Int32? idAss)
        {
            IQueryable<CATEGORIA_USUARIO> query = Db.CATEGORIA_USUARIO;
            query = query.Where(p => p.CAUS_NM_NOME == item.CAUS_NM_NOME);
            return query.FirstOrDefault();
        }

        public List<CATEGORIA_USUARIO> GetAllItens(Int32 idAss)
        {
            IQueryable<CATEGORIA_USUARIO> query = Db.CATEGORIA_USUARIO.Where(p => p.CAUS_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }

        public List<CATEGORIA_USUARIO> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<CATEGORIA_USUARIO> query = Db.CATEGORIA_USUARIO;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            return query.ToList();
        }
    }
}
