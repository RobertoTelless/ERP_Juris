using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;
using ApplicationServices.Interfaces;
using ModelServices.Interfaces.EntitiesServices;
using CrossCutting;
using System.Text.RegularExpressions;
using System.Web;
using Itenso.TimePeriod;

namespace ApplicationServices.Services
{
    public class UsuarioAppService : AppServiceBase<USUARIO>, IUsuarioAppService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly INotificacaoService _notiService;

        public UsuarioAppService(IUsuarioService usuarioService, INotificacaoService notiService): base(usuarioService)
        {
            _usuarioService = usuarioService;
            _notiService = notiService;
        }

        public USUARIO GetByEmail(String email)
        {
            return _usuarioService.GetByEmail(email);
        }

        public USUARIO CheckExist(USUARIO ag, Int32 idAss)
        {
            USUARIO item = _usuarioService.CheckExist(ag, idAss);
            return item;
        }

        public List<CATEGORIA_USUARIO> GetAllTipos(Int32 idAss)
        {
            List<CATEGORIA_USUARIO> lista = _usuarioService.GetAllTipos(idAss);
            return lista;
        }

        public List<CARGO> GetAllCargos(Int32 idAss)
        {
            List<CARGO> lista = _usuarioService.GetAllCargos(idAss);
            return lista;
        }

        public USUARIO GetByLogin(String login)
        {
            return _usuarioService.GetByLogin(login);
        }

        public List<USUARIO> GetAllUsuariosAdm(Int32 idAss)
        {
            return _usuarioService.GetAllUsuariosAdm(idAss);
        }

        public List<USUARIO> GetAllADM(Int32 idAss)
        {
            return _usuarioService.GetAllADM(idAss);
        }

        public USUARIO GetItemById(Int32 id)
        {
            return _usuarioService.GetItemById(id);
        }

        public List<USUARIO> GetAllUsuarios(Int32 idAss)
        {
            return _usuarioService.GetAllUsuarios(idAss);
        }

        public List<USUARIO> GetAllItens(Int32 idAss)
        {
            return _usuarioService.GetAllItens(idAss);
        }

        public List<NOTIFICACAO> GetAllItensUser(Int32 id, Int32 idAss)
        {
            return _usuarioService.GetAllItensUser(id, idAss);
        }

        public List<NOTICIA> GetAllNoticias(Int32 idAss)
        {
            return _usuarioService.GetAllNoticias(idAss);
        }

        public List<UF> GetAllUF()
        {
            return _usuarioService.GetAllUF();
        }

        public List<NOTIFICACAO> GetNotificacaoNovas(Int32 id, Int32 idAss)
        {
            return _usuarioService.GetNotificacaoNovas(id, idAss);
        }

        public List<USUARIO> GetAllItensBloqueados(Int32 idAss)
        {
            return _usuarioService.GetAllItensBloqueados(idAss);
        }

        public List<USUARIO> GetAllItensAcessoHoje(Int32 idAss)
        {
            return _usuarioService.GetAllItensAcessoHoje(idAss);
        }

        public USUARIO_ANEXO GetAnexoById(Int32 id)
        {
            return _usuarioService.GetAnexoById(id);
        }

        public Int32 ValidateCreate(USUARIO usuario, USUARIO usuarioLogado)
        {
            try
            {
                // Verifica senhas
                if (usuario.USUA_NM_SENHA != usuario.USUA_NM_SENHA_CONFIRMA)
                {
                    return 1;
                }

                // Verifica Email
                if (!ValidarItensDiversos.IsValidEmail(usuario.USUA_NM_EMAIL))
                {
                    return 2;
                }

                // Verifica existencia prévia
                if (_usuarioService.GetByEmail(usuario.USUA_NM_EMAIL) != null)
                {
                    return 3;
                }
                if (_usuarioService.GetByLogin(usuario.USUA_NM_LOGIN) != null)
                {
                    return 4;
                }

                // Verifica admissão e demissão
                if (usuario.USUA_DT_ENTRADA != null)
                {
                    if (usuario.USUA_DT_ENTRADA.Value > DateTime.Today.Date)
                    {
                        return 5;
                    }
                }
                if (usuario.USUA_DT_SAIDA != null)
                {
                    if (usuario.USUA_DT_SAIDA.Value > DateTime.Today.Date)
                    {
                        return 6;
                    }
                    if (usuario.USUA_DS_MOTIVO_SAIDA == null)
                    {
                        return 7;
                    }
                }

                //Completa campos de usuários
                String senha = Cryptography.GenerateRandomPassword(6);
                usuario.USUA_NM_SENHA = senha;
                usuario.USUA_NM_LOGIN = usuario.USUA_NM_NOME.Substring(0, 4);
                //usuario.USUA_NM_SENHA = Cryptography.Encode(usuario.USUA_NM_SENHA);
                usuario.USUA_IN_BLOQUEADO = 0;
                usuario.USUA_IN_PROVISORIO = 0;
                usuario.USUA_IN_LOGIN_PROVISORIO = 0;
                usuario.USUA_NR_ACESSOS = 0;
                usuario.USUA_NR_FALHAS = 0;
                usuario.USUA_DT_ALTERACAO = null;
                usuario.USUA_DT_BLOQUEADO = null;
                usuario.USUA_DT_TROCA_SENHA = null;
                usuario.USUA_DT_ACESSO = DateTime.Now;
                usuario.USUA_DT_CADASTRO = DateTime.Today.Date;
                usuario.USUA_IN_ATIVO = 1;
                usuario.USUA_DT_ULTIMA_FALHA = DateTime.Now;
                usuario.ASSI_CD_ID = usuarioLogado.ASSI_CD_ID;
                usuario.USUA_DS_MOTIVO_SAIDA = null;
                usuario.USUA_IN_LOGADO = 0;
                usuario.USUA_NR_MATRICULA = null;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuarioLogado.USUA_CD_ID,
                    ASSI_CD_ID = usuarioLogado.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddUSUA",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<USUARIO>(usuario),
                    LOG_IN_ATIVO = 1
                };

                // Persiste
                Int32 volta = _usuarioService.CreateUser(usuario, log);

                // Gerar Notificação
                NOTIFICACAO noti = new NOTIFICACAO();
                noti.CANO_CD_ID = 1;
                noti.ASSI_CD_ID = usuario.ASSI_CD_ID;
                noti.NOTI_DT_EMISSAO = DateTime.Today;
                noti.NOTI_DT_VALIDADE = DateTime.Today.Date.AddDays(30);
                noti.NOTI_IN_VISTA = 0;
                noti.NOTI_NM_TITULO = "Criação de Usuário";
                noti.NOTI_IN_ATIVO = 1;
                noti.NOTI_IN_NIVEL = 1;
                noti.NOTI_TX_TEXTO = "ATENÇÃO: Usuário" + usuario.USUA_NM_NOME + " criado em " + DateTime.Today.Date.ToLongDateString() + ". Perfil: " + usuario.PERFIL.PERF_NM_NOME + ". Login: " + usuario.USUA_NM_LOGIN + ". Senha: " + usuario.USUA_NM_SENHA + ". Essa senha é provisória e poderá ser usada apenas uma vez, devendo ser alterada após o primeiro login.";
                noti.USUA_CD_ID = usuario.USUA_CD_ID;
                noti.NOTI_IN_STATUS = 1;
                Int32 volta1 = _notiService.Create(noti);

                // Recupera template e-mail
                TEMPLATE template = _usuarioService.GetTemplate("NEWUSR");
                String header = template.TEMP_TX_CABECALHO;
                String body = template.TEMP_TX_CORPO;
                String data = template.TEMP_TX_DADOS;

                // Prepara dados do e-mail  
                data = data.Replace("{Nome}", usuario.USUA_NM_NOME);
                data = data.Replace("{Cargo}", usuario.CARGO.CARG_NM_NOME);
                data = data.Replace("{Perfil}", usuario.PERFIL.PERF_NM_NOME);
                data = data.Replace("{Data}", usuario.USUA_DT_CADASTRO.Value.ToLongDateString());
                data = data.Replace("{Login}", usuario.USUA_NM_LOGIN);
                data = data.Replace("{Senha}", usuario.USUA_NM_SENHA);

                // Concatena
                String emailBody = header + body + data;

                // Prepara e-mail e enviar
                CONFIGURACAO conf = _usuarioService.CarregaConfiguracao(usuario.ASSI_CD_ID);
                Email mensagem = new Email();
                mensagem.ASSUNTO = "Inclusão de Usuário";
                mensagem.CORPO = emailBody;
                mensagem.DEFAULT_CREDENTIALS = false;
                mensagem.EMAIL_DESTINO = usuario.USUA_NM_EMAIL;
                mensagem.EMAIL_EMISSOR = conf.CONF_NM_EMAIL_EMISSOO;
                mensagem.ENABLE_SSL = true;
                mensagem.NOME_EMISSOR = "Sistema";
                mensagem.PORTA = conf.CONF_NM_PORTA_SMTP;
                mensagem.PRIORIDADE = System.Net.Mail.MailPriority.High;
                mensagem.SENHA_EMISSOR = conf.CONF_NM_SENHA_EMISSOR;
                mensagem.SMTP = conf.CONF_NM_HOST_SMTP;

                // Envia e-mail
                Int32 voltaMail = CommunicationPackage.SendEmail(mensagem);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 CreateNotificacao(NOTIFICACAO noti, USUARIO usuarioLogado)
        {
            try
            {
                //Completa campos
                USUARIO adm = _usuarioService.GetAdministrador(usuarioLogado.ASSI_CD_ID);
                noti.ASSI_CD_ID = usuarioLogado.ASSI_CD_ID;
                noti.CANO_CD_ID = 1;
                noti.NOTI_DT_EMISSAO = DateTime.Today.Date;
                noti.NOTI_DT_VALIDADE = DateTime.Today.Date.AddDays(30);
                noti.NOTI_DT_VISTA = null;
                noti.NOTI_IN_ATIVO = 1;
                noti.NOTI_IN_STATUS = 1;
                noti.NOTI_IN_NIVEL = 1;
                noti.NOTI_IN_VISTA = 0;
                noti.NOTI_NM_TITULO = "Solicição de Alteração de Cadastro";
                noti.USUA_CD_ID = adm.USUA_CD_ID;

                // Gera Notificação
                Int32 volta = _notiService.Create(noti);

                //// Recupera template e-mail
                //String header = _usuarioService.GetTemplate("SOLCAD").TEMP_TX_CABECALHO;
                //String body = _usuarioService.GetTemplate("SOLCAD").TEMP_TX_CORPO;
                //String data = _usuarioService.GetTemplate("SOLCAD").TEMP_TX_DADOS;

                //// Prepara dados do e-mail  
                //header = header.Replace("{Nome}", adm.USUA_NM_NOME);
                //data = data.Replace("{Data}", DateTime.Today.Date.ToLongDateString());
                //data = data.Replace("{Usuario}", usuarioLogado.USUA_NM_NOME);
                //data = data.Replace("{Solicitacao}", noti.NOTI_TX_TEXTO);

                //// Concatena
                //String emailBody = header + body + data;

                //// Prepara e-mail e enviar
                //CONFIGURACAO conf = _usuarioService.CarregaConfiguracao(usuarioLogado.ASSI_CD_ID);
                //Email mensagem = new Email();
                //mensagem.ASSUNTO = "Solicitacao de Alteracao Cadastral";
                //mensagem.CORPO = emailBody;
                //mensagem.DEFAULT_CREDENTIALS = false;
                //mensagem.EMAIL_DESTINO = adm.USUA_NM_EMAIL;
                //mensagem.EMAIL_EMISSOR = conf.CONF_NM_EMAIL_EMISSOO;
                //mensagem.ENABLE_SSL = true;
                //mensagem.NOME_EMISSOR = "Sistema";
                //mensagem.PORTA = conf.CONF_NM_PORTA_SMTP;
                //mensagem.PRIORIDADE = System.Net.Mail.MailPriority.High;
                //mensagem.SENHA_EMISSOR = conf.CONF_NM_SENHA_EMISSOR;
                //mensagem.SMTP = conf.CONF_NM_HOST_SMTP;

                //// Envia e-mail
                //Int32 voltaMail = CommunicationPackage.SendEmail(mensagem);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(USUARIO usuario, USUARIO usuarioAntes, USUARIO usuarioLogado)
        {
            try
            {
                // Verifica Email
                if (!ValidarItensDiversos.IsValidEmail(usuario.USUA_NM_EMAIL))
                {
                    return 1;
                }

                // Verifica existencia prévia
                USUARIO usu = _usuarioService.GetByEmail(usuario.USUA_NM_EMAIL);
                if (usu != null)
                {
                    if (usu.USUA_CD_ID != usuario.USUA_CD_ID)
                    {
                        return 2;
                    }
                }
                usu = _usuarioService.GetByLogin(usuario.USUA_NM_LOGIN);
                if (usu != null)
                {
                    if (usu.USUA_CD_ID != usuario.USUA_CD_ID)
                    {
                        return 3;
                    }
                }

                // Verifica admissão e demissão
                if (usuario.USUA_DT_ENTRADA != null)
                {
                    if (usuario.USUA_DT_ENTRADA.Value > DateTime.Today.Date)
                    {
                        return 4;
                    }
                }
                if (usuario.USUA_DT_SAIDA != null)
                {
                    if (usuario.USUA_DT_SAIDA.Value > DateTime.Today.Date)
                    {
                        return 5;
                    }
                    if (usuario.USUA_DS_MOTIVO_SAIDA == null)
                    {
                        return 6;
                    }
                }

                //Acerta campos de usuários
                usuario.USUA_DT_ALTERACAO = DateTime.Now;
                usuario.USUA_IN_ATIVO = 1;
                if (usuario.USUA_DT_SAIDA != null)
                {
                    usuario.USUA_IN_ATIVO = 0;
                    usuario.USUA_IN_BLOQUEADO = 1;
                }
                else
                {
                    usuario.USUA_IN_ATIVO = 1;
                    usuario.USUA_IN_BLOQUEADO = 0;
                }

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuarioLogado.USUA_CD_ID,
                    ASSI_CD_ID = usuarioLogado.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "EditUSUA",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<USUARIO>(usuario),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<USUARIO>(usuarioAntes),
                    LOG_IN_ATIVO = 1
                };


                // Persiste
                Int32 volta = _usuarioService.EditUser(usuario);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(USUARIO usuario, USUARIO usuarioLogado)
        {
            try
            {
                // Verifica Email
                if (!ValidarItensDiversos.IsValidEmail(usuario.USUA_NM_EMAIL))
                {
                    return 1;
                }

                // Verifica existencia prévia
                USUARIO usu = _usuarioService.GetByEmail(usuario.USUA_NM_EMAIL);
                if (usu != null)
                {
                    if (usu.USUA_CD_ID != usuario.USUA_CD_ID)
                    {
                        return 2;
                    }
                }
                usu = _usuarioService.GetByLogin(usuario.USUA_NM_LOGIN);
                if (usu != null)
                {
                    if (usu.USUA_CD_ID != usuario.USUA_CD_ID)
                    {
                        return 3;
                    }
                }

                // Verifica admissão e demissão
                if (usuario.USUA_DT_ENTRADA != null)
                {
                    if (usuario.USUA_DT_ENTRADA.Value > DateTime.Today.Date)
                    {
                        return 4;
                    }
                }
                if (usuario.USUA_DT_SAIDA != null)
                {
                    if (usuario.USUA_DT_SAIDA.Value > DateTime.Today.Date)
                    {
                        return 5;
                    }
                    if (usuario.USUA_DS_MOTIVO_SAIDA == null)
                    {
                        return 6;
                    }
                }

                //Acerta campos de usuários
                usuario.USUA_DT_ALTERACAO = DateTime.Now;
                usuario.USUA_IN_ATIVO = 1;

                // Persiste
                Int32 volta = _usuarioService.EditUser(usuario);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Int32 ValidateDelete(USUARIO usuario, USUARIO usuarioLogado)
        {
            try
            {
                // Verifica integridade


                // Acerta campos de usuários
                usuario.USUA_DT_ALTERACAO = DateTime.Now;
                usuario.USUA_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuarioLogado.USUA_CD_ID,
                    ASSI_CD_ID = usuarioLogado.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "DelUSUA",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<USUARIO>(usuario),
                    LOG_IN_ATIVO = 1
                };

                // Persiste
                return _usuarioService.EditUser(usuario);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(USUARIO usuario, USUARIO usuarioLogado)
        {
            try
            {
                // Verifica integridade

                // Acerta campos de usuários
                usuario.USUA_DT_ALTERACAO = DateTime.Now;
                usuario.USUA_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuarioLogado.USUA_CD_ID,
                    ASSI_CD_ID = usuarioLogado.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "ReatUSUA",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<USUARIO>(usuario),
                    LOG_IN_ATIVO = 1
                };

                // Persiste
                return _usuarioService.EditUser(usuario);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateBloqueio(USUARIO usuario, USUARIO usuarioLogado)
        {
            try
            {
                //Acerta campos de usuários
                usuario.USUA_DT_BLOQUEADO = DateTime.Today;
                usuario.USUA_IN_BLOQUEADO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuarioLogado.USUA_CD_ID,
                    ASSI_CD_ID = usuarioLogado.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "BlqUSUA",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<USUARIO>(usuario),
                    LOG_IN_ATIVO = 1
                };

                // Persiste
                return _usuarioService.EditUser(usuario, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateDesbloqueio(USUARIO usuario, USUARIO usuarioLogado)
        {
            try
            {
                //Acerta campos de usuários
                usuario.USUA_DT_BLOQUEADO = DateTime.Now;
                usuario.USUA_IN_BLOQUEADO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    ASSI_CD_ID = usuarioLogado.ASSI_CD_ID,
                    USUA_CD_ID = usuarioLogado.USUA_CD_ID,
                    LOG_NM_OPERACAO = "DbqUSUA",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<USUARIO>(usuario),
                    LOG_IN_ATIVO = 1
                };

                // Persiste
                return _usuarioService.EditUser(usuario, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateLogin(String login, String senha, out USUARIO usuario)
        {
            try
            {
                usuario = new USUARIO();
                // Checa Preenchimentos 
                if (String.IsNullOrEmpty(login))
                {
                    return 10;
                }
                if (String.IsNullOrEmpty(senha))
                {
                    return 9;
                }

                // Checa login
                usuario = _usuarioService.GetByLogin(login);
                if (usuario == null)
                {
                    usuario = new USUARIO();
                    return 2;
                }

                // Verifica se está ativo
                if (usuario.USUA_IN_ATIVO != 1)
                {
                    return 3;
                }

                // Verifica se está bloqueado
                if (usuario.USUA_IN_BLOQUEADO == 1)
                {
                    return 4;
                }

                // verifica senha proviória
                if (usuario.USUA_IN_PROVISORIO == 1)
                {
                    if (usuario.USUA_IN_LOGIN_PROVISORIO == 0)
                    {
                        usuario.USUA_IN_LOGIN_PROVISORIO = 1;
                    }
                    else
                    {
                        return 5;
                    }
                }

                // Verifica credenciais
                Boolean retorno = _usuarioService.VerificarCredenciais(senha, usuario);
                if (!retorno)
                {
                    if (usuario.USUA_NR_FALHAS <= _usuarioService.CarregaConfiguracao(usuario.ASSI_CD_ID).CONF_NR_FALHAS_DIA)
                    {
                        if (usuario.USUA_DT_ULTIMA_FALHA != null)
                        {
                            if (usuario.USUA_DT_ULTIMA_FALHA.Value.Date != DateTime.Now.Date)
                            {
                                usuario.USUA_DT_ULTIMA_FALHA = DateTime.Now;
                                usuario.USUA_NR_FALHAS = 1;
                            }
                            else
                            {
                                usuario.USUA_NR_FALHAS++;
                            }
                        }
                        else
                        {
                            usuario.USUA_DT_ULTIMA_FALHA = DateTime.Now;
                            usuario.USUA_NR_FALHAS = 1;
                        }

                    }
                    else if (usuario.USUA_NR_FALHAS > _usuarioService.CarregaConfiguracao(usuario.ASSI_CD_ID).CONF_NR_FALHAS_DIA)
                    {
                        usuario.USUA_DT_BLOQUEADO = DateTime.Now;
                        usuario.USUA_IN_BLOQUEADO = 1;
                        usuario.USUA_NR_FALHAS = 0;
                        usuario.USUA_DT_ULTIMA_FALHA = DateTime.Now;
                        Int32 voltaBloqueio = _usuarioService.EditUser(usuario);
                        return 6;
                    }
                    Int32 volta = _usuarioService.EditUser(usuario);
                    return 7;
                }

                // Atualiza acessos e data do acesso
                usuario.USUA_NR_ACESSOS = ++usuario.USUA_NR_ACESSOS;
                usuario.USUA_DT_ACESSO = DateTime.Now;
                usuario.USUA_IN_LOGADO = 1;
                Int32 voltaAcesso = _usuarioService.EditUser(usuario);
                return 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateChangePassword(USUARIO usuario)
        {
            try
            {
                // Checa preenchimento
                if (String.IsNullOrEmpty(usuario.USUA_NM_NOVA_SENHA))
                {
                    return 3;
                }
                if (String.IsNullOrEmpty(usuario.USUA_NM_SENHA_CONFIRMA))
                {
                    return 4;
                }

                // Verifica se senha igual a anterior
                if (usuario.USUA_NM_SENHA == usuario.USUA_NM_NOVA_SENHA)
                {
                    return 1;
                }

                // Verifica se senha foi confirmada
                if (usuario.USUA_NM_NOVA_SENHA != usuario.USUA_NM_SENHA_CONFIRMA)
                {
                    return 2;
                }

                //Completa e acerta campos 
                //usuario.USUA_NM_SENHA = Cryptography.Encode(usuario.USUA_NM_NOVA_SENHA);
                usuario.USUA_NM_SENHA = usuario.USUA_NM_NOVA_SENHA;
                usuario.USUA_DT_TROCA_SENHA = DateTime.Now.Date;
                usuario.USUA_IN_BLOQUEADO = 0;
                usuario.USUA_IN_PROVISORIO = 0;
                usuario.USUA_IN_LOGIN_PROVISORIO = 0;
                usuario.USUA_NR_ACESSOS = 0;
                usuario.USUA_NR_FALHAS = 0;
                usuario.USUA_DT_ALTERACAO = null;
                usuario.USUA_DT_BLOQUEADO = null;
                usuario.USUA_DT_TROCA_SENHA = null;
                usuario.USUA_DT_ACESSO = DateTime.Now;
                usuario.USUA_DT_CADASTRO = DateTime.Now;
                usuario.USUA_IN_ATIVO = 1;
                usuario.USUA_DT_ULTIMA_FALHA = null;
                usuario.ASSI_CD_ID = usuario.ASSI_CD_ID;

                // Gera Notificação
                NOTIFICACAO noti = new NOTIFICACAO();
                noti.CANO_CD_ID = 1;
                noti.ASSI_CD_ID = usuario.ASSI_CD_ID;
                noti.NOTI_DT_EMISSAO = DateTime.Today;
                noti.NOTI_DT_VALIDADE = DateTime.Today.Date.AddDays(30);
                noti.NOTI_IN_VISTA = 0;
                noti.NOTI_NM_TITULO = "Alteração de Senha";
                noti.NOTI_IN_ATIVO = 1;
                noti.NOTI_IN_NIVEL = 1;
                noti.NOTI_TX_TEXTO = "ATENÇÃO: A sua senha foi alterada em " + DateTime.Today.Date.ToLongDateString() + ".";
                noti.USUA_CD_ID = usuario.USUA_CD_ID;
                noti.NOTI_IN_STATUS = 1;
                Int32 volta1 = _notiService.Create(noti);


                //Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    LOG_NM_OPERACAO = "ChangePWD",
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<USUARIO>(usuario),
                };

                // Persiste
                return _usuarioService.EditUser(usuario);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 GenerateNewPassword(String email)
        {
            // Checa email
            if (!ValidarItensDiversos.IsValidEmail(email))
            {
                return 1;
            }
            USUARIO usuario = _usuarioService.RetriveUserByEmail(email);
            if (usuario == null)
            {
                return 2;
            }

            // Verifica se usuário está ativo
            if (usuario.USUA_IN_ATIVO == 0)
            {
                return 3;
            }

            // Verifica se usuário não está bloqueado
            if (usuario.USUA_IN_BLOQUEADO == 1)
            {
                return 4;
            }

            // Gera nova senha
            String senha = Cryptography.GenerateRandomPassword(6);

            // Atauliza objeto
            //usuario.USUA_NM_SENHA = Cryptography.Encode(senha);
            usuario.USUA_NM_SENHA = senha;
            usuario.USUA_IN_PROVISORIO = 1;
            usuario.USUA_DT_ALTERACAO = DateTime.Now;
            usuario.USUA_DT_TROCA_SENHA = DateTime.Now;
            usuario.USUA_IN_LOGIN_PROVISORIO = 0;

            // Monta log
            LOG log = new LOG();
            log.LOG_DT_DATA = DateTime.Now;
            log.LOG_NM_OPERACAO = "NewPWD";
            log.ASSI_CD_ID = usuario.ASSI_CD_ID;
            log.LOG_TX_REGISTRO = senha;
            log.LOG_IN_ATIVO = 1;

            // Gera Notificação
            NOTIFICACAO noti = new NOTIFICACAO();
            noti.CANO_CD_ID = 1;
            noti.ASSI_CD_ID = usuario.ASSI_CD_ID;
            noti.NOTI_DT_EMISSAO = DateTime.Today;
            noti.NOTI_DT_VALIDADE = DateTime.Today.Date.AddDays(30);
            noti.NOTI_IN_VISTA = 0;
            noti.NOTI_NM_TITULO = "Geração de Nova Senha";
            noti.NOTI_IN_ATIVO = 1;
            noti.NOTI_TX_TEXTO = "ATENÇÃO: Sua solicitação de nova senha foi atendida em " + DateTime.Today.Date.ToLongDateString() + ". Verifique no seu e-mail cadastrado no sistema.";
            noti.USUA_CD_ID = usuario.USUA_CD_ID;
            noti.NOTI_IN_STATUS = 1;
            Int32 volta1 = _notiService.Create(noti);

            // Recupera template e-mail
            String header = _usuarioService.GetTemplate("NEWPWD").TEMP_TX_CABECALHO;
            String body = _usuarioService.GetTemplate("NEWPWD").TEMP_TX_CORPO;
            String data = _usuarioService.GetTemplate("NEWPWD").TEMP_TX_DADOS;

            // Prepara dados do e-mail  
            header = header.Replace("{Nome}", usuario.USUA_NM_NOME);
            data = data.Replace("{Data}", usuario.USUA_DT_TROCA_SENHA.Value.ToLongDateString());
            data = data.Replace("{Senha}", usuario.USUA_NM_SENHA);

            // Concatena
            String emailBody = header + body + data;

            // Prepara e-mail e enviar
            CONFIGURACAO conf = _usuarioService.CarregaConfiguracao(usuario.ASSI_CD_ID);
            Email mensagem = new Email();
            mensagem.ASSUNTO = "Geração de Nova Senha";
            mensagem.CORPO = emailBody;
            mensagem.DEFAULT_CREDENTIALS = false;
            mensagem.EMAIL_DESTINO = usuario.USUA_NM_EMAIL;
            mensagem.EMAIL_EMISSOR = conf.CONF_NM_EMAIL_EMISSOO;
            mensagem.ENABLE_SSL = true;
            mensagem.NOME_EMISSOR = "Sistema";
            mensagem.PORTA = conf.CONF_NM_PORTA_SMTP;
            mensagem.PRIORIDADE = System.Net.Mail.MailPriority.High;
            mensagem.SENHA_EMISSOR = conf.CONF_NM_SENHA_EMISSOR;
            mensagem.SMTP = conf.CONF_NM_HOST_SMTP;

            // Envia e-mail
            Int32 voltaMail = CommunicationPackage.SendEmail(mensagem);

            // Atualiza usuario
            Int32 volta = _usuarioService.EditUser(usuario);

            // Retorna sucesso
            return 0;
        }

        public Int32 ExecuteFilter(Int32? causId, Int32? cargo, String nome, String login, String email, String cpf, Int32 idAss, out List<USUARIO> objeto)
        {
            try
            {
                objeto = new List<USUARIO>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _usuarioService.ExecuteFilter(causId, cargo, nome, login, email, cpf, idAss);
                if (objeto.Count == 0)
                {
                    volta = 1;
                }
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<PERFIL> GetAllPerfis()
        {
            List<PERFIL> lista = _usuarioService.GetAllPerfis();
            return lista;
        }

    }
}
