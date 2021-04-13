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
    public class SubsecaoController : Controller
    {
        private readonly ISubsecaoAppService baseApp;
        private readonly ISecaoAppService tjApp;

        private String msg;
        private Exception exception;
        SUBSECAO objeto = new SUBSECAO();
        SUBSECAO objetoAntes = new SUBSECAO();
        List<SUBSECAO> listaMaster = new List<SUBSECAO>();
        String extensao;

        public SubsecaoController(ISubsecaoAppService baseApps)
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
        public ActionResult MontarTelaSubsecao()
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
            if ((List<SECAO>)Session["ListaSubsecao"] == null)
            {
                listaMaster = baseApp.GetAllItens(idAss);
                Session["ListaSubsecao"] = listaMaster;
            }

            ViewBag.Listas = (List<SECAO>)Session["ListaSubsecao"];
            ViewBag.Title = "Subseções";
            ViewBag.Secoes = new SelectList(tjApp.GetAllItens(idAss), "SECA_CD_ID", "SECA_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Indicadores
            ViewBag.Subsecoes = ((List<SECAO>)Session["ListaSubsecao"]).Count;
            
            // Mensagem
            if ((Int32)Session["MensSubsecao"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensSubsecao"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0010", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensSubsecao"] == 3)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0038", CultureInfo.CurrentCulture));
            }

            // Abre view
            Session["MensSubsecao"] = 0;
            objeto = new SUBSECAO();
            Session["Volta"] = 1;
            return View(objeto);
        }

        public ActionResult RetirarFiltroSubsecao()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaSubsecao"] = null;
            Session["FiltroSubsecao"] = null;
            return RedirectToAction("MontarTelaSubsecao");
        }

        public ActionResult MostrarTudoSubsecao()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMaster = baseApp.GetAllItensAdm(idAss);
            Session["FiltroSubsecao"] = null;
            Session["ListaSubsecao"] = listaMaster;
            return RedirectToAction("MontarTelaSubsecao");
        }

        [HttpPost]
        public ActionResult FiltrarSubsecao(SUBSECAO item)
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
                List<SUBSECAO> listaObj = new List<SUBSECAO>();
                Session["FiltroSubsecao"] = item;
                Int32 volta = baseApp.ExecuteFilter(item.SECA_CD_ID, item.SUSE_NM_NOME, item.SUSE_DS_DESCRICAO, idAss, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensSubsecao"] = 1;
                    ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
                }

                // Sucesso
                Session["MensSubsecao"] = 0;
                listaMaster = listaObj;
                Session["ListaSubsecao"] = listaObj;
                return RedirectToAction("MontarTelaSubsecao");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaSubsecao");
            }
        }

        public ActionResult VoltarBaseSubsecao()
        {
            // Verificar login
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaSubsecao");
        }

        [HttpGet]
        public ActionResult IncluirSubsecao()
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
                    Session["MensSubsecao"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Secoes = new SelectList(tjApp.GetAllItens(idAss), "SECA_CD_ID", "SECA_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Prepara view
            SUBSECAO item = new SUBSECAO();
            SubsecaoViewModel vm = Mapper.Map<SUBSECAO, SubsecaoViewModel>(item);
            vm.SUSE_IN_ATIVO = 1;
            vm.ASSI_CD_ID = idAss;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirSubsecao(SubsecaoViewModel vm)
        {
            var result = new Hashtable();
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Secoes = new SelectList(tjApp.GetAllItens(idAss), "SECA_CD_ID", "SECA_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    SUBSECAO item = Mapper.Map<SubsecaoViewModel, SUBSECAO>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = baseApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0039", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<SUBSECAO>();
                    Session["ListaSubsecao"] = null;
                    Session["Subsecoes"] = baseApp.GetAllItens(idAss);
                    return RedirectToAction("MontarTelaSubsecao");
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
        public ActionResult EditarSubsecao(Int32 id)
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
                    Session["MensSubsecao"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }


            // Prepara view
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Secoes = new SelectList(tjApp.GetAllItens(idAss), "SECA_CD_ID", "SECA_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensSubsecao"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }

            SUBSECAO item = baseApp.GetItemById(id);
            objetoAntes = item;
            Session["Subsecao"] = item;
            Session["IdVolta"] = id;
            SubsecaoViewModel vm = Mapper.Map<SUBSECAO, SubsecaoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarSubsecao(SubsecaoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Secoes = new SelectList(tjApp.GetAllItens(idAss), "SECA_CD_ID", "SECA_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    SUBSECAO item = Mapper.Map<SubsecaoViewModel, SUBSECAO>(vm);
                    Int32 volta = baseApp.ValidateEdit(item, (SUBSECAO)Session["Subsecao"], usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0039", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<SUBSECAO>();
                    Session["ListaSubsecao"] = null;
                    Session["MensSubsecao"] = 0;
                    return RedirectToAction("MontarTelaSubsecao");
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
        public ActionResult ExcluirSubsecao(Int32 id)
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
                    Session["MensSubsecao"] = 2;
                    return RedirectToAction("MontarTelaSubsecao", "Subsecao");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            SUBSECAO item = baseApp.GetItemById(id);
            objetoAntes = (SUBSECAO)Session["Subsecao"];
            item.SUSE_IN_ATIVO = 0;
            Int32 volta = baseApp.ValidateDelete(item, usuario);
            // Verifica retorno
            if (volta == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0038", CultureInfo.CurrentCulture));
                Session["MensSubsecao"] = 3;
                return RedirectToAction("MontarTelaSubsecao", "Subsecao");
            }
            listaMaster = new List<SUBSECAO>();
            Session["ListaSubsecao"] = null;
            return RedirectToAction("MontarTelaSubsecao");
        }


        [HttpGet]
        public ActionResult ReativarSubsecao(Int32 id)
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
                    Session["MensSubsecao"] = 2;
                    return RedirectToAction("MontarTelaSubsecao", "Subsecao");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            SUBSECAO item = baseApp.GetItemById(id);
            objetoAntes = (SUBSECAO)Session["Subsecao"];
            item.SUSE_IN_ATIVO = 1;
            Int32 volta = baseApp.ValidateReativar(item, usuario);
            listaMaster = new List<SUBSECAO>();
            Session["ListaSubsecao"] = null;
            return RedirectToAction("MontarTelaSubsecao");        
        }
    }
}