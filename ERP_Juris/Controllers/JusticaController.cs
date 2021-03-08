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
    public class JusticaController : Controller
    {
        private readonly ITipoAcaoAppService taApp;
        private readonly ITipoJusticaAppService tjApp;
        private readonly IUsuarioAppService usuApp;
        private readonly IRegiaoJusticaAppService reApp;
        private readonly ISecaoAppService seApp;
        private readonly ISubsecaoAppService suApp;

        private String msg;
        private Exception exception;
        TIPO_ACAO objetoTa = new TIPO_ACAO();
        TIPO_ACAO objetoTaAntes = new TIPO_ACAO();
        List<TIPO_ACAO> listaMasterTa = new List<TIPO_ACAO>();
        TIPO_JUSTICA objetoTj = new TIPO_JUSTICA();
        TIPO_JUSTICA objetoTjAntes = new TIPO_JUSTICA();
        List<TIPO_JUSTICA> listaMasterTj = new List<TIPO_JUSTICA>();
        REGIAO_JUSTICA objetoRj = new REGIAO_JUSTICA();
        REGIAO_JUSTICA objetoRjAntes = new REGIAO_JUSTICA();
        List<REGIAO_JUSTICA> listaMasterRj = new List<REGIAO_JUSTICA>();
        SECAO objetoSe = new SECAO();
        SECAO objetoSeAntes = new SECAO();
        List<SECAO> listaMasterSe = new List<SECAO>();
        SUBSECAO objetoSu = new SUBSECAO();
        SUBSECAO objetoSuAntes = new SUBSECAO();
        List<SUBSECAO> listaMasterSu = new List<SUBSECAO>();
        String extensao;

        public JusticaController(ITipoAcaoAppService taApps, ITipoJusticaAppService tjApps, IUsuarioAppService usuApps, IRegiaoJusticaAppService reApps, ISecaoAppService seApps, ISubsecaoAppService suApps)
        {
            taApp = taApps;
            tjApp = tjApps;
            usuApp = usuApps;
            reApp = reApps;
            seApp = seApps;
            suApp = suApps;
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
        public ActionResult MontarTelaTipoAcao()
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
            if ((List<TIPO_ACAO>)Session["ListaTipoAcao"] == null)
            {
                listaMasterTa = taApp.GetAllItens(idAss);
                Session["ListaTipoAcao"] = listaMasterTa;
            }

            ViewBag.Listas = listaMasterTa;
            ViewBag.Title = "Tipos de Ação";
            //ViewBag.Cats = new SelectList(fornApp.GetAllCategorias(idAss), "CAFO_CD_ID", "CAFO_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Indicadores
            ViewBag.TiposAcao = listaMasterTa.Count;
            
            // Mensagem
            if ((Int32)Session["MensTipoAcao"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTipoAcao"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0010", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTipoAcao"] == 3)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0031", CultureInfo.CurrentCulture));
            }

            // Abre view
            Session["MensTipoAcao"] = 0;
            objetoTa = new TIPO_ACAO();
            Session["VoltaTa"] = 1;
            return View(objetoTa);
        }

        public ActionResult RetirarFiltroTipoAcao()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaTipoAcao"] = null;
            Session["FiltroTipoAcao"] = null;
            return RedirectToAction("MontarTelaTipoAcao");
        }

        public ActionResult MostrarTudoTipoAcao()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterTa = taApp.GetAllItensAdm(idAss);
            Session["FiltroTipoAcao"] = null;
            Session["ListaTipoAcao"] = listaMasterTa;
            return RedirectToAction("MontarTelaTipoAcao");
        }

        [HttpPost]
        public ActionResult FiltrarTipoAcao(TIPO_ACAO item)
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
                List<TIPO_ACAO> listaObj = new List<TIPO_ACAO>();
                Session["FiltroTipoAcao"] = item;
                Int32 volta = taApp.ExecuteFilter(item.TIAC_NM_NOME, item.TIAC_DS_DESCRICAO, idAss, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensTipoAcao"] = 1;
                    ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
                }

                // Sucesso
                Session["MensTipoAcao"] = 0;
                listaMasterTa = listaObj;
                Session["ListaTipoAcao"] = listaObj;
                return RedirectToAction("MontarTelaTipoAcao");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaTipoAcao");
            }
        }

        public ActionResult VoltarBaseTipoAcao()
        {
            // Verificar login
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaTipoAcao");
        }

        [HttpGet]
        public ActionResult IncluirTipoAcao()
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
                    Session["MensTipoAcao"] = 2;
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
            TIPO_ACAO item = new TIPO_ACAO();
            TipoAcaoViewModel vm = Mapper.Map<TIPO_ACAO, TipoAcaoViewModel>(item);
            vm.TIAC_IN_ATIVO = 1;
            vm.ASSI_CD_ID = idAss;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirTipoAcao(TipoAcaoViewModel vm)
        {
            var result = new Hashtable();

            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    TIPO_ACAO item = Mapper.Map<TipoAcaoViewModel, TIPO_ACAO>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = taApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0030", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMasterTa = new List<TIPO_ACAO>();
                    Session["ListaTipoAcao"] = null;
                    Session["TiposAcao"] = taApp.GetAllItens(idAss);
                    return RedirectToAction("MontarTelaTipoAcao");
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
        public ActionResult EditarTipoAcao(Int32 id)
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
                    Session["MensTipoAcao"] = 2;
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
            if ((Int32)Session["MensTipoAcao"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }

            TIPO_ACAO item = taApp.GetItemById(id);
            objetoTaAntes = item;
            Session["TipoAcao"] = item;
            Session["IdVolta"] = id;
            TipoAcaoViewModel vm = Mapper.Map<TIPO_ACAO, TipoAcaoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarTipoAcao(TipoAcaoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    TIPO_ACAO item = Mapper.Map<TipoAcaoViewModel, TIPO_ACAO>(vm);
                    Int32 volta = taApp.ValidateEdit(item, objetoTaAntes, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0030", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMasterTa = new List<TIPO_ACAO>();
                    Session["ListaTipoAcao"] = null;
                    Session["MensTipoAcao"] = 0;
                    return RedirectToAction("MontarTelaTipoAcao");
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
        public ActionResult ExcluirTipoAcao(Int32 id)
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
                    Session["MensTipoAcao"] = 2;
                    return RedirectToAction("MontarTelaTipoAcao", "Justica");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            TIPO_ACAO item = taApp.GetItemById(id);
            objetoTaAntes = (TIPO_ACAO)Session["TipoAcao"];
            item.TIAC_IN_ATIVO = 0;
            Int32 volta = taApp.ValidateDelete(item, usuario);
            // Verifica retorno
            if (volta == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0031", CultureInfo.CurrentCulture));
                Session["MensTipoAcao"] = 3;
                return RedirectToAction("MontarTelaTipoAcao", "Justica");
            }
            listaMasterTa = new List<TIPO_ACAO>();
            Session["ListaTipoAcao"] = null;
            return RedirectToAction("MontarTelaTipoAcao");
        }


        [HttpGet]
        public ActionResult ReativarTipoAcao(Int32 id)
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
                    Session["MensTipoAcao"] = 2;
                    return RedirectToAction("MontarTelaTipoAcao", "Justica");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            TIPO_ACAO item = taApp.GetItemById(id);
            objetoTaAntes = (TIPO_ACAO)Session["TipoAcao"];
            item.TIAC_IN_ATIVO = 1;
            Int32 volta = taApp.ValidateReativar(item, usuario);
            listaMasterTa = new List<TIPO_ACAO>();
            Session["ListaTipoAcao"] = null;
            return RedirectToAction("MontarTelaTipoAcao");
        }
    }
}