using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;
using EntitiesServices.Attributes;

namespace ERP_Juris.ViewModels
{
    public class UsuarioViewModel
    {
        [Key]
        public int USUA_CD_ID { get; set; }
        public int ASSI_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo PERFIL obrigatorio")]
        public int PERF_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo CATEGORIA obrigatorio")]
        public Nullable<int> CAUS_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo CARGO obrigatorio")]
        public Nullable<int> CARG_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo NOME obrigatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O NOME deve ter no minimo 1 e no máximo 50 caracteres.")]
        public string USUA_NM_NOME { get; set; }
        [Required(ErrorMessage = "Campo LOGIN obrigatorio")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "O LOGIN deve ter no minimo 1 e no máximo 10 caracteres.")]
        public string USUA_NM_LOGIN { get; set; }
        [StringLength(150, ErrorMessage = "O E-MAIL deve ter no máximo 150 caracteres.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Deve ser um e-mail válido")]
        public string USUA_NM_EMAIL { get; set; }
        [StringLength(10, ErrorMessage = "A MATRÍCULA deve ter no máximo 10 caracteres.")]
        public string USUA_NR_MATRICULA { get; set; }
        [StringLength(50, ErrorMessage = "O TELEFONE deve ter no máximo 50 caracteres.")]
        public string USUA_NR_TELEFONE { get; set; }
        [StringLength(50, ErrorMessage = "O CELULAR deve ter no máximo 50 caracteres.")]
        public string USUA_NR_CELULAR { get; set; }
        [StringLength(50, ErrorMessage = "O WHATSAPP deve ter no máximo 50 caracteres.")]
        public string USUA_NR_WHATSAPP { get; set; }
        [StringLength(10, MinimumLength = 1, ErrorMessage = "A SENHA deve ter no minimo 1 e no máximo 10 caracteres.")]
        public string USUA_NM_SENHA { get; set; }
        [StringLength(10, ErrorMessage = "A SENHA deve ter no máximo 10 caracteres.")]
        public string USUA_NM_SENHA_CONFIRMA { get; set; }
        [StringLength(10, ErrorMessage = "A SENHA deve ter no máximo 10 caracteres.")]
        public string USUA_NM_NOVA_SENHA { get; set; }
        public Nullable<int> USUA_IN_BLOQUEADO { get; set; }
        public Nullable<int> USUA_IN_PROVISORIO { get; set; }
        public Nullable<int> USUA_IN_LOGIN_PROVISORIO { get; set; }
        public Nullable<int> USUA_IN_ATIVO { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE BLOQUEIO Deve ser uma data válida")]
        public Nullable<System.DateTime> USUA_DT_BLOQUEADO { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE ALTERAÇÃO Deve ser uma data válida")]
        public Nullable<System.DateTime> USUA_DT_ALTERACAO { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE TROCA DE SENHA Deve ser uma data válida")]
        public Nullable<System.DateTime> USUA_DT_TROCA_SENHA { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE ACESSO Deve ser uma data válida")]
        public Nullable<System.DateTime> USUA_DT_ACESSO { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DA ULTIMA FALHA Deve ser uma data válida")]
        public Nullable<System.DateTime> USUA_DT_ULTIMA_FALHA { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE CADASTRO Deve ser uma data válida")]
        public Nullable<System.DateTime> USUA_DT_CADASTRO { get; set; }
        public Nullable<int> USUA_NR_ACESSOS { get; set; }
        public Nullable<int> USUA_NR_FALHAS { get; set; }
        public string USUA_TX_OBSERVACOES { get; set; }
        public string USUA_AQ_FOTO { get; set; }
        public Nullable<int> USUA_IN_LOGADO { get; set; }
        [StringLength(20, ErrorMessage = "O CPF deve ter no máximo 20 caracteres.")]
        [CustomValidationCPF(ErrorMessage = "CPF inválido")]
        public string USUA_NR_CPF { get; set; }
        [StringLength(20, ErrorMessage = "O RG deve ter no máximo 20 caracteres.")]
        public string USUA_NR_RG { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE ENTRADA Deve ser uma data válida")]
        public Nullable<System.DateTime> USUA_DT_ENTRADA { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE SAÌDA Deve ser uma data válida")]
        public Nullable<System.DateTime> USUA_DT_SAIDA { get; set; }
        [StringLength(200, ErrorMessage = "O MOTIVO DA SAÌDA deve ter no máximo 200 caracteres.")]
        public string USUA_DS_MOTIVO_SAIDA { get; set; }
        public Nullable<int> ESPE_CD_ID { get; set; }
        public Nullable<int> TIDE_CD_ID { get; set; }
        public string USUA_NR_OAB { get; set; }
        public Nullable<System.DateTime> USUA_DT_OAB { get; set; }

        public bool Bloqueio
        {
            get
            {
                if (USUA_IN_BLOQUEADO == 1)
                {
                    return true;
                }
                return false;
            }
            set
            {
                USUA_IN_BLOQUEADO = (value == true) ? 1 : 0;
            }
        }
        public bool LoginProvisorio
        {
            get
            {
                if (USUA_IN_LOGIN_PROVISORIO == 1)
                {
                    return true;
                }
                return false;
            }
            set
            {
                USUA_IN_LOGIN_PROVISORIO = (value == true) ? 1 : 0;
            }
        }
        public bool Provisoria
        {
            get
            {
                if (USUA_IN_PROVISORIO == 1)
                {
                    return true;
                }
                return false;
            }
            set
            {
                USUA_IN_PROVISORIO = (value == true) ? 1 : 0;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AGENDA> AGENDA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AGENDA> AGENDA1 { get; set; }
        public virtual ASSINANTE ASSINANTE { get; set; }
        public virtual CARGO CARGO { get; set; }
        public virtual CATEGORIA_USUARIO CATEGORIA_USUARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORNECEDOR_COMENTARIO> FORNECEDOR_COMENTARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOG> LOG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTICIA_COMENTARIO> NOTICIA_COMENTARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTIFICACAO> NOTIFICACAO { get; set; }
        public virtual PERFIL PERFIL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAREFA> TAREFA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAREFA_ACOMPANHAMENTO> TAREFA_ACOMPANHAMENTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAREFA_NOTIFICACAO> TAREFA_NOTIFICACAO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAREFA_VINCULO> TAREFA_VINCULO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_ANEXO> USUARIO_ANEXO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_FUNCIONARIO> USUARIO_FUNCIONARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORNECEDOR_MENSAGEM> FORNECEDOR_MENSAGEM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPRESA> EMPRESA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADVOGADO_CONTRARIO_COMENTARIO> ADVOGADO_CONTRARIO_COMENTARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE_COMENTARIO> CLIENTE_COMENTARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE_MENSAGEM> CLIENTE_MENSAGEM { get; set; }
        public virtual ESPECIALIDADE ESPECIALIDADE { get; set; }
        public virtual TIPO_DESLIGAMENTO TIPO_DESLIGAMENTO { get; set; }

    }
}