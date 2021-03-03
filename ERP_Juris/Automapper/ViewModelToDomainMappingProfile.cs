using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EntitiesServices.Model;
using ERP_Juris.ViewModels;

namespace MvcMapping.Mappers
{
    public class ViewModelToDomainMappingProfiles : Profile
    {
        public ViewModelToDomainMappingProfiles()
        {
            CreateMap<UsuarioViewModel, USUARIO>();
            CreateMap<UsuarioLoginViewModel, USUARIO>();
            CreateMap<LogViewModel, LOG>();
            CreateMap<ConfiguracaoViewModel, CONFIGURACAO>();
            CreateMap<NoticiaViewModel, NOTICIA>();
            CreateMap<NoticiaComentarioViewModel, NOTICIA_COMENTARIO>();
            CreateMap<NotificacaoViewModel, NOTIFICACAO>();
            CreateMap<TipoPessoaViewModel, TIPO_PESSOA>();
            CreateMap<TemplateViewModel, TEMPLATE>();
            CreateMap<TarefaViewModel, TAREFA>();
            CreateMap<CategoriaAgendaViewModel, CATEGORIA_AGENDA>();
            CreateMap<AgendaViewModel, AGENDA>();
            CreateMap<TarefaAcompanhamentoViewModel, TAREFA_ACOMPANHAMENTO>();
            CreateMap<CategoriaAgendaViewModel, CATEGORIA_AGENDA>();
            CreateMap<CategoriaNotificacaoViewModel, CATEGORIA_NOTIFICACAO>();
            CreateMap<CategoriaUsuarioViewModel, CATEGORIA_USUARIO>();
            CreateMap<TipoTarefaViewModel, TIPO_TAREFA>();
            CreateMap<CargoViewModel, CARGO>();
            CreateMap<FornecedorViewModel, FORNECEDOR>();
            CreateMap<FornecedorContatoViewModel, FORNECEDOR_CONTATO>();
            CreateMap<FornecedorComentarioViewModel, FORNECEDOR_COMENTARIO>();
            CreateMap<FornecedorMensagemViewModel, FORNECEDOR_MENSAGEM>();
            CreateMap<EmpresaViewModel, EMPRESA>();
            CreateMap<TipoJusticaViewModel, TIPO_JUSTICA>();
            CreateMap<RegiaoJusticaViewModel, REGIAO_JUSTICA>();
            CreateMap<SecaoViewModel, SECAO>();
            CreateMap<SubsecaoViewModel, SUBSECAO>();

        }
    }
}