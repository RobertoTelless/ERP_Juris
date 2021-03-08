using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface ISecaoAppService : IAppServiceBase<SECAO>
    {
        Int32 ValidateCreate(SECAO item, USUARIO usuario);
        Int32 ValidateEdit(SECAO item, SECAO itemAntes, USUARIO usuario);
        Int32 ValidateEdit(SECAO item, SECAO itemAntes);
        Int32 ValidateDelete(SECAO item, USUARIO usuario);
        Int32 ValidateReativar(SECAO item, USUARIO usuario);

        SECAO CheckExist(SECAO tarefa, Int32 idUsu);
        List<SECAO> GetAllItens(Int32 idAss);
        List<SECAO> GetAllItensAdm(Int32 idAss);
        List<SECAO> GetByRegiao(Int32 idRegiao);
        SECAO GetItemById(Int32 id);
        Int32 ExecuteFilter(Int32 idRegiao, String nome, String descricao, Int32 idAss, out List<SECAO> objeto);
    }
}
