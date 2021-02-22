using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IConfiguracaoService : IServiceBase<CONFIGURACAO>
    {
        CONFIGURACAO GetItemById(Int32 id);
        List<CONFIGURACAO> GetAllItems();
        Int32 Edit(CONFIGURACAO item, LOG log);
        Int32 Create(CONFIGURACAO item);
    }
}
