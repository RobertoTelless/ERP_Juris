using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface ICidadeComarcaAppService : IAppServiceBase<CIDADE_COMARCA>
    {
        Int32 ValidateCreate(CIDADE_COMARCA item, USUARIO usuario);
        Int32 ValidateEdit(CIDADE_COMARCA item, CIDADE_COMARCA itemAntes, USUARIO usuario);
        Int32 ValidateEdit(CIDADE_COMARCA item, CIDADE_COMARCA itemAntes);
        Int32 ValidateDelete(CIDADE_COMARCA item, USUARIO usuario);
        Int32 ValidateReativar(CIDADE_COMARCA item, USUARIO usuario);

        CIDADE_COMARCA CheckExist(CIDADE_COMARCA item, Int32? idAss);
        List<CIDADE_COMARCA> GetAllItens(Int32 idAss);
        List<CIDADE_COMARCA> GetAllItensAdm(Int32 idAss);
        List<UF> GetAllUF();
        CIDADE_COMARCA GetItemById(Int32 id);
        Int32 ExecuteFilter(String nome, Int32 uf, Int32 idAss, out List<CIDADE_COMARCA> objeto);
    }
}
