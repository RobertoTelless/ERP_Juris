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
    public class VaraAppService : AppServiceBase<VARA>, IVaraAppService
    {
        private readonly IVaraService _baseService;

        public VaraAppService(IVaraService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<VARA> GetAllItens(Int32 idAss)
        {
            List<VARA> lista = _baseService.GetAllItens(idAss);
            return lista;
        }

        public List<VARA> GetAllItensAdm(Int32 idAss)
        {
            List<VARA> lista = _baseService.GetAllItensAdm(idAss);
            return lista;
        }

        public List<VARA> GetByForo(Int32 idForo)
        {
            List<VARA> lista = _baseService.GetByForo(idForo);
            return lista;
        }

        public List<UF> GetAllUF()
        {
            return _baseService.GetAllUF();
        }

        public UF GetUFBySigla(String sigla)
        {
            UF item = _baseService.GetUFBySigla(sigla);
            return item;
        }

        public VARA GetItemById(Int32 id)
        {
            VARA item = _baseService.GetItemById(id);
            return item;
        }

        public VARA CheckExist(VARA ag, Int32? idAss)
        {
            VARA item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public Int32 ExecuteFilter(Int32 idForo, String nome, String descricao, String juiz, String cidade, String Bairro, Int32 uf, Int32 idAss, out List<VARA> objeto)
        {
            try
            {
                objeto = new List<VARA>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(idForo, nome, descricao, juiz, cidade, Bairro, uf, idAss);
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

        public Int32 ValidateCreate(VARA item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.VARA_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddVARA",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<VARA>(item)
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

        public Int32 ValidateEdit(VARA item, VARA itemAntes, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (item.VARA_NM_NOME != itemAntes.VARA_NM_NOME & item.FORO_CD_ID == itemAntes.FORO_CD_ID)
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
                    LOG_NM_OPERACAO = "EditVARA",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<VARA>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<VARA>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(VARA item, VARA itemAntes)
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

        public Int32 ValidateDelete(VARA item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.PROCESSO.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.VARA_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleVARA",
                    LOG_TX_REGISTRO = "Vara: " + item.VARA_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(VARA item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.VARA_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatVARA",
                    LOG_TX_REGISTRO = "Vara: " + item.VARA_NM_NOME
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
