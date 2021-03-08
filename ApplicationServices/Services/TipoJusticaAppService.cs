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
    public class TipoJusticaAppService : AppServiceBase<TIPO_JUSTICA>, ITipoJusticaAppService
    {
        private readonly ITipoJusticaService _baseService;

        public TipoJusticaAppService(ITipoJusticaService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<TIPO_JUSTICA> GetAllItens(Int32? idAss)
        {
            List<TIPO_JUSTICA> lista = _baseService.GetAllItens(idAss.Value);
            return lista;
        }

        public List<TIPO_JUSTICA> GetAllItensAdm(Int32? idAss)
        {
            List<TIPO_JUSTICA> lista = _baseService.GetAllItensAdm(idAss.Value);
            return lista;
        }

        public TIPO_JUSTICA GetItemById(Int32 id)
        {
            TIPO_JUSTICA item = _baseService.GetItemById(id);
            return item;
        }

        public TIPO_JUSTICA CheckExist(TIPO_JUSTICA ag, Int32 idAss)
        {
            TIPO_JUSTICA item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public Int32 ExecuteFilter(String nome, String descricao, Int32 idAss, out List<TIPO_JUSTICA> objeto)
        {
            try
            {
                objeto = new List<TIPO_JUSTICA>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(nome, descricao, idAss);
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

        public Int32 ValidateCreate(TIPO_JUSTICA item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.TIJU_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddTIJU",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<TIPO_JUSTICA>(item)
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

        public Int32 ValidateEdit(TIPO_JUSTICA item, TIPO_JUSTICA itemAntes, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (item.TIJU_NM_NOME != itemAntes.TIJU_NM_NOME)
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
                    LOG_NM_OPERACAO = "EditTIJU",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<TIPO_JUSTICA>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<TIPO_JUSTICA>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(TIPO_JUSTICA item, TIPO_JUSTICA itemAntes)
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

        public Int32 ValidateDelete(TIPO_JUSTICA item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.PROCESSO.Count > 0 || item.REGIAO_JUSTICA.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.TIJU_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleTIJU",
                    LOG_TX_REGISTRO = "Tipo de Justiça: " + item.TIJU_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(TIPO_JUSTICA item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.TIJU_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatTIJU",
                    LOG_TX_REGISTRO = "Tipo de Justiça: " + item.TIJU_NM_NOME
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
