﻿using System;
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
    public class AgendaController : Controller
    {
        private readonly IAgendaAppService baseApp;
        private readonly ILogAppService logApp;
        private readonly IUsuarioAppService usuApp;
        private readonly ICategoriaAgendaAppService caApp;

        private String msg;
        private Exception exception;
        AGENDA objeto = new AGENDA();
        AGENDA objetoAntes = new AGENDA();
        List<AGENDA> listaMaster = new List<AGENDA>();
        List<Hashtable> listaCalendario = new List<Hashtable>();
        String extensao;

        public AgendaController(IAgendaAppService baseApps, ILogAppService logApps, IUsuarioAppService usuApps, ICategoriaAgendaAppService caApps)
        {
            baseApp = baseApps;
            logApp = logApps;
            usuApp = usuApps;
            caApp = caApps;
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
            listaMaster = new List<AGENDA>();
            Session["Agenda"] = null;
            return RedirectToAction("CarregarAdmin", "BaseAdmin");
        }

        [HttpGet]
        public ActionResult MontarTelaAgendaCalendario()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["FiltroAgendaCalendario"] = 2;
            var usuario = (USUARIO)Session["UserCredentials"];
            Int32 idAss = (Int32)Session["IdAssinante"];

            ViewBag.Tipos = new SelectList(caApp.GetAllItens(idAss), "CAAG_CD_ID", "CAAG_NM_NOME");
            ViewBag.Usuarios = new SelectList(usuApp.GetAllItens(idAss), "USUA_CD_ID", "USUA_NM_NOME");

            AGENDA item = new AGENDA();
            AgendaViewModel vm = Mapper.Map<AGENDA, AgendaViewModel>(item);
            vm.ASSI_CD_ID = idAss;
            vm.AGEN_DT_DATA = DateTime.Today.Date;
            vm.AGEN_IN_ATIVO = 1;
            vm.USUA_CD_ID = usuario.USUA_CD_ID;
            vm.AGEN_IN_STATUS = 1;

            if (Session["ListaAgenda"] == null)
            {
                listaMaster = baseApp.GetByUser(usuario.USUA_CD_ID,idAss);
                Session["ListaAgenda"] = listaMaster;
            }
            ViewBag.Title = "Agenda";
            return View(vm);
        }

        [HttpPost]
        public JsonResult GetEventosCalendario()
        {
            var usuario = (USUARIO)Session["UserCredentials"];
            Int32 idAss = (Int32)Session["IdAssinante"];

            if (Session["ListaAgenda"] == null)
            {
                listaMaster = baseApp.GetByUser(usuario.USUA_CD_ID, idAss);
                Session["ListaAgenda"] = listaMaster;
            }

            foreach (var item in listaMaster)
            {
                var hash = new Hashtable();

                hash.Add("id", item.AGEN_CD_ID);
                hash.Add("title", item.AGEN_NM_TITULO);
                hash.Add("start", (item.AGEN_DT_DATA + item.AGEN_HR_HORA).ToString("yyyy-MM-dd HH:mm:ss"));
                hash.Add("description", (new DateTime() + item.AGEN_HR_HORA).ToString("HH:mm"));

                listaCalendario.Add(hash);
            }

            return Json(listaCalendario);
        }

        [HttpPost]
        public JsonResult GetDetalhesEvento(Int32 id)
        {
            var evento = baseApp.GetById(id);

            var hash = new Hashtable();
            hash.Add("data", evento.AGEN_DT_DATA.ToShortDateString());
            hash.Add("hora", evento.AGEN_HR_HORA.ToString());
            hash.Add("categoria", evento.CATEGORIA_AGENDA.CAAG_NM_NOME);
            hash.Add("titulo", evento.AGEN_NM_TITULO);
            hash.Add("contato", evento.USUARIO1 == null ? "-" : evento.USUARIO1.USUA_NM_NOME);
            if (evento.AGEN_IN_STATUS == 1)
            {
                hash.Add("status", "Ativo");
            }
            else if (evento.AGEN_IN_STATUS == 2)
            {
                hash.Add("status", "Suspenso");
            }
            else
            {
                hash.Add("status", "Encerrado");
            }
            hash.Add("anexo", evento.AGENDA_ANEXO.Count);

            return Json(hash);
        }

        [HttpGet]
        public ActionResult MontarTelaAgenda()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["FiltroAgendaCalendario"] = 1;
            var usuario = (USUARIO)Session["UserCredentials"];
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Carrega listas
            if (Session["ListaAgenda"] == null)
            {
                listaMaster = baseApp.GetByUser(usuario.USUA_CD_ID, idAss);
                Session["ListaAgenda"] = listaMaster;
            }
            ViewBag.Listas = (List<AGENDA>)Session["ListaAgenda"];
            ViewBag.Itens = ((List<AGENDA>)Session["ListaAgenda"]).Count;
            ViewBag.Title = "Agenda";
            ViewBag.Tipos = new SelectList(caApp.GetAllItens(idAss), "CAAG_CD_ID", "CAAG_NM_NOME");

            // Indicadores
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensAgenda"] == 1)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }

            // Abre view
            objeto = new AGENDA();
            objeto.AGEN_DT_DATA = DateTime.Today.Date;
            Session["VoltaAgenda"] = 1;
            Session["MensAgenda"] = 0;
            return View(objeto);
        }

        public ActionResult RetirarFiltroAgenda()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Session["ListaAgenda"] = null;
            if ((Int32)Session["VoltaAgenda"] == 2)
            {
                return RedirectToAction("VerTimelineAgenda");
            }
            return RedirectToAction("MontarTelaAgenda");
        }

        public ActionResult MostrarTudoAgenda()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            var usuario = (USUARIO)Session["UserCredentials"];
            listaMaster = baseApp.GetAllItensAdm(idAss).Where(p => p.USUA_CD_ID == usuario.USUA_CD_ID).ToList();
            Session["ListaAgenda"] = listaMaster;
            if ((Int32)Session["VoltaAgenda"] == 2)
            {
                return RedirectToAction("VerTimelineAgenda");
            }
            return RedirectToAction("MontarTelaAgenda");
        }

        [HttpPost]
        public ActionResult FiltrarAgenda(AGENDA item)
        {
            try
            {
                if ((String)Session["Ativa"] == null)
                {
                    return RedirectToAction("Login", "ControleAcesso");
                }
                // Executa a operação
                Int32 idAss = (Int32)Session["IdAssinante"];
                var usuario = (USUARIO)Session["UserCredentials"];
                List<AGENDA> listaObj = new List<AGENDA>();
                Int32 volta = baseApp.ExecuteFilter(item.AGEN_DT_DATA, item.CAAG_CD_ID, item.AGEN_NM_TITULO, item.AGEN_DS_DESCRICAO, idAss, out listaObj);

                // Verifica retorno
                if (volta == 1)
                {
                    Session["MensAgenda"] = 1;
                    ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
                }

                // Sucesso
                listaMaster = listaObj;
                Session["ListaAgenda"] = listaObj;
                if ((Int32)Session["VoltaAgenda"] == 2)
                {
                    return RedirectToAction("VerTimelineAgenda");
                }
                return RedirectToAction("MontarTelaAgenda");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("MontarTelaAgenda");
            }
        }

        public ActionResult VoltarBaseAgenda()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((Int32)Session["VoltaAgenda"] == 2)
            {
                return RedirectToAction("VerTimelineAgenda");
            }
            else if ((Int32)Session["FiltroAgendaCalendario"] == 1)
            {
                return RedirectToAction("MontarTelaAgenda");
            }
            else if ((Int32)Session["FiltroAgendaCalendario"] == 2)
            {
                return RedirectToAction("MontarTelaAgendaCalendario");
            }
            else
            {
                return RedirectToAction("MontarTelaAgenda");
            }
        }

        [HttpGet]
        public ActionResult IncluirAgenda()
        {
            // Prepara listas
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Tipos = new SelectList(caApp.GetAllItens(idAss), "CAAG_CD_ID", "CAAG_NM_NOME");
            ViewBag.Usuarios = new SelectList(usuApp.GetAllItens(idAss), "USUA_CD_ID", "USUA_NM_NOME");

            // Prepara view
            USUARIO usuario = (USUARIO)Session["UserCredentials"];

            AGENDA item = new AGENDA();
            //if (SessionMocks.idAtendimentoAgenda != null)
            //{
            //    item = SessionMocks.agenda;
            //}
            AgendaViewModel vm = Mapper.Map<AGENDA, AgendaViewModel>(item);
            vm.AGEN_DT_DATA = DateTime.Today.Date;
            vm.ASSI_CD_ID = usuario.ASSI_CD_ID;
            vm.AGEN_IN_ATIVO = 1;
            vm.USUA_CD_ID = usuario.USUA_CD_ID;
            vm.AGEN_IN_STATUS = 1;
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult IncluirAgenda(AgendaViewModel vm)
        {
            var result = new Hashtable();

            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Tipos = new SelectList(caApp.GetAllItens(idAss), "CAAG_CD_ID", "CAAG_NM_NOME");
            ViewBag.Usuarios = new SelectList(usuApp.GetAllItens(idAss), "USUA_CD_ID", "USUA_NM_NOME");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    AGENDA item = Mapper.Map<AgendaViewModel, AGENDA>(vm);
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 volta = baseApp.ValidateCreate(item, usuarioLogado);

                    // Cria pastas
                    String caminho = "/Imagens/Agenda/" + idAss.ToString() + "/" + item.AGEN_CD_ID.ToString() + "/Anexos/";
                    Directory.CreateDirectory(Server.MapPath(caminho));
                    
                    // Sucesso
                    listaMaster = new List<AGENDA>();
                    Session["ListaAgenda"] = null;

                    Session["IdVolta"] = item.AGEN_CD_ID;

                    //if (SessionMocks.idAtendimentoAgenda != null)
                    //{
                    //    ATENDIMENTO_AGENDA aa = new ATENDIMENTO_AGENDA();
                    //    aa.AGEN_CD_ID = item.AGEN_CD_ID;
                    //    aa.ATEN_CD_ID = (Int32)SessionMocks.idAtendimentoAgenda;
                    //    aa.ATAG_IN_ATIVO = 1;

                    //    Int32 voltaAA = atenAgenApp.ValidateCreate(aa, usuarioLogado);

                    //    result.Add("idAtendimento", SessionMocks.idAtendimentoAgenda);
                    //    return Json(result);
                    //}
                    //else
                    //{
                    result.Add("id", item.AGEN_CD_ID);
                    return Json(result);
                    //}
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
        public ActionResult EditarAgenda(Int32 id)
        {
            // Prepara view
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Tipos = new SelectList(caApp.GetAllItens(idAss), "CAAG_CD_ID", "CAAG_NM_NOME");
            ViewBag.Usuarios = new SelectList(usuApp.GetAllItens(idAss), "USUA_CD_ID", "USUA_NM_NOME");
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem() { Text = "Ativo", Value = "1" });
            status.Add(new SelectListItem() { Text = "Suspenso", Value = "2" });
            status.Add(new SelectListItem() { Text = "Encerrado", Value = "3" });
            ViewBag.Status = new SelectList(status, "Value", "Text");

            AGENDA item = baseApp.GetItemById(id);
            objetoAntes = item;
            Session["Agenda"] = item;
            Session["IdVolta"] = id;
            AgendaViewModel vm = Mapper.Map<AGENDA, AgendaViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarAgenda(AgendaViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Tipos = new SelectList(caApp.GetAllItens(idAss), "CAAG_CD_ID", "CAAG_NM_NOME");
            ViewBag.Usuarios = new SelectList(usuApp.GetAllItens(idAss), "USUA_CD_ID", "USUA_NM_NOME");
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem() { Text = "Ativo", Value = "1" });
            status.Add(new SelectListItem() { Text = "Suspenso", Value = "2" });
            status.Add(new SelectListItem() { Text = "Encerrado", Value = "3" });
            ViewBag.Status = new SelectList(status, "Value", "Text");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    AGENDA item = Mapper.Map<AgendaViewModel, AGENDA>(vm);
                    Int32 volta = baseApp.ValidateEdit(item, objetoAntes, usuarioLogado);

                    // Verifica retorno

                    // Sucesso
                    listaMaster = new List<AGENDA>();
                    Session["ListaAgenda"] = null;
                    return RedirectToAction("MontarTelaAgenda");
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
        public ActionResult ExcluirAgenda(Int32 id)
        {
            // Verifica se tem usuario logado
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            USUARIO usu = (USUARIO)Session["UserCredentials"];

            // Executar
            AGENDA item = baseApp.GetItemById(id);
            objetoAntes = (AGENDA)Session["Agenda"];
            item.AGEN_IN_ATIVO = 0;
            Int32 volta = baseApp.ValidateDelete(item, usu);
            listaMaster = new List<AGENDA>();
            Session["ListaAgenda"] = null;
            return RedirectToAction("MontarTelaAgenda");
        }

        [HttpGet]
        public ActionResult ReativarAgenda(Int32 id)
        {
            // Verifica se tem usuario logado
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            USUARIO usu = (USUARIO)Session["UserCredentials"];
            // Executar
            AGENDA item = baseApp.GetItemById(id);
            objetoAntes = (AGENDA)Session["Agenda"];
            item.AGEN_IN_ATIVO = 1;
            Int32 volta = baseApp.ValidateReativar(item, usu);
            listaMaster = new List<AGENDA>();
            Session["ListaAgenda"] = null;
            return RedirectToAction("MontarTelaAgenda");
        }

        [HttpGet]
        public ActionResult VerAnexoAgenda(Int32 id)
        {
            // Prepara view
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            AGENDA_ANEXO item = baseApp.GetAnexoById(id);
            return View(item);
        }

        public ActionResult VoltarAnexoAgenda()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("EditarAgenda", new { id = (Int32)Session["IdVolta"] });
        }

        public FileResult DownloadAgenda(Int32 id)
        {
            AGENDA_ANEXO item = baseApp.GetAnexoById(id);
            String arquivo = item.AGAN_AQ_ARQUIVO;
            Int32 pos = arquivo.LastIndexOf("/") + 1;
            String nomeDownload = arquivo.Substring(pos);
            String contentType = string.Empty;
            if (arquivo.Contains(".pdf"))
            {
                contentType = "application/pdf";
            }
            else if (arquivo.Contains(".jpg"))
            {
                contentType = "image/jpg";
            }
            else if (arquivo.Contains(".png"))
            {
                contentType = "image/png";
            }
            return File(arquivo, contentType, nomeDownload);
        }

        [HttpPost]
        public JsonResult UploadFileAgenda_Inclusao(IEnumerable<HttpPostedFileBase> files, Int32 perfil)
        {
            var count = 0;

            if (perfil == 0)
            {
                count++;
            }

            foreach (var file in files)
            {
                if (count == 0)
                {
                    //UploadFotoAgenda(file);

                    //count++;
                }
                else
                {
                    UploadFileAgenda(file);
                }
            }

            return Json("1"); // VoltarAnexoAgenda
        }

        [HttpPost]
        public ActionResult UploadFileAgenda(HttpPostedFileBase file)
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if (file == null)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0019", CultureInfo.CurrentCulture));
                return RedirectToAction("VoltarAnexoAgenda");
            }

            AGENDA item = baseApp.GetById((Int32)Session["IdVolta"]);
            USUARIO usu = (USUARIO)Session["UserCredentials"];
            var fileName = Path.GetFileName(file.FileName);
            Int32 idAss = (Int32)Session["IdAssinante"];

            if (fileName.Length > 100)
            {
                ModelState.AddModelError("", ERPJuris_Resource.ResourceManager.GetString("M0024", CultureInfo.CurrentCulture));
                return RedirectToAction("VoltarAnexoAgenda");
            }

            String caminho = "/Imagens/Agenda/" + idAss.ToString() + "/" + item.AGEN_CD_ID.ToString() + "/Anexos/";
            String path = Path.Combine(Server.MapPath(caminho), fileName);
            file.SaveAs(path);

            //Recupera tipo de arquivo
            extensao = Path.GetExtension(fileName);
            String a = extensao;

            // Gravar registro
            AGENDA_ANEXO foto = new AGENDA_ANEXO();
            foto.AGAN_AQ_ARQUIVO = "~" + caminho + fileName;
            foto.AGAN_DT_ANEXO = DateTime.Today;
            foto.AGAN_IN_ATIVO = 1;
            Int32 tipo = 4;
            if (extensao.ToUpper() == ".JPG" || extensao.ToUpper() == ".GIF" || extensao.ToUpper() == ".PNG" || extensao.ToUpper() == ".JPEG")
            {
                tipo = 1;
            }
            if (extensao.ToUpper() == ".MP4" || extensao.ToUpper() == ".AVI" || extensao.ToUpper() == ".MPEG")
            {
                tipo = 2;
            }
            if (extensao.ToUpper() == ".PDF")
            {
                tipo = 3;
            }
            foto.AGAN_IN_TIPO = tipo;
            foto.AGAN_NM_TITULO = fileName;
            foto.AGEN_CD_ID = item.AGEN_CD_ID;

            item.AGENDA_ANEXO.Add(foto);
            objetoAntes = item;
            Int32 volta = baseApp.ValidateEdit(item, objetoAntes, usu);
            return RedirectToAction("VoltarAnexoAgenda");
        }


        [HttpGet]
        public ActionResult VerTimelineAgenda()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            ViewBag.Title = "Agenda";
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Tipos = new SelectList(caApp.GetAllItens(idAss), "CAAG_CD_ID", "CAAG_NM_NOME");
            List<AGENDA> lista = (List<AGENDA>)Session["ListaAgenda"];
            var listaAgenda = lista.Where(x => x.AGEN_DT_DATA.Date == DateTime.Now.Date || x.AGEN_DT_DATA.Date == DateTime.Now.AddDays(1).Date).ToList();

            // Carrega listas
            USUARIO usuario = (USUARIO)Session["UserCredentials"];
            listaMaster = baseApp.GetByUser(usuario.USUA_CD_ID, idAss);
            Session["ListaAgenda"] = listaMaster;
            ViewBag.Listas = listaAgenda;
            ViewBag.Agenda = listaAgenda.Count;

            objeto = new AGENDA();
            objeto.AGEN_DT_DATA = DateTime.Today.Date;
            Session["VoltaAgenda"] = 2;
            return View(objeto);
        }

        public ActionResult GerarRelatorioLista()
        {
            // Prepara geração
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            String data = DateTime.Today.Date.ToShortDateString();
            data = data.Substring(0, 2) + data.Substring(3, 2) + data.Substring(6, 4);
            String nomeRel = "AgendaLista" + "_" + data + ".pdf";
            List<AGENDA> lista = (List<AGENDA>)Session["ListaAgenda"];
            AGENDA filtro = (AGENDA)Session["FiltroAgenda"];
            Font meuFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Font meuFont1 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Font meuFont2 = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Cria documento
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            // Linha horizontal
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            // Cabeçalho
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.SpacingBefore = 1f;
            table.SpacingAfter = 1f;

            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            Image image = Image.GetInstance(Server.MapPath("~/Images/5.png"));
            image.ScaleAbsolute(50, 50);
            cell.AddElement(image);
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Agenda - Listagem", meuFont2))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            cell.Border = 0;
            cell.Colspan = 4;
            table.AddCell(cell);
            pdfDoc.Add(table);

            // Linha Horizontal
            Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line1);
            line1 = new Paragraph("  ");
            pdfDoc.Add(line1);

            // Grid
            table = new PdfPTable(new float[] { 60f, 60f, 90f, 200f, 120f, 70f, 50f});
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 1f;
            table.SpacingAfter = 1f;

            cell = new PdfPCell(new Paragraph("Itens de Agenda selecionados pelos parametros de filtro abaixo", meuFont1))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.Colspan = 6;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Data", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Hora", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Categoria", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Título", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Contato", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Status", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Anexos", meuFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);

            foreach (AGENDA item in lista)
            {
                cell = new PdfPCell(new Paragraph(item.AGEN_DT_DATA.ToShortDateString(), meuFont))
                {
                    VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Paragraph(item.AGEN_HR_HORA.ToString(), meuFont))
                {
                    VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Paragraph(item.CATEGORIA_AGENDA.CAAG_NM_NOME, meuFont))
                {
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Paragraph(item.AGEN_NM_TITULO, meuFont))
                {
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                if (item.USUARIO1 != null)
                {
                    cell = new PdfPCell(new Paragraph(item.USUARIO1.USUA_NM_NOME, meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Paragraph("-", meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(cell);

                }
                if (item.AGEN_IN_STATUS == 1)
                {
                    cell = new PdfPCell(new Paragraph("Ativo", meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(cell);
                }
                else if (item.AGEN_IN_STATUS == 2)
                {
                    cell = new PdfPCell(new Paragraph("Suspenso", meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Paragraph("Encerrado", meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(cell);
                }
                if (item.AGENDA_ANEXO.Count > 0)
                {
                    cell = new PdfPCell(new Paragraph(item.AGENDA_ANEXO.Count.ToString(), meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Paragraph("0", meuFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(cell);
                }
            }
            pdfDoc.Add(table);

            // Linha Horizontal
            Paragraph line2 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line2);

            // Rodapé
            Chunk chunk1 = new Chunk("Parâmetros de filtro: ", FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK));
            pdfDoc.Add(chunk1);

            String parametros = String.Empty;
            Int32 ja = 0;
            if (filtro != null)
            {
                if (filtro.CAAG_CD_ID > 0)
                {
                    parametros += "Categoria: " + filtro.CAAG_CD_ID;
                    ja = 1;
                }
                if (filtro.AGEN_DT_DATA != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Data: " + filtro.AGEN_DT_DATA.ToShortDateString();
                        ja = 1;
                    }
                    else
                    {
                        parametros +=  " e Data: " + filtro.AGEN_DT_DATA.ToShortDateString();
                    }
                }
                if (filtro.AGEN_NM_TITULO != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Título: " + filtro.AGEN_NM_TITULO;
                        ja = 1;
                    }
                    else
                    {
                        parametros += " e Título: " + filtro.AGEN_NM_TITULO;
                    }
                }
                if (filtro.AGEN_DS_DESCRICAO != null)
                {
                    if (ja == 0)
                    {
                        parametros += "Descrição: " + filtro.AGEN_DS_DESCRICAO;
                        ja = 1;
                    }
                    else
                    {
                        parametros += " e Descrição: " + filtro.AGEN_DS_DESCRICAO;
                    }
                }
                if (ja == 0)
                {
                    parametros = "Nenhum filtro definido.";
                }
            }
            else
            {
                parametros = "Nenhum filtro definido.";
            }
            Chunk chunk = new Chunk(parametros, FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK));
            pdfDoc.Add(chunk);

            // Linha Horizontal
            Paragraph line3 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line3);

            // Finaliza
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + nomeRel);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

            return RedirectToAction("MontarTelaAgenda");
        }
    }
}