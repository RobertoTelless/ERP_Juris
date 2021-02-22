using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface ICategoriaUsuarioService : IServiceBase<CATEGORIA_USUARIO>
    {
        Int32 Create(CATEGORIA_USUARIO item, LOG log);
        Int32 Create(CATEGORIA_USUARIO item);
        Int32 Edit(CATEGORIA_USUARIO item, LOG log);
        Int32 Edit(CATEGORIA_USUARIO item);
        Int32 Delete(CATEGORIA_USUARIO item, LOG log);

        CATEGORIA_USUARIO CheckExist(CATEGORIA_USUARIO item, Int32? idAss);
        List<CATEGORIA_USUARIO> GetAllItens(Int32? idAss);
        CATEGORIA_USUARIO GetItemById(Int32 id);
        List<CATEGORIA_USUARIO> GetAllItensAdm(Int32? idAss);
    }
}
