using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IEmpresaService : IServiceBase<EMPRESA>
    {
        EMPRESA GetItemById(Int32 id);
        List<EMPRESA> GetAllItems();
        Int32 Edit(EMPRESA item, LOG log);
        Int32 Create(EMPRESA item);
    }
}
