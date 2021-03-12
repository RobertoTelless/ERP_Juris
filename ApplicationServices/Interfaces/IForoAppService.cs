using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface IForoAppService : IAppServiceBase<FORO>
    {
        Int32 ValidateCreate(FORO item, USUARIO usuario);
        Int32 ValidateEdit(FORO item, FORO itemAntes, USUARIO usuario);
        Int32 ValidateEdit(FORO item, FORO itemAntes);
        Int32 ValidateDelete(FORO item, USUARIO usuario);
        Int32 ValidateReativar(FORO item, USUARIO usuario);

        FORO CheckExist(FORO item, Int32? idAss);
        List<FORO> GetAllItens(Int32 idAss);
        List<FORO> GetAllItensAdm(Int32 idAss);
        List<UF> GetAllUF();
        List<FORO> GetByCidade(Int32 idCidade);
        FORO GetItemById(Int32 id);
        Int32 ExecuteFilter(Int32 idCidade, String nome, String descricao, String Bairro, Int32 uf, Int32 idAss, out List<FORO> objeto);
    }
}
