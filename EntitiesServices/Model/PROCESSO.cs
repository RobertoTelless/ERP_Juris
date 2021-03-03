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
    
    public partial class PROCESSO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROCESSO()
        {
            this.DOCUMENTO = new HashSet<DOCUMENTO>();
        }
    
        public int PROC_CD_ID { get; set; }
        public int ASSI_CD_ID { get; set; }
        public int CLIE_CD_ID { get; set; }
        public string PROC_NR_NUMERO { get; set; }
        public string PROC_NM_NOME { get; set; }
        public string PROC_DS_ASSUNTO { get; set; }
        public Nullable<int> TIAC_CD_ID { get; set; }
        public Nullable<int> SIPR_CD_ID { get; set; }
        public string PROC_NM_CLASSIFICACAO_INTERNA { get; set; }
        public Nullable<decimal> PROC_VL_VALOR_HORA { get; set; }
        public Nullable<System.DateTime> PROC_DT_INICIO { get; set; }
        public Nullable<System.DateTime> PROC_DT_DISTRIBUICAO { get; set; }
        public Nullable<System.DateTime> PROC_DT_TERMINO { get; set; }
        public Nullable<int> COPR_CD_ID { get; set; }
        public Nullable<int> PACO_CD_ID { get; set; }
        public Nullable<int> ADCO_CD_ID { get; set; }
        public Nullable<int> TIJU_CD_ID { get; set; }
        public Nullable<int> REJU_CD_ID { get; set; }
        public Nullable<int> SECA_CD_ID { get; set; }
        public Nullable<int> SUSE_CD_ID { get; set; }
        public Nullable<int> CICO_CD_ID { get; set; }
        public Nullable<int> FORO_CD_ID { get; set; }
        public Nullable<int> VARA_CD_ID { get; set; }
        public string PROC_NM_JUIZ { get; set; }
        public int PROC_IN_ATIVO { get; set; }
        public string PROC_TX_OBSERVACOES { get; set; }
    
        public virtual ADVOGADO_CONTRARIO ADVOGADO_CONTRARIO { get; set; }
        public virtual ASSINANTE ASSINANTE { get; set; }
        public virtual CIDADE_COMARCA CIDADE_COMARCA { get; set; }
        public virtual CLIENTE CLIENTE { get; set; }
        public virtual CONDICAO_PROCESSUAL CONDICAO_PROCESSUAL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTO> DOCUMENTO { get; set; }
        public virtual FORO FORO { get; set; }
        public virtual PARTE_CONTRARIA PARTE_CONTRARIA { get; set; }
        public virtual REGIAO_JUSTICA REGIAO_JUSTICA { get; set; }
        public virtual SECAO SECAO { get; set; }
        public virtual SITUACAO_PROCESSO SITUACAO_PROCESSO { get; set; }
        public virtual SUBSECAO SUBSECAO { get; set; }
        public virtual TIPO_ACAO TIPO_ACAO { get; set; }
        public virtual TIPO_JUSTICA TIPO_JUSTICA { get; set; }
        public virtual VARA VARA { get; set; }
    }
}
