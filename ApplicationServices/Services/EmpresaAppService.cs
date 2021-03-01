using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;
using ApplicationServices.Interfaces;
using ModelServices.Interfaces.EntitiesServices;
using CrossCutting;
using System.Text.RegularExpressions;

namespace ApplicationServices.Services
{
    public class EmpresaAppService : AppServiceBase<EMPRESA>, IEmpresaAppService
    {
        private readonly IEmpresaService _baseService;

        public EmpresaAppService(IEmpresaService baseService) : base(baseService)
        {
            _baseService = baseService;
        }

        public EMPRESA GetItemById(Int32 id)
        {
            return _baseService.GetItemById(id);
        }

        public UF GetItemBySigla(String sigla)
        {
            return _baseService.GetItemBySigla(sigla);
        }

        public List<EMPRESA> GetAllItems()
        {
            return _baseService.GetAllItems();
        }

        public List<UF> GetAllUF()
        {
            return _baseService.GetAllUF();
        }

        public Int32 ValidateEdit(EMPRESA item, EMPRESA itemAntes, USUARIO usuario)
        {
            try
            {
                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "EditEMPR",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<EMPRESA>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<EMPRESA>(itemAntes),
                    LOG_IN_ATIVO = 1
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateCreate(EMPRESA item)
        {
            try
            {
                // Persiste
                Int32 volta = _baseService.Create(item);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
