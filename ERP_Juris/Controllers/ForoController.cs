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
using Canducci.Zip;

namespace ERP_Juris.Controllers
{
    public class ForoController : Controller
    {
        private readonly IForoAppService baseApp;
        private readonly ICidadeComarcaAppService cidApp;

        private String msg;
        private Exception exception;
        FORO objeto = new FORO();
        FORO objetoAntes = new FORO();
        List<FORO> listaMaster = new List<FORO>();
        String extensao;

        public ForoController(IForoAppService baseApps, ICidadeComarcaAppService cidApps)
        {
            baseApp = baseApps;
            cidApp = cidApps;
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
        public ActionResult MontarTelaForo()
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
            if ((List<FORO>)Session["ListaForo"] == null)
            {
                listaMaster = baseApp.GetAllItens(idAss);
                Session["ListaForo"] = listaMaster;
            }

            ViewBag.Listas = listaMaster;
            ViewBag.Title = "Foros";
            ViewBag.Cidades = new SelectList(cidApp.GetAllItens(idAss), "CICO_CD_ID", "CICO_NM_NOME");
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_SG_SIGLA");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Indicadores
            ViewBag.Foros = listaMaster.Count;
            
            // Mensagem
            if ((Int32)Session["MensForo"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensForo"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0010", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensForo"] == 3)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0042", CultureInfo.CurrentCulture));
            }

            // Abre view
            Session["MensForo"] = 0;
            objeto = new FORO();
            Session["Volta"] = 1;
            return View(objeto);
        }

        public ActionResult RetirarFiltroForo()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaForo"] = null;
            Session["FiltroForo"] = null;
            return RedirectToAction("MontarTelaForo");
        }

        public ActionResult MostrarTudoForo()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMaster = baseApp.GetAllItensAdm(idAss);
            Session["FiltroForo"] = null;
            Session["ListaForo"] = listaMaster;
            return RedirectToAction("MontarTelaForo");
        }

        [HttpPost]
        public ActionResult FiltrarForo(FORO item)
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
                List<FORO> listaObj = new List<FORO>();
                Session["FiltroForo"] = item;
                Int32 volta = baseApp.ExecuteFilter(item.CICO_CD_ID, item.FORO_NM_NOME, item.FORO_DS_DESCRICAO, item.FORO_NM_BAIRRO, item.UF_CD_ID.Value, idAss, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensForo"] = 1;
                    ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
                }

                // Sucesso
                Session["MensForo"] = 0;
                listaMaster = listaObj;
                Session["ListaForo"] = listaObj;
                return RedirectToAction("MontarTelaForo");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaForo");
            }
        }

        public ActionResult VoltarBaseForo()
        {
            // Verificar login
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaForo");
        }

        [HttpGet]
        public ActionResult IncluirForo()
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
                    Session["MensForo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Prepara listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_SG_SIGLA");
            ViewBag.Cidades = new SelectList(cidApp.GetAllItens(idAss), "CICO_CD_ID", "CICO_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Prepara view
            FORO item = new FORO();
            ForoViewModel vm = Mapper.Map<FORO, ForoViewModel>(item);
            vm.FORO_IN_ATIVO = 1;
            vm.ASSI_CD_ID = idAss;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirForo(ForoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            var result = new Hashtable();
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_SG_SIGLA");
            ViewBag.Cidades = new SelectList(cidApp.GetAllItens(idAss), "CICO_CD_ID", "CICO_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    FORO item = Mapper.Map<ForoViewModel, FORO>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = baseApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0043", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<FORO>();
                    Session["ListaForo"] = null;
                    Session["Foros"] = baseApp.GetAllItens(idAss);
                    return RedirectToAction("MontarTelaForo");
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
        public ActionResult EditarForo(Int32 id)
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
                    Session["MensForo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }


            // Prepara view
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_SG_SIGLA");
            ViewBag.Cidades = new SelectList(cidApp.GetAllItens(idAss), "CICO_CD_ID", "CICO_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensForo"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }

            FORO item = baseApp.GetItemById(id);
            objetoAntes = item;
            Session["Foro"] = item;
            Session["IdVolta"] = id;
            ForoViewModel vm = Mapper.Map<FORO, ForoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarForo(ForoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_SG_SIGLA");
            ViewBag.Cidades = new SelectList(cidApp.GetAllItens(idAss), "CICO_CD_ID", "CICO_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    FORO item = Mapper.Map<ForoViewModel, FORO>(vm);
                    Int32 volta = baseApp.ValidateEdit(item, objetoAntes, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0043", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<FORO>();
                    Session["ListaForo"] = null;
                    Session["MensForo"] = 0;
                    return RedirectToAction("MontarTelaForo");
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
        public ActionResult ExcluirForo(Int32 id)
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
                    Session["MensForo"] = 2;
                    return RedirectToAction("MontarTelaForo", "Foro");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            FORO item = baseApp.GetItemById(id);
            objetoAntes = (FORO)Session["Foro"];
            item.FORO_IN_ATIVO = 0;
            Int32 volta = baseApp.ValidateDelete(item, usuario);
            // Verifica retorno
            if (volta == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0042", CultureInfo.CurrentCulture));
                Session["MensForo"] = 3;
                return RedirectToAction("MontarTelaForo", "Foro");
            }
            listaMaster = new List<FORO>();
            Session["ListaForo"] = null;
            return RedirectToAction("MontarTelaForo");
        }


        [HttpGet]
        public ActionResult ReativarForo(Int32 id)
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
                    Session["MensForo"] = 2;
                    return RedirectToAction("MontarTelaForo", "Foro");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            FORO item = baseApp.GetItemById(id);
            objetoAntes = (FORO)Session["Foro"];
            item.FORO_IN_ATIVO = 1;
            Int32 volta = baseApp.ValidateReativar(item, usuario);
            listaMaster = new List<FORO>();
            Session["ListaForo"] = null;
            return RedirectToAction("MontarTelaForo");
        }

        [HttpPost]
        public JsonResult PesquisaCEP_Javascript(String cep, int tipoEnd)
        {
            // Chama servico ECT
            //FORO cli = baseApp.GetItemById((Int32)Session["IdVolta"]);

            ZipCodeLoad zipLoad = new ZipCodeLoad();
            ZipCodeInfo end = new ZipCodeInfo();
            ZipCode zipCode = null;
            cep = CrossCutting.ValidarNumerosDocumentos.RemoveNaoNumericos(cep);
            if (ZipCode.TryParse(cep, out zipCode))
            {
                end = zipLoad.Find(zipCode);
            }

            // Atualiza
            var hash = new Hashtable();

            if (tipoEnd == 1)
            {
                hash.Add("FORO_NM_ENDERECO", end.Address);
                hash.Add("FORO_NR_NUMERO", end.Complement);
                hash.Add("FORO_NM_BAIRRO", end.District);
                hash.Add("UF_CD_ID", baseApp.GetUFBySigla(end.Uf).UF_CD_ID);
                hash.Add("FORO_NR_CEP", cep);
            }

            // Retorna
            Session["VoltaCEP"] = 2;
            return Json(hash);
        }

    }
}