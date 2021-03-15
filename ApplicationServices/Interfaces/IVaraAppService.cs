using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface IVaraAppService : IAppServiceBase<VARA>
    {
        Int32 ValidateCreate(VARA item, USUARIO usuario);
        Int32 ValidateEdit(VARA item, VARA itemAntes, USUARIO usuario);
        Int32 ValidateEdit(VARA item, VARA itemAntes);
        Int32 ValidateDelete(VARA item, USUARIO usuario);
        Int32 ValidateReativar(VARA item, USUARIO usuario);

        VARA CheckExist(VARA item, Int32? idAss);
        List<VARA> GetAllItens(Int32 idAss);
        List<VARA> GetAllItensAdm(Int32 idAss);
        List<UF> GetAllUF();
        List<VARA> GetByForo(Int32 idForo);
        VARA GetItemById(Int32 id);
        UF GetUFBySigla(String sigla);
        Int32 ExecuteFilter(Int32 idForo, String nome, String descricao, String juiz, String cidade, String Bairro, Int32 uf, Int32 idAss, out List<VARA> objeto);
    }
}
