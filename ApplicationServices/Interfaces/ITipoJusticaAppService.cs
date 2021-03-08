using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface ITipoJusticaAppService : IAppServiceBase<TIPO_JUSTICA>
    {
        Int32 ValidateCreate(TIPO_JUSTICA item, USUARIO usuario);
        Int32 ValidateEdit(TIPO_JUSTICA item, TIPO_JUSTICA itemAntes, USUARIO usuario);
        Int32 ValidateEdit(TIPO_JUSTICA item, TIPO_JUSTICA itemAntes);
        Int32 ValidateDelete(TIPO_JUSTICA item, USUARIO usuario);
        Int32 ValidateReativar(TIPO_JUSTICA item, USUARIO usuario);

        TIPO_JUSTICA CheckExist(TIPO_JUSTICA tarefa, Int32 idUsu);
        List<TIPO_JUSTICA> GetAllItens(Int32? idAss);
        List<TIPO_JUSTICA> GetAllItensAdm(Int32? idAss);
        TIPO_JUSTICA GetItemById(Int32 id);
        Int32 ExecuteFilter(String nome, String descricao, Int32 idAss, out List<TIPO_JUSTICA> objeto);
    }
}
