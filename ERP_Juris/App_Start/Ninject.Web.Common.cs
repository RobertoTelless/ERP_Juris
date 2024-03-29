using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Web.Common;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using ApplicationServices.Interfaces;
using ModelServices.Interfaces.EntitiesServices;
using ModelServices.Interfaces.Repositories;
using ApplicationServices.Services;
using ModelServices.EntitiesServices;
using DataServices.Repositories;
using Ninject.Web.Common.WebHost;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Presentation.Start.NinjectWebCommons), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Presentation.Start.NinjectWebCommons), "Stop")]

namespace Presentation.Start
{
    public class NinjectWebCommons
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(typeof(IAppServiceBase<>)).To(typeof(AppServiceBase<>));
            kernel.Bind<IUsuarioAppService>().To<UsuarioAppService>();
            kernel.Bind<ILogAppService>().To<LogAppService>();
            kernel.Bind<IPerfilAppService>().To<PerfilAppService>();
            kernel.Bind<IConfiguracaoAppService>().To<ConfiguracaoAppService>();
            kernel.Bind<INoticiaAppService>().To<NoticiaAppService>();
            kernel.Bind<INotificacaoAppService>().To<NotificacaoAppService>();
            kernel.Bind<ITipoPessoaAppService>().To<TipoPessoaAppService>();
            kernel.Bind<ITemplateAppService>().To<TemplateAppService>();
            kernel.Bind<ITarefaAppService>().To<TarefaAppService>();
            kernel.Bind<IAgendaAppService>().To<AgendaAppService>();
            kernel.Bind<ICargoAppService>().To<CargoAppService>();
            kernel.Bind<ICategoriaFornecedorAppService>().To<CategoriaFornecedorAppService>();
            kernel.Bind<IFornecedorAppService>().To<FornecedorAppService>();
            kernel.Bind<IFornecedorCnpjAppService>().To<FornecedorCnpjAppService>();
            kernel.Bind<IEmpresaAppService>().To<EmpresaAppService>();
            kernel.Bind<ITipoAcaoAppService>().To<TipoAcaoAppService>();
            kernel.Bind<ITipoJusticaAppService>().To<TipoJusticaAppService>();
            kernel.Bind<IRegiaoJusticaAppService>().To<RegiaoJusticaAppService>();
            kernel.Bind<ISecaoAppService>().To<SecaoAppService>();
            kernel.Bind<ISubsecaoAppService>().To<SubsecaoAppService>();
            kernel.Bind<ICidadeComarcaAppService>().To<CidadeComarcaAppService>();
            kernel.Bind<IForoAppService>().To<ForoAppService>();
            kernel.Bind<IVaraAppService>().To<VaraAppService>();
            kernel.Bind<IBancoAppService>().To<BancoAppService>();
            kernel.Bind<IContaBancariaAppService>().To<ContaBancariaAppService>();
            kernel.Bind<ICentroCustoAppService>().To<CentroCustoAppService>();
            kernel.Bind<IGrupoAppService>().To<GrupoAppService>();
            kernel.Bind<ISubgrupoAppService>().To<SubgrupoAppService>();
            kernel.Bind<ICategoriaAgendaAppService>().To<CategoriaAgendaAppService>();

            kernel.Bind(typeof(IServiceBase<>)).To(typeof(ServiceBase<>));
            kernel.Bind<IUsuarioService>().To<UsuarioService>();
            kernel.Bind<ILogService>().To<LogService>();
            kernel.Bind<IPerfilService>().To<PerfilService>();
            kernel.Bind<IConfiguracaoService>().To<ConfiguracaoService>();
            kernel.Bind<INotificacaoService>().To<NotificacaoService>();
            kernel.Bind<INoticiaService>().To<NoticiaService>();
            kernel.Bind<ITipoPessoaService>().To<TipoPessoaService>();
            kernel.Bind<ITemplateService>().To<TemplateService>();
            kernel.Bind<ITarefaService>().To<TarefaService>();
            kernel.Bind<IAgendaService>().To<AgendaService>();
            kernel.Bind<ICargoService>().To<CargoService>();
            kernel.Bind<ICategoriaFornecedorService>().To<CategoriaFornecedorService>();
            kernel.Bind<IFornecedorService>().To<FornecedorService>();
            kernel.Bind<IFornecedorCnpjService>().To<FornecedorCnpjService>();
            kernel.Bind<IEmpresaService>().To<EmpresaService>();
            kernel.Bind<ITipoAcaoService>().To<TipoAcaoService>();
            kernel.Bind<ITipoJusticaService>().To<TipoJusticaService>();
            kernel.Bind<IRegiaoJusticaService>().To<RegiaoJusticaService>();
            kernel.Bind<ISecaoService>().To<SecaoService>();
            kernel.Bind<ISubsecaoService>().To<SubSecaoService>();
            kernel.Bind<ICidadeComarcaService>().To<CidadeComarcaService>();
            kernel.Bind<IForoService>().To<ForoService>();
            kernel.Bind<IVaraService>().To<VaraService>();
            kernel.Bind<IBancoService>().To<BancoService>();
            kernel.Bind<IContaBancariaService>().To<ContaBancariaService>();
            kernel.Bind<ICentroCustoService>().To<CentroCustoService>();
            kernel.Bind<IGrupoService>().To<GrupoService>();
            kernel.Bind<ISubgrupoService>().To<SubgrupoService>();
            kernel.Bind<ITipoContaService>().To<TipoContaService>();
            kernel.Bind<ICategoriaAgendaService>().To<CategoriaAgendaService>();

            kernel.Bind(typeof(IRepositoryBase<>)).To(typeof(RepositoryBase<>));
            kernel.Bind<IConfiguracaoRepository>().To<ConfiguracaoRepository>();
            kernel.Bind<IUsuarioRepository>().To<UsuarioRepository>();
            kernel.Bind<ILogRepository>().To<LogRepository>();
            kernel.Bind<IPerfilRepository>().To<PerfilRepository>();
            kernel.Bind<ITemplateRepository>().To<TemplateRepository>();
            kernel.Bind<ITipoPessoaRepository>().To<TipoPessoaRepository>();
            kernel.Bind<ICategoriaNotificacaoRepository>().To<CategoriaNotificacaoRepository>();
            kernel.Bind<INotificacaoRepository>().To<NotificacaoRepository>();
            kernel.Bind<INoticiaRepository>().To<NoticiaRepository>();
            kernel.Bind<INoticiaComentarioRepository>().To<NoticiaComentarioRepository>();
            kernel.Bind<INotificacaoAnexoRepository>().To<NotificacaoAnexoRepository>();
            kernel.Bind<ITarefaRepository>().To<TarefaRepository>();
            kernel.Bind<ITipoTarefaRepository>().To<TipoTarefaRepository>();
            kernel.Bind<ITarefaAnexoRepository>().To<TarefaAnexoRepository>();
            kernel.Bind<ITarefaNotificacaoRepository>().To<TarefaNotificacaoRepository>();
            kernel.Bind<IUsuarioAnexoRepository>().To<UsuarioAnexoRepository>();
            kernel.Bind<IUFRepository>().To<UFRepository>();
            kernel.Bind<IAgendaRepository>().To<AgendaRepository>();
            kernel.Bind<IAgendaAnexoRepository>().To<AgendaAnexoRepository>();
            kernel.Bind<ICategoriaAgendaRepository>().To<CategoriaAgendaRepository>();
            kernel.Bind<ICategoriaUsuarioRepository>().To<CategoriaUsuarioRepository>();
            kernel.Bind<ICargoRepository>().To<CargoRepository>();
            kernel.Bind<ICategoriaFornecedorRepository>().To<CategoriaFornecedorRepository>();
            kernel.Bind<IFornecedorRepository>().To<FornecedorRepository>();
            kernel.Bind<IFornecedorAnexoRepository>().To<FornecedorAnexoRepository>();
            kernel.Bind<IFornecedorContatoRepository>().To<FornecedorContatoRepository>();
            kernel.Bind<IFornecedorCnpjRepository>().To<FornecedorCnpjRepository>();
            kernel.Bind<IFornecedorMensagemRepository>().To<FornecedorMensagemRepository>();
            kernel.Bind<ITipoMensagemRepository>().To<TipoMensagemRepository>();
            kernel.Bind<IEmpresaRepository>().To<EmpresaRepository>();
            kernel.Bind<ITipoAcaoRepository>().To<TipoAcaoRepository>();
            kernel.Bind<ITipoJusticaRepository>().To<TipoJusticaRepository>();
            kernel.Bind<IRegiaoJusticaRepository>().To<RegiaoJusticaRepository>();
            kernel.Bind<ISecaoRepository>().To<SecaoRepository>();
            kernel.Bind<ISubsecaoRepository>().To<SubsecaoRepository>();
            kernel.Bind<ICidadeComarcaRepository>().To<CidadeComarcaRepository>();
            kernel.Bind<IForoRepository>().To<ForoRepository>();
            kernel.Bind<IVaraRepository>().To<VaraRepository>();
            kernel.Bind<IBancoRepository>().To<BancoRepository>();
            kernel.Bind<IContaBancariaRepository>().To<ContaBancariaRepository>();
            kernel.Bind<IContaBancariaContatoRepository>().To<ContaBancariaContatoRepository>();
            kernel.Bind<IContaBancariaLancamentoRepository>().To<ContaBancariaLancamentoRepository>();
            kernel.Bind<ICentroCustoRepository>().To<CentroCustoRepository>();
            kernel.Bind<IGrupoRepository>().To<GrupoRepository>();
            kernel.Bind<ISubgrupoRepository>().To<SubgrupoRepository>();
            kernel.Bind<ITipoContaRepository>().To<TipoContaRepository>();

        }
    }
}