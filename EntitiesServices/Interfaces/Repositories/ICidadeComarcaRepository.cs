using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface ICidadeComarcaRepository : IRepositoryBase<CIDADE_COMARCA>
    {
        CIDADE_COMARCA CheckExist(CIDADE_COMARCA item, Int32? idAss);
        List<CIDADE_COMARCA> GetAllItens(Int32 idAss);
        List<CIDADE_COMARCA> GetAllItensAdm(Int32 idAss);
        CIDADE_COMARCA GetItemById(Int32 id);
        List<CIDADE_COMARCA> ExecuteFilter(String nome, Int32 uf, Int32 tipo, Int32 idAss);
    }
}
