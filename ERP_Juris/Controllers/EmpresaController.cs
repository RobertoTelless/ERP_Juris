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
    public class EmpresaController : Controller
    {
        private readonly IEmpresaAppService baseApp;
        private readonly ILogAppService logApp;
        private readonly IUsuarioAppService usuApp;
        private String msg;
        private Exception exception;
        EMPRESA objeto = new EMPRESA();
        EMPRESA objetoAntes = new EMPRESA();
        List<EMPRESA> listaMaster = new List<EMPRESA>();
        String extensao;

        public EmpresaController(IEmpresaAppService baseApps, ILogAppService logApps, IUsuarioAppService usuApps)
        {
            baseApp = baseApps;
            logApp = logApps;
            usuApp = usuApps;
        }

        public ActionResult Voltar()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        public ActionResult DashboardGerencial()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("DashboardGerencial", "BaseAdmin");
        }

        public ActionResult VoltarGeral()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        public ActionResult VoltarBase()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("MontarTelaEmpresa");
        }

        [HttpGet]
        public ActionResult MontarTelaEmpresa()
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
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Carrega Dados
            if ((CONFIGURACAO)Session["Empresa"] == null)
            {
                objeto = baseApp.GetItemById(idAss);
                Session["IdEmpresa"] = objeto.EMPR_CD_ID;
                Session["Empresa"] = objeto;
            }
            ViewBag.Title = "Empresas";

            // Carrega listas
            ViewBag.Usuarios = new SelectList(usuApp.GetAllADM(idAss), "USUA_CD_ID", "USUA_NM_NOME");
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;
            ViewBag.UF = new SelectList(baseApp.GetAllUF(), "UF_CD_ID", "UF_SG_SIGLA");

            // Mensagem
            if ((Int32)Session["MensEmpresa"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensEmpresa"] == 2)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }

            // Abre view
            Session["MensEmpresa"] = 0;
            EmpresaViewModel vm = Mapper.Map<EMPRESA, EmpresaViewModel>(objeto);
            return View(vm);
        }

        [HttpPost]
        public ActionResult MontarTelaEmpresa(EmpresaViewModel vm)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    EMPRESA item = Mapper.Map<EmpresaViewModel, EMPRESA>(vm);
                    Int32 volta = baseApp.ValidateEdit(item, objetoAntes, usuarioLogado);

                    // Verifica retorno

                    // Sucesso
                    Int32 idAss = (Int32)Session["IdAssinante"];
                    objeto = baseApp.GetItemById(idAss);
                    Session["Empresa"] = null;
                    Session["MensEmpresa"] = 0;
                    return RedirectToAction("MontarTelaEmpresa");
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

        [HttpPost]
        public ActionResult UploadFotoEmpresa(HttpPostedFileBase file)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if (file == null)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0019", CultureInfo.CurrentCulture));
                Session["MensEmpresa"] = 3;
                return RedirectToAction("MontarTelaEmpresa");
            }

            // Recupera arquivo
            Int32 idUsu = (Int32)Session["IdUsuario"];
            Int32 idAss = (Int32)Session["IdAssinante"];

            EMPRESA emp = (EMPRESA)Session["Empresa"];
            USUARIO usu = (USUARIO)Session["UserCredentials"];
            var fileName = Path.GetFileName(file.FileName);
            if (fileName.Length > 100)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0024", CultureInfo.CurrentCulture));
                Session["MensEmpresa"] = 4;
                return RedirectToAction("MontarTelaEmpresa");
            }
            String caminho = "/Imagens/" + emp.ASSI_CD_ID.ToString() + "/Empresa/" + emp.EMPR_CD_ID.ToString() + "/Logo/";
            String path = Path.Combine(Server.MapPath(caminho), fileName);

            //Recupera tipo de arquivo
            extensao = Path.GetExtension(fileName);
            String a = extensao;

            // Checa extensão
            if (extensao.ToUpper() == ".JPG" || extensao.ToUpper() == ".GIF" || extensao.ToUpper() == ".PNG" || extensao.ToUpper() == ".JPEG")
            {
                // Salva arquivo
                file.SaveAs(path);

                // Gravar registro
                emp.EMPR_AQ_LOGO = "~" + caminho + fileName;
                objetoAntes = emp;
                Int32 volta = baseApp.ValidateEdit(emp, objetoAntes, usu);
            }
            else
            {
                Session["MensEmpresa"] = 5;
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0021", CultureInfo.CurrentCulture));
            }
            return RedirectToAction("MontarTelaEmpresa");
        }

        [HttpPost]
        public JsonResult PesquisaCEP_Javascript(String cep, int tipoEnd)
        {
            // Chama servico ECT
            //Address end = ExternalServices.ECT_Services.GetAdressCEP(item.CLIE_NR_CEP_BUSCA);
            //Endereco end = ExternalServices.ECT_Services.GetAdressCEPService(item.CLIE_NR_CEP_BUSCA);
            EMPRESA cli = baseApp.GetItemById((Int32)Session["IdEmpresa"]);

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
                hash.Add("EMPR_NM_ENDERECO", end.Address + "/" + end.Complement);
                hash.Add("EMPR_NM_BAIRRO", end.District);
                hash.Add("EMPR_NM_CIDADE", end.City);
                hash.Add("UF_CD_ID", baseApp.GetItemBySigla(end.Uf).UF_CD_ID);
                hash.Add("EMPR_NR_CEP", cep);
            }
            else if (tipoEnd == 2)
            {
                hash.Add("EMPR_NM_ENDERECO", end.Address + "/" + end.Complement);
                hash.Add("EMPR_NM_BAIRRO", end.District);
                hash.Add("EMPR_NM_CIDADE", end.City);
                hash.Add("UF_CD_ID", baseApp.GetItemBySigla(end.Uf).UF_CD_ID);
                hash.Add("EMPR_NR_CEP", cep);
            }

            // Retorna
            Session["VoltaCEP"] = 2;
            return Json(hash);
        }
    }
}