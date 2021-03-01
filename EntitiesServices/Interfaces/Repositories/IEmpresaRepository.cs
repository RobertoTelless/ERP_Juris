using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface IEmpresaRepository : IRepositoryBase<EMPRESA>
    {
        EMPRESA GetItemById(Int32 id);
        List<EMPRESA> GetAllItems();
    }
}
