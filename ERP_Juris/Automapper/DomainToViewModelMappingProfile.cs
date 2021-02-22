using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EntitiesServices.Model;
using ERP_Juris.ViewModels;

namespace MvcMapping.Mappers
{
    public class DomainToViewModelMappingProfiles : Profile
    {
        public DomainToViewModelMappingProfiles()
        {
            CreateMap<USUARIO, UsuarioViewModel>();
            CreateMap<USUARIO, UsuarioLoginViewModel>();
            CreateMap<LOG, LogViewModel>();
            CreateMap<CONFIGURACAO, ConfiguracaoViewModel>();
            CreateMap<NOTICIA, NoticiaViewModel>();
            CreateMap<NOTICIA_COMENTARIO, NoticiaComentarioViewModel>();
            CreateMap<NOTIFICACAO, NotificacaoViewModel>();
            CreateMap<TIPO_PESSOA, TipoPessoaViewModel>();
            CreateMap<TEMPLATE, TemplateViewModel>();
            CreateMap<TAREFA, TarefaViewModel>();
            CreateMap<CATEGORIA_AGENDA, CategoriaAgendaViewModel>();
            CreateMap<AGENDA, AgendaViewModel>();
            CreateMap<TAREFA_ACOMPANHAMENTO, TarefaAcompanhamentoViewModel>();
            CreateMap<CATEGORIA_AGENDA, CategoriaAgendaViewModel>();
            CreateMap<CATEGORIA_NOTIFICACAO, CategoriaNotificacaoViewModel>();
            CreateMap<CATEGORIA_USUARIO, CategoriaUsuarioViewModel>();
            CreateMap<TIPO_TAREFA, TipoTarefaViewModel>();
            CreateMap<CARGO, CargoViewModel>();
            CreateMap<FORNECEDOR, FornecedorViewModel>();
            CreateMap<FORNECEDOR_CONTATO, FornecedorContatoViewModel>();
            CreateMap<FORNECEDOR_COMENTARIO, FornecedorComentarioViewModel>();
            CreateMap<FORNECEDOR_MENSAGEM, FornecedorMensagemViewModel>();
        }
    }
}
