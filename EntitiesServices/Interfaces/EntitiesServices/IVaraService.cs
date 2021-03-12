using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IVaraService : IServiceBase<VARA>
    {
        Int32 Create(VARA item, LOG log);
        Int32 Create(VARA item);
        Int32 Edit(VARA item, LOG log);
        Int32 Edit(VARA item);
        Int32 Delete(VARA item, LOG log);

        VARA CheckExist(VARA item, Int32? idAss);
        List<VARA> GetAllItens(Int32 idAss);
        List<VARA> GetAllItensAdm(Int32 idAss);
        List<UF> GetAllUF();
        List<VARA> GetByForo(Int32 idForo);
        VARA GetItemById(Int32 id);
        List<VARA> ExecuteFilter(Int32 idForo, String nome, String descricao, String juiz, String cidade, String Bairro, Int32 uf, Int32 idAss);
    }
}
