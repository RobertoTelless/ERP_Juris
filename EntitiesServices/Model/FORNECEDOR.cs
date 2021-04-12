//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntitiesServices.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class FORNECEDOR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FORNECEDOR()
        {
            this.FORNECEDOR_ANEXO = new HashSet<FORNECEDOR_ANEXO>();
            this.FORNECEDOR_COMENTARIO = new HashSet<FORNECEDOR_COMENTARIO>();
            this.FORNECEDOR_CONTATO = new HashSet<FORNECEDOR_CONTATO>();
            this.FORNECEDOR_MENSAGEM = new HashSet<FORNECEDOR_MENSAGEM>();
            this.FORNECEDOR_QUADRO_SOCIETARIO = new HashSet<FORNECEDOR_QUADRO_SOCIETARIO>();
        }
    
        public int FORN_CD_ID { get; set; }
        public int ASSI_CD_ID { get; set; }
        public Nullable<int> CAFO_CD_ID { get; set; }
        public Nullable<int> TIPE_CD_ID { get; set; }
        public string FORN_NM_NOME { get; set; }
        public string FORN_NM_RAZAO_SOCIAL { get; set; }
        public string FORN_NR_DOCUMENTO { get; set; }
        public string FORM_NM_ENDERECO { get; set; }
        public string FORN_NM_BAIRRO { get; set; }
        public string FORN_NM_CIDADE { get; set; }
        public Nullable<int> UF_CD_ID { get; set; }
        public string FORN_NR_CEP { get; set; }
        public string FORN_NR_TELEFONE { get; set; }
        public string FORN_NM_CONTATO { get; set; }
        public string FORN_NM_EMAIL { get; set; }
        public string FORN_DS_DESCRICAO { get; set; }
        public System.DateTime FORN_DT_CADASTRO { get; set; }
        public int FORN_IN_ATIVO { get; set; }
        public string FORN_DS_ESCOPO { get; set; }
        public string FORN_NR_CELULAR { get; set; }
        public string FORN_NR_WHATSAPP { get; set; }
        public string FORN_NM_WEBSITE { get; set; }
        public string FORN_NR_INSCRICAO_ESTADUAL { get; set; }
        public string FORN_NM_SITUACAO { get; set; }
        public string FORN_NR_INSCRICAO_MUNICIPAL { get; set; }
        public string FORN_NR_CPF { get; set; }
        public string FORN_NR_CNPJ { get; set; }
        public string FORN_AQ_FOTO { get; set; }
        public string FORN_TX_OBSERVACOES { get; set; }
    
        public virtual ASSINANTE ASSINANTE { get; set; }
        public virtual CATEGORIA_FORNECEDOR CATEGORIA_FORNECEDOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORNECEDOR_ANEXO> FORNECEDOR_ANEXO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORNECEDOR_COMENTARIO> FORNECEDOR_COMENTARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORNECEDOR_CONTATO> FORNECEDOR_CONTATO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORNECEDOR_MENSAGEM> FORNECEDOR_MENSAGEM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORNECEDOR_QUADRO_SOCIETARIO> FORNECEDOR_QUADRO_SOCIETARIO { get; set; }
        public virtual TIPO_PESSOA TIPO_PESSOA { get; set; }
        public virtual UF UF { get; set; }
    }
}
