using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface ITipoJusticaRepository : IRepositoryBase<TIPO_JUSTICA>
    {
        TIPO_JUSTICA CheckExist(TIPO_JUSTICA item, Int32? idAss);
        List<TIPO_JUSTICA> GetAllItens(Int32 idAss);
        List<TIPO_JUSTICA> GetAllItensAdm(Int32 idAss);
        TIPO_JUSTICA GetItemById(Int32 id);
        List<TIPO_JUSTICA> ExecuteFilter(String nome, String descricao, Int32 idAss);
    }
}
