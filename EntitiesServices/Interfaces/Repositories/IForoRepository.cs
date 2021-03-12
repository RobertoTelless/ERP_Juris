using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface IForoRepository : IRepositoryBase<FORO>
    {
        FORO CheckExist(FORO item, Int32? idAss);
        List<FORO> GetAllItens(Int32 idAss);
        List<FORO> GetAllItensAdm(Int32 idAss);
        List<FORO> GetByCidade(Int32 idCidade);
        FORO GetItemById(Int32 id);
        List<FORO> ExecuteFilter(Int32 idCidade, String nome, String descricao, String Bairro, Int32 uf, Int32 idAss);
    }
}
