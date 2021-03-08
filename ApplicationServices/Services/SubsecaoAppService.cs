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
    public class SubsecaoAppService : AppServiceBase<SUBSECAO>, ISubsecaoAppService
    {
        private readonly ISubsecaoService _baseService;

        public SubsecaoAppService(ISubsecaoService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<SUBSECAO> GetAllItens(Int32 idAss)
        {
            List<SUBSECAO> lista = _baseService.GetAllItens(idAss);
            return lista;
        }

        public List<SUBSECAO> GetAllItensAdm(Int32 idAss)
        {
            List<SUBSECAO> lista = _baseService.GetAllItensAdm(idAss);
            return lista;
        }

        public List<SUBSECAO> GetBySecao(Int32 idSecao)
        {
            List<SUBSECAO> lista = _baseService.GetBySecao(idSecao);
            return lista;
        }

        public SUBSECAO GetItemById(Int32 id)
        {
            SUBSECAO item = _baseService.GetItemById(id);
            return item;
        }

        public SUBSECAO CheckExist(SUBSECAO ag, Int32 idAss)
        {
            SUBSECAO item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public Int32 ExecuteFilter(Int32 idSecao, String nome, String descricao, Int32 idAss, out List<SUBSECAO> objeto)
        {
            try
            {
                objeto = new List<SUBSECAO>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(idSecao, nome, descricao, idAss);
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

        public Int32 ValidateCreate(SUBSECAO item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.SUSE_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddSUSE",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<SUBSECAO>(item)
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

        public Int32 ValidateEdit(SUBSECAO item, SUBSECAO itemAntes, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (item.SUSE_NM_NOME != itemAntes.SUSE_NM_NOME)
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
                    LOG_NM_OPERACAO = "EditSUSE",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<SUBSECAO>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<SUBSECAO>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(SUBSECAO item, SUBSECAO itemAntes)
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

        public Int32 ValidateDelete(SUBSECAO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.PROCESSO.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.SUSE_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleSUSE",
                    LOG_TX_REGISTRO = "Subseção: " + item.SUSE_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(SUBSECAO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.SUSE_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatSUSE",
                    LOG_TX_REGISTRO = "Subseção: " + item.SUSE_NM_NOME
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
