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
    public class VaraController : Controller
    {
        private readonly IVaraAppService baseApp;
        private readonly IForoAppService foroApp;

        private String msg;
        private Exception exception;
        VARA objeto = new VARA();
        VARA objetoAntes = new VARA();
        List<VARA> listaMaster = new List<VARA>();
        String extensao;

        public VaraController(IVaraAppService baseApps, IForoAppService foroApps)
        {
            baseApp = baseApps;
            foroApp = foroApps;
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
        public ActionResult MontarTelaVara()
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
            if ((List<FORO>)Session["ListaVara"] == null)
            {
                listaMaster = baseApp.GetAllItens(idAss);
                Session["ListaVara"] = listaMaster;
            }

            ViewBag.Listas = listaMaster;
            ViewBag.Title = "Varas";
            ViewBag.Foros = new SelectList(foroApp.GetAllItens(idAss), "FORO_CD_ID", "FORO_NM_NOME");
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_SG_SIGLA");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Indicadores
            ViewBag.Varas = listaMaster.Count;
            
            // Mensagem
            if ((Int32)Session["MensVara"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensVara"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0010", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensVara"] == 3)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0044", CultureInfo.CurrentCulture));
            }

            // Abre view
            Session["MensVara"] = 0;
            objeto = new VARA();
            Session["Volta"] = 1;
            return View(objeto);
        }

        public ActionResult RetirarFiltroVara()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaVara"] = null;
            Session["FiltroVara"] = null;
            return RedirectToAction("MontarTelaVara");
        }

        public ActionResult MostrarTudoVara()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMaster = baseApp.GetAllItensAdm(idAss);
            Session["FiltroVara"] = null;
            Session["ListaVara"] = listaMaster;
            return RedirectToAction("MontarTelaVara");
        }

        [HttpPost]
        public ActionResult FiltrarVara(VARA item)
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
                List<VARA> listaObj = new List<VARA>();
                Session["FiltroForo"] = item;
                Int32 volta = baseApp.ExecuteFilter(item.FORO_CD_ID, item.VARA_NM_NOME, item.VARA_DS_DESCRICAO, item.VARA_NM_JUIZ, item.VARA_NM_CIDADE, item.VARA_NM_BAIRRO, item.UF_CD_ID.Value, idAss, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensVara"] = 1;
                    ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
                }

                // Sucesso
                Session["MensVara"] = 0;
                listaMaster = listaObj;
                Session["ListaVara"] = listaObj;
                return RedirectToAction("MontarTelaVara");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaVara");
            }
        }

        public ActionResult VoltarBaseVara()
        {
            // Verificar login
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaVara");
        }

        [HttpGet]
        public ActionResult IncluirVara()
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
                    Session["MensVara"] = 2;
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
            ViewBag.Foros = new SelectList(foroApp.GetAllItens(idAss), "FORO_CD_ID", "FORO_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Prepara view
            VARA item = new VARA();
            VaraViewModel vm = Mapper.Map<VARA, VaraViewModel>(item);
            vm.VARA_IN_ATIVO = 1;
            vm.ASSI_CD_ID = idAss;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirVara(VaraViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            var result = new Hashtable();
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_SG_SIGLA");
            ViewBag.Foros = new SelectList(foroApp.GetAllItens(idAss), "FORO_CD_ID", "FORO_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    VARA item = Mapper.Map<VaraViewModel, VARA>(vm);
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    Int32 volta = baseApp.ValidateCreate(item, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0045", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<VARA>();
                    Session["ListaVara"] = null;
                    Session["Varas"] = baseApp.GetAllItens(idAss);
                    return RedirectToAction("MontarTelaVara");
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
        public ActionResult EditarVara(Int32 id)
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
                    Session["MensVara"] = 2;
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
            ViewBag.Foros = new SelectList(foroApp.GetAllItens(idAss), "FORO_CD_ID", "FORO_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensVara"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }

            VARA item = baseApp.GetItemById(id);
            objetoAntes = item;
            Session["Vara"] = item;
            Session["IdVolta"] = id;
            VaraViewModel vm = Mapper.Map<VARA, VaraViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditarVara(VaraViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_SG_SIGLA");
            ViewBag.Foros = new SelectList(foroApp.GetAllItens(idAss), "FORO_CD_ID", "FORO_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuario = (USUARIO)Session["UserCredentials"];
                    VARA item = Mapper.Map<VaraViewModel, VARA>(vm);
                    Int32 volta = baseApp.ValidateEdit(item, objetoAntes, usuario);

                    // Verifica retorno
                    if (volta == 1)
                    {
                        ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0045", CultureInfo.CurrentCulture));
                        return View(vm);
                    }

                    // Sucesso
                    listaMaster = new List<VARA>();
                    Session["ListaVara"] = null;
                    Session["MensVara"] = 0;
                    return RedirectToAction("MontarTelaVara");
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
        public ActionResult ExcluirVara(Int32 id)
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
                    Session["MensVara"] = 2;
                    return RedirectToAction("MontarTelaVara", "Vara");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            VARA item = baseApp.GetItemById(id);
            objetoAntes = (VARA)Session["Vara"];
            item.VARA_IN_ATIVO = 0;
            Int32 volta = baseApp.ValidateDelete(item, usuario);
            // Verifica retorno
            if (volta == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0044", CultureInfo.CurrentCulture));
                Session["MensVara"] = 3;
                return RedirectToAction("MontarTelaVara", "Vara");
            }
            listaMaster = new List<VARA>();
            Session["ListaVara"] = null;
            return RedirectToAction("MontarTelaVara");
        }


        [HttpGet]
        public ActionResult ReativarVara(Int32 id)
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
                    Session["MensVara"] = 2;
                    return RedirectToAction("MontarTelaVara", "Vara");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Executar
            VARA item = baseApp.GetItemById(id);
            objetoAntes = (VARA)Session["Vara"];
            item.VARA_IN_ATIVO = 1;
            Int32 volta = baseApp.ValidateReativar(item, usuario);
            listaMaster = new List<VARA>();
            Session["ListaVara"] = null;
            return RedirectToAction("MontarTelaVara");
        }

        [HttpPost]
        public JsonResult PesquisaCEP_Javascript(String cep, int tipoEnd)
        {
            // Chama servico ECT
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
                hash.Add("VARA_NM_ENDERECO", end.Address);
                hash.Add("VARA_NR_NUMERO", end.Complement);
                hash.Add("VARA_NM_BAIRRO", end.District);
                hash.Add("UF_CD_ID", baseApp.GetUFBySigla(end.Uf).UF_CD_ID);
                hash.Add("VARA_NR_CEP", cep);
            }

            // Retorna
            Session["VoltaCEP"] = 2;
            return Json(hash);
        }
    }
}