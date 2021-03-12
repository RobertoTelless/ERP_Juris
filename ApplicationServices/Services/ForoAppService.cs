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
    public class ForoAppService : AppServiceBase<FORO>, IForoAppService
    {
        private readonly IForoService _baseService;

        public ForoAppService(IForoService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<FORO> GetAllItens(Int32 idAss)
        {
            List<FORO> lista = _baseService.GetAllItens(idAss);
            return lista;
        }

        public List<FORO> GetAllItensAdm(Int32 idAss)
        {
            List<FORO> lista = _baseService.GetAllItensAdm(idAss);
            return lista;
        }

        public List<FORO> GetByCidade(Int32 idCidade)
        {
            List<FORO> lista = _baseService.GetByCidade(idCidade);
            return lista;
        }

        public List<UF> GetAllUF()
        {
            return _baseService.GetAllUF();
        }

        public FORO GetItemById(Int32 id)
        {
            FORO item = _baseService.GetItemById(id);
            return item;
        }

        public UF GetUFBySigla(String sigla)
        {
            UF item = _baseService.GetUFBySigla(sigla);
            return item;
        }

        public FORO CheckExist(FORO ag, Int32? idAss)
        {
            FORO item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public Int32 ExecuteFilter(Int32 idCidade, String nome, String descricao, String Bairro, Int32 uf, Int32 idAss, out List<FORO> objeto)
        {
            try
            {
                objeto = new List<FORO>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(idCidade, nome, descricao, Bairro, uf, idAss);
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

        public Int32 ValidateCreate(FORO item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.FORO_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddFORO",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<FORO>(item)
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

        public Int32 ValidateEdit(FORO item, FORO itemAntes, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (item.FORO_NM_NOME != itemAntes.FORO_NM_NOME & item.CICO_CD_ID == itemAntes.CICO_CD_ID)
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
                    LOG_NM_OPERACAO = "EditFORO",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<FORO>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<FORO>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(FORO item, FORO itemAntes)
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

        public Int32 ValidateDelete(FORO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.PROCESSO.Count > 0 || item.VARA.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.FORO_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleFORO",
                    LOG_TX_REGISTRO = "Foro: " + item.FORO_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(FORO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.FORO_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatFORO",
                    LOG_TX_REGISTRO = "Foro: " + item.FORO_NM_NOME
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
