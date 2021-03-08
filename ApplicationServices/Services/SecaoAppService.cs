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
    public class SecaoAppService : AppServiceBase<SECAO>, ISecaoAppService
    {
        private readonly ISecaoService _baseService;

        public SecaoAppService(ISecaoService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<SECAO> GetAllItens(Int32? idAss)
        {
            List<SECAO> lista = _baseService.GetAllItens(idAss.Value);
            return lista;
        }

        public List<SECAO> GetAllItensAdm(Int32? idAss)
        {
            List<SECAO> lista = _baseService.GetAllItensAdm(idAss.Value);
            return lista;
        }

        public List<SECAO> GetByRegiao(Int32? idRegiao)
        {
            List<SECAO> lista = _baseService.GetByRegiao(idRegiao.Value);
            return lista;
        }

        public SECAO GetItemById(Int32 id)
        {
            SECAO item = _baseService.GetItemById(id);
            return item;
        }

        public SECAO CheckExist(SECAO ag, Int32 idAss)
        {
            SECAO item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public Int32 ExecuteFilter(Int32 idRegiao, String nome, String descricao, Int32 idAss, out List<SECAO> objeto)
        {
            try
            {
                objeto = new List<SECAO>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(idRegiao, nome, descricao, idAss);
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

        public Int32 ValidateCreate(SECAO item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.SECA_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddSECA",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<SECAO>(item)
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

        public Int32 ValidateEdit(SECAO item, SECAO itemAntes, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (item.SECA_NM_NOME != itemAntes.SECA_NM_NOME)
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
                    LOG_NM_OPERACAO = "EditSECA",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<SECAO>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<SECAO>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(SECAO item, SECAO itemAntes)
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

        public Int32 ValidateDelete(SECAO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.PROCESSO.Count > 0 || item.SUBSECAO.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.SECA_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleSECA",
                    LOG_TX_REGISTRO = "Seção: " + item.SECA_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(SECAO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.SECA_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatSECA",
                    LOG_TX_REGISTRO = "Seção: " + item.SECA_NM_NOME
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
