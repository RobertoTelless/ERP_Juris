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
    public class RegiaoJusticaAppService : AppServiceBase<REGIAO_JUSTICA>, IRegiaoJusticaAppService
    {
        private readonly IRegiaoJusticaService _baseService;

        public RegiaoJusticaAppService(IRegiaoJusticaService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<REGIAO_JUSTICA> GetAllItens(Int32? idAss)
        {
            List<REGIAO_JUSTICA> lista = _baseService.GetAllItens(idAss.Value);
            return lista;
        }

        public List<REGIAO_JUSTICA> GetAllItensAdm(Int32? idAss)
        {
            List<REGIAO_JUSTICA> lista = _baseService.GetAllItensAdm(idAss.Value);
            return lista;
        }

        public List<REGIAO_JUSTICA> GetByTipo(Int32? idTipo)
        {
            List<REGIAO_JUSTICA> lista = _baseService.GetByTipo(idTipo.Value);
            return lista;
        }

        public REGIAO_JUSTICA GetItemById(Int32 id)
        {
            REGIAO_JUSTICA item = _baseService.GetItemById(id);
            return item;
        }

        public REGIAO_JUSTICA CheckExist(REGIAO_JUSTICA ag, Int32 idAss)
        {
            REGIAO_JUSTICA item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public Int32 ExecuteFilter(Int32 idTipo, String nome, String descricao, Int32 idAss, out List<REGIAO_JUSTICA> objeto)
        {
            try
            {
                objeto = new List<REGIAO_JUSTICA>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(idTipo, nome, descricao, idAss);
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

        public Int32 ValidateCreate(REGIAO_JUSTICA item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.REJU_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddREJU",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<REGIAO_JUSTICA>(item)
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

        public Int32 ValidateEdit(REGIAO_JUSTICA item, REGIAO_JUSTICA itemAntes, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (item.REJU_NM_NOME != itemAntes.REJU_NM_NOME)
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
                    LOG_NM_OPERACAO = "EditREJU",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<REGIAO_JUSTICA>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<REGIAO_JUSTICA>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(REGIAO_JUSTICA item, REGIAO_JUSTICA itemAntes)
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

        public Int32 ValidateDelete(REGIAO_JUSTICA item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.PROCESSO.Count > 0 || item.SECAO.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.REJU_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleREJU",
                    LOG_TX_REGISTRO = "Região de Justiça: " + item.REJU_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(REGIAO_JUSTICA item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.REJU_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatREJU",
                    LOG_TX_REGISTRO = "Região de Justiça: " + item.REJU_NM_NOME
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
