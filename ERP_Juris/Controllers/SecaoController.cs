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
    public class SecaoController : Controller
    {
        private readonly ISecaoAppService baseApp;
        private readonly IRegiaoJusticaAppService tjApp;

        private String msg;
        private Exception exception;
        SECAO objeto = new SECAO();
        SECAO objetoAntes = new SECAO();
        List<SECAO> listaMaster = new List<SECAO>();
        String extensao;

        public SecaoController(ISecaoAppService baseApps)
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
        public ActionResult MontarTelaSecao()
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
            if ((List<SECAO>)Session["ListaSecao"] == null)
            {
                listaMaster = baseApp.GetAllItens(idAss);
                Session["ListaSecao"] = listaMaster;
            }

            ViewBag.Listas = (List<SECAO>)Session["ListaSecao"];
            ViewBag.Title = "Seções";
            ViewBag.Regioes = new SelectList(tjApp.GetAllItens(idAss), "REJU_CD_ID", "REJU_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Indicadores
            ViewBag.Secoes = ((List<SECAO>)Session["ListaSecao"]).Count;
            
            // Mensagem
            if ((Int32)Session["MensSecao"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensSecao"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0010", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensSecao"] == 3)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0036", CultureInfo.CurrentCulture));
            }

            // Abre view
            Session["MensSecao"] = 0;
            objeto = new SECAO();
            Session["Volta"] = 1;
            return View(objeto);
        }

        public ActionResult RetirarFiltroSecao()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaSecao"] = null;
            Session["FiltroSecao"] = null;
            return RedirectToAction("MontarTelaSecao");
        }

        public ActionResult MostrarTudoSecao()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMaster = baseApp.GetAllItensAdm(idAss);
            Session["FiltroSecao"] = null;
            Session["ListaSecao"] = listaMaster;
            return RedirectToAction("MontarTelaSecao");
        }

        [HttpPost]
        public ActionResult FiltrarSecao(SECAO item)
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
                List<SECAO> listaObj = new List<SECAO>();
                Session["FiltroSecao"] = item;
                Int32 volta = baseApp.ExecuteFilter(item.REJU_CD_ID, item.SECA_NM_NOME, item.SECA_DS_DESCRICAO, idAss, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensSecao"] = 1;
                    ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
                }

                // Sucesso
                Session["MensSecao"] = 0;
                listaMaster = listaObj;
                Session["ListaSecao"] = listaObj;
                return RedirectToAction("MontarTelaSecao");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaSecao");
            }
        }

        public ActionResult VoltarBaseSecao()
        {
            // Verificar login
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaSecao");
        }

        [HttpGet]
        public ActionResult IncluirSecao()
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
                    Session["MensSecao"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Regioes = new SelectList(tjApp.GetAllItens(idAss), "REJU_CD_ID", "REJU_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Prepara view
            SECAO item = new SECAO();
            SecaoViewModel vm = Mapper.Map<SECAO, SecaoViewModel>(item);
            vm.SECA_IN_ATIVO = 1;
            vm.ASSI_CD_ID = idAss;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirSecao(SecaoViewModel vm)
        {
            var result = new Hashtable();
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Regioes = new SelectList(tjApp.GetAllItens(idAss), "REJU_CD_ID", "REJU_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    SECAO item = Mapper.Map<SecaoViewModel, SECAO>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = baseApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0037", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<SECAO>();
                    Session["ListaSecao"] = null;
                    Session["Secoes"] = baseApp.GetAllItens(idAss);
                    return RedirectToAction("MontarTelaSecao");
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
        public ActionResult EditarSecao(Int32 id)
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
                    Session["MensSecao"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }


            // Prepara view
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Regioes = new SelectList(tjApp.GetAllItens(idAss), "REJU_CD_ID", "REJU_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensSecao"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }

            SECAO item = baseApp.GetItemById(id);
            objetoAntes = item;
            Session["Secao"] = item;
            Session["IdVolta"] = id;
            SecaoViewModel vm = Mapper.Map<SECAO, SecaoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarSecao(SecaoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Regioes = new SelectList(tjApp.GetAllItens(idAss), "REJU_CD_ID", "REJU_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    SECAO item = Mapper.Map<SecaoViewModel, SECAO>(vm);
                    Int32 volta = baseApp.ValidateEdit(item, (SECAO)Session["Secao"], usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0037", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<SECAO>();
                    Session["ListaSecao"] = null;
                    Session["MensSecao"] = 0;
                    return RedirectToAction("MontarTelaSecao");
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
        public ActionResult ExcluirSecao(Int32 id)
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
                    Session["MensSecao"] = 2;
                    return RedirectToAction("MontarTelaSecao", "Secao");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            SECAO item = baseApp.GetItemById(id);
            objetoAntes = (SECAO)Session["Secao"];
            item.SECA_IN_ATIVO = 0;
            Int32 volta = baseApp.ValidateDelete(item, usuario);
            // Verifica retorno
            if (volta == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0036", CultureInfo.CurrentCulture));
                Session["MensSecao"] = 3;
                return RedirectToAction("MontarTelaSecao", "Secao");
            }
            listaMaster = new List<SECAO>();
            Session["ListaSecao"] = null;
            return RedirectToAction("MontarTelaSecao");
        }


        [HttpGet]
        public ActionResult ReativarSecao(Int32 id)
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
                    Session["MensSecao"] = 2;
                    return RedirectToAction("MontarTelaSecao", "Secao");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            SECAO item = baseApp.GetItemById(id);
            objetoAntes = (SECAO)Session["Secao"];
            item.SECA_IN_ATIVO = 1;
            Int32 volta = baseApp.ValidateReativar(item, usuario);
            listaMaster = new List<SECAO>();
            Session["ListaSecao"] = null;
            return RedirectToAction("MontarTelaSecao");
        }

    }
}