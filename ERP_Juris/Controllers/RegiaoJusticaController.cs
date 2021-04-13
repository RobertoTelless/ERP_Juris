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
    public class RegiaoJusticaController : Controller
    {
        private readonly IRegiaoJusticaAppService baseApp;
        private readonly ITipoJusticaAppService tjApp;

        private String msg;
        private Exception exception;
        REGIAO_JUSTICA objeto = new REGIAO_JUSTICA();
        REGIAO_JUSTICA objetoAntes = new REGIAO_JUSTICA();
        List<REGIAO_JUSTICA> listaMaster = new List<REGIAO_JUSTICA>();
        String extensao;

        public RegiaoJusticaController(IRegiaoJusticaAppService baseApps)
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
        public ActionResult MontarTelaRegiaoJustica()
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
            if ((List<REGIAO_JUSTICA>)Session["ListaRegiaoJustica"] == null)
            {
                listaMaster = baseApp.GetAllItens(idAss);
                Session["ListaRegiaoJustica"] = listaMaster;
            }

            ViewBag.Listas = (List<REGIAO_JUSTICA>)Session["ListaRegiaoJustica"];
            ViewBag.Title = "Regiões de Justiça";
            ViewBag.Tipos = new SelectList(tjApp.GetAllItens(idAss), "TIJU_CD_ID", "TIJU_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Indicadores
            ViewBag.Regioes = ((List<REGIAO_JUSTICA>)Session["ListaRegiaoJustica"]).Count;
            
            // Mensagem
            if ((Int32)Session["MensRegiaoJustica"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensRegiaoJustica"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0010", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensRegiaoJustica"] == 3)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0034", CultureInfo.CurrentCulture));
            }

            // Abre view
            Session["MensRegiaoJustica"] = 0;
            objeto = new REGIAO_JUSTICA();
            Session["Volta"] = 1;
            return View(objeto);
        }

        public ActionResult RetirarFiltroRegiaoJustica()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaRegiaoJustica"] = null;
            Session["FiltroRegiaoJustica"] = null;
            return RedirectToAction("MontarTelaRegiaoJustica");
        }

        public ActionResult MostrarTudoRegiaoJustica()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMaster = baseApp.GetAllItensAdm(idAss);
            Session["FiltroRegiaoJustica"] = null;
            Session["ListaRegiaoJustica"] = listaMaster;
            return RedirectToAction("MontarTelaRegiaoJustica");
        }

        [HttpPost]
        public ActionResult FiltrarRegiaoJustica(REGIAO_JUSTICA item)
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
                List<REGIAO_JUSTICA> listaObj = new List<REGIAO_JUSTICA>();
                Session["FiltroRegiaoJustica"] = item;
                Int32 volta = baseApp.ExecuteFilter(item.TIJU_CD_ID, item.REJU_NM_NOME, item.REJU_DS_DESCRICAO, idAss, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensRegiaoJustica"] = 1;
                    ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
                }

                // Sucesso
                Session["MensRegiaoJustica"] = 0;
                listaMaster = listaObj;
                Session["ListaRegiaoJustica"] = listaObj;
                return RedirectToAction("MontarTelaRegiaoJustica");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaRegiaoJustica");
            }
        }

        public ActionResult VoltarBaseRegiaoJustica()
        {
            // Verificar login
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaRegiaoJustica");
        }

        [HttpGet]
        public ActionResult IncluirRegiaoJustica()
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
                    Session["MensRegiaoJustica"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Tipos = new SelectList(tjApp.GetAllItens(idAss), "TIJU_CD_ID", "TIJU_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Prepara view
            REGIAO_JUSTICA item = new REGIAO_JUSTICA();
            RegiaoJusticaViewModel vm = Mapper.Map<REGIAO_JUSTICA, RegiaoJusticaViewModel>(item);
            vm.REJU_IN_ATIVO = 1;
            vm.ASSI_CD_ID = idAss;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirRegiaoJustica(RegiaoJusticaViewModel vm)
        {
            var result = new Hashtable();
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Tipos = new SelectList(tjApp.GetAllItens(idAss), "TIJU_CD_ID", "TIJU_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    REGIAO_JUSTICA item = Mapper.Map<RegiaoJusticaViewModel, REGIAO_JUSTICA>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = baseApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0035", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<REGIAO_JUSTICA>();
                    Session["ListaRegiaoJustica"] = null;
                    Session["RegiaoesJustica"] = baseApp.GetAllItens(idAss);
                    return RedirectToAction("MontarTelaRegiaoJustica");
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
        public ActionResult EditarRegiaoJustica(Int32 id)
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
                    Session["MensRegiaoJustica"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }


            // Prepara view
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Tipos = new SelectList(tjApp.GetAllItens(idAss), "TIJU_CD_ID", "TIJU_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensRegiaoJustica"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }

            REGIAO_JUSTICA item = baseApp.GetItemById(id);
            objetoAntes = item;
            Session["RegiaoJustica"] = item;
            Session["IdVolta"] = id;
            RegiaoJusticaViewModel vm = Mapper.Map<REGIAO_JUSTICA, RegiaoJusticaViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarRegiaoJustica(RegiaoJusticaViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Tipos = new SelectList(tjApp.GetAllItens(idAss), "TIJU_CD_ID", "TIJU_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    REGIAO_JUSTICA item = Mapper.Map<RegiaoJusticaViewModel, REGIAO_JUSTICA>(vm);
                    Int32 volta = baseApp.ValidateEdit(item, (REGIAO_JUSTICA)Session["RegiaoJustica"], usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0035", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<REGIAO_JUSTICA>();
                    Session["ListaRegiaoJustica"] = null;
                    Session["MensRegiaoJustica"] = 0;
                    return RedirectToAction("MontarTelaRegiaoJustica");
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
        public ActionResult ExcluirRegiaoJustica(Int32 id)
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
                    Session["MensRegiaoJustica"] = 2;
                    return RedirectToAction("MontarTelaRegiaoJustica", "RegiaoJustica");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            REGIAO_JUSTICA item = baseApp.GetItemById(id);
            objetoAntes = (REGIAO_JUSTICA)Session["RegiaoJustica"];
            item.REJU_IN_ATIVO = 0;
            Int32 volta = baseApp.ValidateDelete(item, usuario);
            // Verifica retorno
            if (volta == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0034", CultureInfo.CurrentCulture));
                Session["MensRegiaoJustica"] = 3;
                return RedirectToAction("MontarTelaRegiaoJustica", "RegiaoJustica");
            }
            listaMaster = new List<REGIAO_JUSTICA>();
            Session["ListaRegiaoJustica"] = null;
            return RedirectToAction("MontarTelaRegiaoJustica");
        }


        [HttpGet]
        public ActionResult ReativarRegiaoJustica(Int32 id)
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
                    Session["MensRegiaoJustica"] = 2;
                    return RedirectToAction("MontarTelaRegiaoJustica", "RegiaoJustica");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            REGIAO_JUSTICA item = baseApp.GetItemById(id);
            objetoAntes = (REGIAO_JUSTICA)Session["RegiaoJustica"];
            item.REJU_IN_ATIVO = 1;
            Int32 volta = baseApp.ValidateReativar(item, usuario);
            listaMaster = new List<REGIAO_JUSTICA>();
            Session["ListaRegiaoJustica"] = null;
            return RedirectToAction("MontarTelaRegiaoJustica");
        }
    }
}