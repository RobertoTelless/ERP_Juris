using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface ISubsecaoService : IServiceBase<SUBSECAO>
    {
        Int32 Create(SUBSECAO item, LOG log);
        Int32 Create(SUBSECAO item);
        Int32 Edit(SUBSECAO item, LOG log);
        Int32 Edit(SUBSECAO item);
        Int32 Delete(SUBSECAO item, LOG log);

        SUBSECAO CheckExist(SUBSECAO item, Int32? idAss);
        List<SUBSECAO> GetAllItens(Int32 idAss);
        List<SUBSECAO> GetBySecao(Int32 idSecao);
        SUBSECAO GetItemById(Int32 id);
        List<SUBSECAO> ExecuteFilter(Int32 idSecao, String nome, String descricao, Int32 idAss);
    }
}
