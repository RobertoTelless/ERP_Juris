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
    public class CidadeComarcaAppService : AppServiceBase<CIDADE_COMARCA>, ICidadeComarcaAppService
    {
        private readonly ICidadeComarcaService _baseService;

        public CidadeComarcaAppService(ICidadeComarcaService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<CIDADE_COMARCA> GetAllItens(Int32 idAss)
        {
            List<CIDADE_COMARCA> lista = _baseService.GetAllItens(idAss);
            return lista;
        }

        public List<CIDADE_COMARCA> GetAllItensAdm(Int32 idAss)
        {
            List<CIDADE_COMARCA> lista = _baseService.GetAllItensAdm(idAss);
            return lista;
        }

        public CIDADE_COMARCA GetItemById(Int32 id)
        {
            CIDADE_COMARCA item = _baseService.GetItemById(id);
            return item;
        }

        public CIDADE_COMARCA CheckExist(CIDADE_COMARCA ag, Int32? idAss)
        {
            CIDADE_COMARCA item = _baseService.CheckExist(ag, idAss.Value);
            return item;
        }

        public List<UF> GetAllUF()
        {
            return _baseService.GetAllUF();
        }

        public Int32 ExecuteFilter(String nome, Int32 uf, Int32 idAss, out List<CIDADE_COMARCA> objeto)
        {
            try
            {
                objeto = new List<CIDADE_COMARCA>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(nome, uf, idAss);
                if (objeto.Count == 0)
                {
                    volta = 1;
                }
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateCreate(CIDADE_COMARCA item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.CICO_IN_ATIVO = "1";

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddCICO",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<CIDADE_COMARCA>(item)
                };

                // Persiste
                Int32 volta = _baseService.Create(item, log);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(CIDADE_COMARCA item, CIDADE_COMARCA itemAntes, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (item.CICO_NM_NOME != itemAntes.CICO_NM_NOME)
                {
                    if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                    {
                        return 1;
                    }
                }

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "EditCICO",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<CIDADE_COMARCA>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<CIDADE_COMARCA>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(CIDADE_COMARCA item, CIDADE_COMARCA itemAntes)
        {
            try
            {
                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateDelete(CIDADE_COMARCA item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.FORO.Count > 0 || item.PROCESSO.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.CICO_IN_ATIVO = "0";

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleCICO",
                    LOG_TX_REGISTRO = "Cidade/Comarca: " + item.CICO_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(CIDADE_COMARCA item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.CICO_IN_ATIVO = "1";

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatCICO",
                    LOG_TX_REGISTRO = "Cidade/Comarca: " + item.CICO_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
