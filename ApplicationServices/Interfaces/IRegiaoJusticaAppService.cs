using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface IRegiaoJusticaAppService : IAppServiceBase<REGIAO_JUSTICA>
    {
        Int32 ValidateCreate(REGIAO_JUSTICA item, USUARIO usuario);
        Int32 ValidateEdit(REGIAO_JUSTICA item, REGIAO_JUSTICA itemAntes, USUARIO usuario);
        Int32 ValidateEdit(REGIAO_JUSTICA item, REGIAO_JUSTICA itemAntes);
        Int32 ValidateDelete(REGIAO_JUSTICA item, USUARIO usuario);
        Int32 ValidateReativar(REGIAO_JUSTICA item, USUARIO usuario);

        REGIAO_JUSTICA CheckExist(REGIAO_JUSTICA tarefa, Int32 idUsu);
        List<REGIAO_JUSTICA> GetAllItens(Int32? idAss);
        List<REGIAO_JUSTICA> GetAllItensAdm(Int32? idAss);
        REGIAO_JUSTICA GetItemById(Int32 id);
        Int32 ExecuteFilter(Int32 idTipo, String nome, String descricao, Int32 idAss, out List<REGIAO_JUSTICA> objeto);
    }
}
