using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationServices.Interfaces;
using EntitiesServices.Model;
using System.Globalization;
using ERP_Juris.App_Start;
using EntitiesServices.Work_Classes;
using AutoMapper;
using ERP_Juris.ViewModels;
using System.IO;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ERP_Juris.Controllers
{
    public class TipoJusticaController : Controller
    {
        private readonly ITipoJusticaAppService baseApp;

        private String msg;
        private Exception exception;
        TIPO_JUSTICA objeto = new TIPO_JUSTICA();
        TIPO_JUSTICA objetoAntes = new TIPO_JUSTICA();
        List<TIPO_JUSTICA> listaMaster = new List<TIPO_JUSTICA>();
        String extensao;

        public TipoJusticaController(ITipoJusticaAppService baseApps)
        {
            baseApp = baseApps;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Voltar()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        public ActionResult VoltarGeral()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        public ActionResult DashboardAdministracao()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarAdmin", "BaseAdmin");
        }

        [HttpGet]
        public ActionResult MontarTelaTipoJustica()
        {
            // Verifica se tem usuario logado
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

                // Verfifica permissão
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];


            // Carrega listas
            if ((List<TIPO_JUSTICA>)Session["ListaTipoJustica"] == null)
            {
                listaMaster = baseApp.GetAllItens(idAss);
                Session["ListaTipoJustica"] = listaMaster;
            }

            ViewBag.Listas = listaMaster;
            ViewBag.Title = "Tipos de Justiça";
            //ViewBag.Cats = new SelectList(fornApp.GetAllCategorias(idAss), "CAFO_CD_ID", "CAFO_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Indicadores
            ViewBag.TiposJustica = listaMaster.Count;
            
            // Mensagem
            if ((Int32)Session["MensTipoJustica"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTipoJustica"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0010", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTipoJustica"] == 3)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0032", CultureInfo.CurrentCulture));
            }

            // Abre view
            Session["MensTipoJustica"] = 0;
            objeto = new TIPO_JUSTICA();
            Session["Volta"] = 1;
            return View(objeto);
        }

        public ActionResult RetirarFiltroTipoJustica()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaTipoJustica"] = null;
            Session["FiltroTipoJustica"] = null;
            return RedirectToAction("MontarTelaTipoJustica");
        }

        public ActionResult MostrarTudoTipoJustica()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMaster = baseApp.GetAllItensAdm(idAss);
            Session["FiltroTipoJustica"] = null;
            Session["ListaTipoJustica"] = listaMaster;
            return RedirectToAction("MontarTelaTipoJustica");
        }

        [HttpPost]
        public ActionResult FiltrarTipoJustica(TIPO_JUSTICA item)
        {
            // Verificar login
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Processo
            try
            {
                // Executa a operação
                Int32 idAss = (Int32)Session["IdAssinante"];
                List<TIPO_JUSTICA> listaObj = new List<TIPO_JUSTICA>();
                Session["FiltroTipoJustica"] = item;
                Int32 volta = baseApp.ExecuteFilter(item.TIJU_NM_NOME, item.TIJU_DS_DESCRICAO, idAss, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensTipoJustica"] = 1;
                    ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
                }

                // Sucesso
                Session["MensTipoJustica"] = 0;
                listaMaster = listaObj;
                Session["ListaTipoJustica"] = listaObj;
                return RedirectToAction("MontarTelaTipoJustica");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaTipoJustica");
            }
        }

        public ActionResult VoltarBaseTipoJustica()
        {
            // Verificar login
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaTipoJustica");
        }

        [HttpGet]
        public ActionResult IncluirTipoJustica()
        {
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM")
                {
                    Session["MensTipoJustica"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            //ViewBag.Cats = new SelectList(fornApp.GetAllCategorias(idAss), "CAFO_CD_ID", "CAFO_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Prepara view
            TIPO_JUSTICA item = new TIPO_JUSTICA();
            TipoJusticaViewModel vm = Mapper.Map<TIPO_JUSTICA, TipoJusticaViewModel>(item);
            vm.TIJU_IN_ATIVO = 1;
            vm.ASSI_CD_ID = idAss;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirTipoJustica(TipoJusticaViewModel vm)
        {
            var result = new Hashtable();
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    TIPO_JUSTICA item = Mapper.Map<TipoJusticaViewModel, TIPO_JUSTICA>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = baseApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0033", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<TIPO_JUSTICA>();
                    Session["ListaTipoJustica"] = null;
                    Session["TiposJustica"] = baseApp.GetAllItens(idAss);
                    return RedirectToAction("MontarTelaTipoJustica");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    result.Add("error", ex.Message);
                    return Json(result);
                }
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult EditarTipoJustica(Int32 id)
        {
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM")
                {
                    Session["MensTipoJustica"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }


            // Prepara view
            Int32 idAss = (Int32)Session["IdAssinante"];
            //ViewBag.Cats = new SelectList(fornApp.GetAllCategorias(idAss), "CAFO_CD_ID", "CAFO_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensTipoJustica"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }

            TIPO_JUSTICA item = baseApp.GetItemById(id);
            objetoAntes = item;
            Session["TipoJustica"] = item;
            Session["IdVolta"] = id;
            TipoJusticaViewModel vm = Mapper.Map<TIPO_JUSTICA, TipoJusticaViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarTipoJustica(TipoJusticaViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    TIPO_JUSTICA item = Mapper.Map<TipoJusticaViewModel, TIPO_JUSTICA>(vm);
                    Int32 volta = baseApp.ValidateEdit(item, objetoAntes, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0033", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<TIPO_JUSTICA>();
                    Session["ListaTipoJustica"] = null;
                    Session["MensTipoJustica"] = 0;
                    return RedirectToAction("MontarTelaTipoJustica");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View(vm);
                }
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult ExcluirTipoJustica(Int32 id)
        {
            // Verifica se tem usuario logado
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            USUARIO usuario = new USUARIO();
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];
                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM")
                {
                    Session["MensTipoJustica"] = 2;
                    return RedirectToAction("MontarTelaTipoJustica", "TipoJustica");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            TIPO_JUSTICA item = baseApp.GetItemById(id);
            objetoAntes = (TIPO_JUSTICA)Session["TipoJustica"];
            item.TIJU_IN_ATIVO = 0;
            Int32 volta = baseApp.ValidateDelete(item, usuario);
            // Verifica retorno
            if (volta == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0032", CultureInfo.CurrentCulture));
                Session["MensTipoJustica"] = 3;
                return RedirectToAction("MontarTelaTipoJustica", "TipoJustica");
            }
            listaMaster = new List<TIPO_JUSTICA>();
            Session["ListaTipoJustica"] = null;
            return RedirectToAction("MontarTelaTipoJustica");
        }


        [HttpGet]
        public ActionResult ReativarTipoJustica(Int32 id)
        {
            // Verifica se tem usuario logado
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            USUARIO usuario = new USUARIO();
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];
                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM")
                {
                    Session["MensTipoJustica"] = 2;
                    return RedirectToAction("MontarTelaTipoJustica", "TipoJustica");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            TIPO_JUSTICA item = baseApp.GetItemById(id);
            objetoAntes = (TIPO_JUSTICA)Session["TipoJustica"];
            item.TIJU_IN_ATIVO = 1;
            Int32 volta = baseApp.ValidateReativar(item, usuario);
            listaMaster = new List<TIPO_JUSTICA>();
            Session["ListaTipoJustica"] = null;
            return RedirectToAction("MontarTelaTipoJustica");
        }
    }
}