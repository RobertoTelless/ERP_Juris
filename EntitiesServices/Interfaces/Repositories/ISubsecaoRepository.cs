using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface ISubsecaoRepository : IRepositoryBase<SUBSECAO>
    {
        SUBSECAO CheckExist(SUBSECAO item, Int32? idAss);
        List<SUBSECAO> GetAllItens(Int32 idAss);
        List<SUBSECAO> GetAllItensAdm(Int32 idAss);
        List<SUBSECAO> GetBySecao(Int32 idSecao);
        SUBSECAO GetItemById(Int32 id);
        List<SUBSECAO> ExecuteFilter(Int32 idSecao, String nome, String descricao, Int32 idAss);
    }
}
