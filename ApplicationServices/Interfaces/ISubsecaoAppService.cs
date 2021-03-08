using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface ISubsecaoAppService : IAppServiceBase<SUBSECAO>
    {
        Int32 ValidateCreate(SUBSECAO item, USUARIO usuario);
        Int32 ValidateEdit(SUBSECAO item, SUBSECAO itemAntes, USUARIO usuario);
        Int32 ValidateEdit(SUBSECAO item, SUBSECAO itemAntes);
        Int32 ValidateDelete(SUBSECAO item, USUARIO usuario);
        Int32 ValidateReativar(SUBSECAO item, USUARIO usuario);

        SUBSECAO CheckExist(SUBSECAO tarefa, Int32 idUsu);
        List<SUBSECAO> GetAllItens(Int32 idAss);
        List<SUBSECAO> GetAllItensAdm(Int32 idAss);
        List<SUBSECAO> GetBySecao(Int32 idSecao);
        SUBSECAO GetItemById(Int32 id);
        Int32 ExecuteFilter(Int32 idSecao, String nome, String descricao, Int32 idAss, out List<SUBSECAO> objeto);
    }
}
