using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface ITipoTarefaService : IServiceBase<TIPO_TAREFA>
    {
        Int32 Create(TIPO_TAREFA item, LOG log);
        Int32 Create(TIPO_TAREFA item);
        Int32 Edit(TIPO_TAREFA item, LOG log);
        Int32 Edit(TIPO_TAREFA item);
        Int32 Delete(TIPO_TAREFA item, LOG log);

        TIPO_TAREFA CheckExist(TIPO_TAREFA item, Int32? idAss);
        List<TIPO_TAREFA> GetAllItens(Int32? idAss);
        TIPO_TAREFA GetItemById(Int32 id);
        List<TIPO_TAREFA> GetAllItensAdm(Int32? idAss);
    }
}
