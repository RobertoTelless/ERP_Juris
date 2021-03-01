using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface IEmpresaAppService : IAppServiceBase<EMPRESA>
    {
        Int32 ValidateEdit(EMPRESA item, EMPRESA itemAntes, USUARIO usuario);
        EMPRESA GetItemById(Int32 id);
        List<EMPRESA> GetAllItems();
        Int32 ValidateCreate(EMPRESA item);
        UF GetItemBySigla(String sigla);
        List<UF> GetAllUF();
    }
}
