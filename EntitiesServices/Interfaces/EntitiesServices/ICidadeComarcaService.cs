using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface ICidadeComarcaService : IServiceBase<CIDADE_COMARCA>
    {
        Int32 Create(CIDADE_COMARCA item, LOG log);
        Int32 Create(CIDADE_COMARCA item);
        Int32 Edit(CIDADE_COMARCA item, LOG log);
        Int32 Edit(CIDADE_COMARCA item);
        Int32 Delete(CIDADE_COMARCA item, LOG log);

        CIDADE_COMARCA CheckExist(CIDADE_COMARCA item, Int32? idAss);
        List<CIDADE_COMARCA> GetAllItens(Int32 idAss);
        List<CIDADE_COMARCA> GetAllItensAdm(Int32 idAss);
        List<UF> GetAllUF();
        CIDADE_COMARCA GetItemById(Int32 id);
        List<CIDADE_COMARCA> ExecuteFilter(String nome, Int32 uf, Int32 idAss);
    }
}
