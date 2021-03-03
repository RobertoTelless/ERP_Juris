using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface ISecaoService : IServiceBase<SECAO>
    {
        Int32 Create(SECAO item, LOG log);
        Int32 Create(SECAO item);
        Int32 Edit(SECAO item, LOG log);
        Int32 Edit(SECAO item);
        Int32 Delete(SECAO item, LOG log);

        SECAO CheckExist(SECAO item, Int32? idAss);
        List<SECAO> GetAllItens(Int32 idAss);
        List<SECAO> GetAllItensAdm(Int32 idAss);
        List<SECAO> GetByRegiao(Int32 idRegiao);
        SECAO GetItemById(Int32 id);
        List<SECAO> ExecuteFilter(Int32 idRegiao, String nome, String descricao, Int32 idAss);
    }
}
