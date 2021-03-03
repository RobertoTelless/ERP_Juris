using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface ITipoJusticaService : IServiceBase<TIPO_JUSTICA>
    {
        Int32 Create(TIPO_JUSTICA item, LOG log);
        Int32 Create(TIPO_JUSTICA item);
        Int32 Edit(TIPO_JUSTICA item, LOG log);
        Int32 Edit(TIPO_JUSTICA item);
        Int32 Delete(TIPO_JUSTICA item, LOG log);

        TIPO_JUSTICA CheckExist(TIPO_JUSTICA item, Int32? idAss);
        List<TIPO_JUSTICA> GetAllItens(Int32 idAss);
        List<TIPO_JUSTICA> GetAllItensAdm(Int32 idAss);
        TIPO_JUSTICA GetItemById(Int32 id);
        List<TIPO_JUSTICA> ExecuteFilter(String nome, String descricao, Int32 idAss);
    }
}
