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
    public class TipoAcaoAppService : AppServiceBase<TIPO_ACAO>, ITipoAcaoAppService
    {
        private readonly ITipoAcaoService _baseService;

        public TipoAcaoAppService(ITipoAcaoService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<TIPO_ACAO> GetAllItens(Int32? idAss)
        {
            List<TIPO_ACAO> lista = _baseService.GetAllItens(idAss.Value);
            return lista;
        }

        public List<TIPO_ACAO> GetAllItensAdm(Int32? idAss)
        {
            List<TIPO_ACAO> lista = _baseService.GetAllItensAdm(idAss.Value);
            return lista;
        }

        public TIPO_ACAO GetItemById(Int32 id)
        {
            TIPO_ACAO item = _baseService.GetItemById(id);
            return item;
        }

        public TIPO_ACAO CheckExist(TIPO_ACAO ag, Int32 idAss)
        {
            TIPO_ACAO item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public Int32 ExecuteFilter(String nome, String descricao, Int32 idAss, out List<TIPO_ACAO> objeto)
        {
            try
            {
                objeto = new List<TIPO_ACAO>();
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

        public Int32 ValidateCreate(TIPO_ACAO item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.TIAC_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddTIAC",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<TIPO_ACAO>(item)
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

        public Int32 ValidateEdit(TIPO_ACAO item, TIPO_ACAO itemAntes, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (item.TIAC_NM_NOME != itemAntes.TIAC_NM_NOME)
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
                    LOG_NM_OPERACAO = "EditTIAC",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<TIPO_ACAO>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<TIPO_ACAO>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(TIPO_ACAO item, TIPO_ACAO itemAntes)
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

        public Int32 ValidateDelete(TIPO_ACAO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.PROCESSO.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.TIAC_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleTIAC",
                    LOG_TX_REGISTRO = "Tipo de Ação: " + item.TIAC_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(TIPO_ACAO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.TIAC_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatTIAC",
                    LOG_TX_REGISTRO = "Tipo de Ação: " + item.TIAC_NM_NOME
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
