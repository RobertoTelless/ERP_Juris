using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface IRegiaoJusticaRepository : IRepositoryBase<REGIAO_JUSTICA>
    {
        REGIAO_JUSTICA CheckExist(REGIAO_JUSTICA item, Int32? idAss);
        List<REGIAO_JUSTICA> GetAllItens(Int32 idAss);
        List<REGIAO_JUSTICA> GetAllItensAdm(Int32 idAss);
        List<REGIAO_JUSTICA> GetByTipo(Int32 idTipo);
        REGIAO_JUSTICA GetItemById(Int32 id);
        List<REGIAO_JUSTICA> ExecuteFilter(Int32 idTipo, String nome, String descricao, Int32 idAss);
    }
}
