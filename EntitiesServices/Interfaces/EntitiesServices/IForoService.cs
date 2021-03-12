using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IForoService : IServiceBase<FORO>
    {
        Int32 Create(FORO item, LOG log);
        Int32 Create(FORO item);
        Int32 Edit(FORO item, LOG log);
        Int32 Edit(FORO item);
        Int32 Delete(FORO item, LOG log);

        FORO CheckExist(FORO item, Int32? idAss);
        List<FORO> GetAllItens(Int32 idAss);
        List<FORO> GetAllItensAdm(Int32 idAss);
        List<UF> GetAllUF();
        List<FORO> GetByCidade(Int32 idCidade);
        FORO GetItemById(Int32 id);
        List<FORO> ExecuteFilter(Int32 idCidade, String nome, String descricao, String Bairro, Int32 uf, Int32 idAss);
    }
}
