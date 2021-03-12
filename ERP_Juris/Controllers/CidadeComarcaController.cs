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
    public class CidadeComarcaController : Controller
    {
        private readonly ICidadeComarcaAppService baseApp;

        private String msg;
        private Exception exception;
        CIDADE_COMARCA objeto = new CIDADE_COMARCA();
        CIDADE_COMARCA objetoAntes = new CIDADE_COMARCA();
        List<CIDADE_COMARCA> listaMaster = new List<CIDADE_COMARCA>();
        String extensao;

        public CidadeComarcaController(ICidadeComarcaAppService baseApps)
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
        public ActionResult MontarTelaCidadeComarca()
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
            if ((List<CIDADE_COMARCA>)Session["ListaCidadeComarca"] == null)
            {
                listaMaster = baseApp.GetAllItens(idAss);
                Session["ListaCidadeComarca"] = listaMaster;
            }

            ViewBag.Listas = listaMaster;
            ViewBag.Title = "Cidades/Comarcas";
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_NM_NOME");
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.Add(new SelectListItem() { Text = "Cidade", Value = "1" });
            tipo.Add(new SelectListItem() { Text = "Comarca", Value = "2" });
            ViewBag.Tipo = new SelectList(tipo, "Value", "Text");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Indicadores
            ViewBag.CidadeComarca = listaMaster.Count;
            
            // Mensagem
            if ((Int32)Session["MensCidadeComarca"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensCidadeComarca"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0010", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensCidadeComarca"] == 3)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0040", CultureInfo.CurrentCulture));
            }

            // Abre view
            Session["MensCidadeComarca"] = 0;
            objeto = new CIDADE_COMARCA();
            Session["Volta"] = 1;
            return View(objeto);
        }

        public ActionResult RetirarFiltroCidadeComarca()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaCidadeComarca"] = null;
            Session["FiltroCidadeComarca"] = null;
            return RedirectToAction("MontarTelaCidadeComarca");
        }

        public ActionResult MostrarTudoCidadeComarca()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMaster = baseApp.GetAllItensAdm(idAss);
            Session["FiltroCidadeComarca"] = null;
            Session["ListaCidadeComarca"] = listaMaster;
            return RedirectToAction("MontarTelaCidadeComarca");
        }

        [HttpPost]
        public ActionResult FiltrarCidadeComarca(CIDADE_COMARCA item)
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
                List<CIDADE_COMARCA> listaObj = new List<CIDADE_COMARCA>();
                Session["FiltroCidadeComarca"] = item;
                Int32 volta = baseApp.ExecuteFilter(item.CICO_NM_NOME, item.UF_CD_ID.Value, item.CICO_IN_TIPO.Value, idAss, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensCidadeComarca"] = 1;
                    ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
                }

                // Sucesso
                Session["MensCidadeComarca"] = 0;
                listaMaster = listaObj;
                Session["ListaCidadeComarca"] = listaObj;
                return RedirectToAction("MontarTelaCidadeComarca");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaCidadeComarca");
            }
        }

        public ActionResult VoltarBaseCidadeComarca()
        {
            // Verificar login
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaCidadeComarca");
        }

        [HttpGet]
        public ActionResult IncluirCidadeComarca()
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
                    Session["MensCidadeComarca"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_NM_NOME");
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.Add(new SelectListItem() { Text = "Cidade", Value = "1" });
            tipo.Add(new SelectListItem() { Text = "Comarca", Value = "2" });
            ViewBag.Tipo = new SelectList(tipo, "Value", "Text");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Prepara view
            CIDADE_COMARCA item = new CIDADE_COMARCA();
            CidadeComarcaViewModel vm = Mapper.Map<CIDADE_COMARCA, CidadeComarcaViewModel>(item);
            vm.CICO_IN_ATIVO = "1";
            vm.ASSI_CD_ID = idAss;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirCidadeComarca(CidadeComarcaViewModel vm)
        {
            var result = new Hashtable();
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_NM_NOME");
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.Add(new SelectListItem() { Text = "Cidade", Value = "1" });
            tipo.Add(new SelectListItem() { Text = "Comarca", Value = "2" });
            ViewBag.Tipo = new SelectList(tipo, "Value", "Text");
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    CIDADE_COMARCA item = Mapper.Map<CidadeComarcaViewModel, CIDADE_COMARCA>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = baseApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0041", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<CIDADE_COMARCA>();
                    Session["ListaCidadeComarca"] = null;
                    Session["Cidades"] = baseApp.GetAllItens(idAss);
                    return RedirectToAction("MontarTelaCidadeComarca");
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
        public ActionResult EditarCidadeComarca(Int32 id)
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
                    Session["MensCidadeComarca"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }


            // Prepara view
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_NM_NOME");
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.Add(new SelectListItem() { Text = "Cidade", Value = "1" });
            tipo.Add(new SelectListItem() { Text = "Comarca", Value = "2" });
            ViewBag.Tipo = new SelectList(tipo, "Value", "Text");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensCidadeComarca"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }

            CIDADE_COMARCA item = baseApp.GetItemById(id);
            objetoAntes = item;
            Session["CidadeComarca"] = item;
            Session["IdVolta"] = id;
            CidadeComarcaViewModel vm = Mapper.Map<CIDADE_COMARCA, CidadeComarcaViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarCidadeComarca(CidadeComarcaViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_NM_NOME");
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.Add(new SelectListItem() { Text = "Cidade", Value = "1" });
            tipo.Add(new SelectListItem() { Text = "Comarca", Value = "2" });
            ViewBag.Tipo = new SelectList(tipo, "Value", "Text");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    CIDADE_COMARCA item = Mapper.Map<CidadeComarcaViewModel, CIDADE_COMARCA>(vm);
                    Int32 volta = baseApp.ValidateEdit(item, objetoAntes, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0041", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<CIDADE_COMARCA>();
                    Session["ListaCidadeComarca"] = null;
                    Session["MensCidadeComarca"] = 0;
                    return RedirectToAction("MontarTelaCidadeComarca");
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
        public ActionResult ExcluirCidadeComarca(Int32 id)
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
                    Session["MensCidadeComarca"] = 2;
                    return RedirectToAction("MontarTelaCidadeComarca", "CidadeComarca");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            CIDADE_COMARCA item = baseApp.GetItemById(id);
            objetoAntes = (CIDADE_COMARCA)Session["CidadeComarca"];
            item.CICO_IN_ATIVO = "0";
            Int32 volta = baseApp.ValidateDelete(item, usuario);
            // Verifica retorno
            if (volta == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0040", CultureInfo.CurrentCulture));
                Session["MensCidadeComarca"] = 3;
                return RedirectToAction("MontarTelaCidadeComarca", "CidadeComarca");
            }
            listaMaster = new List<CIDADE_COMARCA>();
            Session["ListaCidadeComarca"] = null;
            return RedirectToAction("MontarTelaCidadeComarca");
        }


        [HttpGet]
        public ActionResult ReativarCidadeComarca(Int32 id)
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
                    Session["MensCidadeComarca"] = 2;
                    return RedirectToAction("MontarTelaCidadeComarca", "CidadeComarca");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            CIDADE_COMARCA item = baseApp.GetItemById(id);
            objetoAntes = (CIDADE_COMARCA)Session["CidadeComarca"];
            item.CICO_IN_ATIVO = "1";
            Int32 volta = baseApp.ValidateReativar(item, usuario);
            listaMaster = new List<CIDADE_COMARCA>();
            Session["ListaCidadeComarca"] = null;
            return RedirectToAction("MontarTelaCidadeComarca");
        }
    }
}