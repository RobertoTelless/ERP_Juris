using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;
using EntitiesServices.Attributes;

namespace ERP_Juris.ViewModels
{
    public class UsuariofuncionarioViewModel
    {
        [Key]
        public int USFU_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo USUÁRIO obrigatorio")]
        public int USUA_CD_ID { get; set; }
        public Nullable<System.DateTime> USFU_DT_NASCIMENTO { get; set; }
        [StringLength(50, ErrorMessage = "O NOME DO PAI deve ter no máximo 50 caracteres.")]
        public string USFU_NM_PAI { get; set; }
        [StringLength(50, ErrorMessage = "O NOME DA MÃE deve ter no máximo 50 caracteres.")]
        public string USFU_NM_MAE { get; set; }
        [StringLength(50, ErrorMessage = "A NATURALIDADE deve ter no máximo 50 caracteres.")]
        public string USFU_NM_NATURALIDADE { get; set; }
        public string USFU_SG_UF_NATURALIDADE { get; set; }
        [StringLength(50, ErrorMessage = "A NACIONALIDADE deve ter no máximo 50 caracteres.")]
        public string USFU_NM_NACIONALIDADE { get; set; }
        [StringLength(10, ErrorMessage = "O TIPO SANGUINEO deve ter no máximo 10 caracteres.")]
        public string USFU_NM_TIPO_SANGUINEO { get; set; }
        [StringLength(10, ErrorMessage = "O FATOR RH deve ter no máximo 10 caracteres.")]
        public string USFU_NM_FATOR_RH { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE EMISSÂO DO RG Deve ser uma data válida")]
        public Nullable<System.DateTime> USFU_DT_EMISSAO_RG { get; set; }
        [StringLength(50, ErrorMessage = "O ÓRGÃO EMISSOR DO RG deve ter no máximo 50 caracteres.")]
        public string USFU_NM_ORGAO_RG { get; set; }
        public string USFU_SG_UF_RG { get; set; }
        [StringLength(20, ErrorMessage = "O NIS deve ter no máximo 20 caracteres.")]
        public string USFU_NR_NIS { get; set; }
        [StringLength(20, ErrorMessage = "O TÍTULO DE ELEITOR deve ter no máximo 20 caracteres.")]
        public string USFU_NR_TITULO_ELEITOR { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DO TIT.ELEITOR deve ser uma data válida")]
        public Nullable<System.DateTime> USFU_DT_TITULO_ELEITOR { get; set; }
        [StringLength(20, ErrorMessage = "A ZONA ELEITORAL deve ter no máximo 20 caracteres.")]
        public string USFU_NR_TITULO_ELEITOR_ZONA { get; set; }
        [StringLength(20, ErrorMessage = "A SEÇÃO ELEITORAL deve ter no máximo 20 caracteres.")]
        public string USFU_NR_TITULO_ELEITOR_SECAO { get; set; }
        [StringLength(50, ErrorMessage = "O MUNICÌPIO deve ter no máximo 50 caracteres.")]
        public string USFU_NM_TITULO_ELEITOR_MUNICIPIO { get; set; }
        public string USFU_SG__TITULO_ELEITOR_UF { get; set; }
        [StringLength(20, ErrorMessage = "A CTPS deve ter no máximo 20 caracteres.")]
        public string USFU_NR_CTPS { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DA CTPS deve ser uma data válida")]
        public Nullable<System.DateTime> USFU_DT_CTPS { get; set; }
        [StringLength(20, ErrorMessage = "A SÉRIE DA CTPS deve ter no máximo 20 caracteres.")]
        public string USFU_NR_CTPS_SERIE { get; set; }
        public string USFU_SG_CTPS_UF { get; set; }
        [StringLength(20, ErrorMessage = "A IDENTIDADE DE CLASSE deve ter no máximo 20 caracteres.")]
        public string USFU_NR_IDENTIDADE_CLASSE { get; set; }
        [StringLength(20, ErrorMessage = "O ÓRGÃO EMISSOR DA IDENTIDADE DE CLASSE deve ter no máximo 20 caracteres.")]
        public string USFU_NM_ORGAO_CLASSE { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DA IDENT.CLASSE deve ser uma data válida")]
        public Nullable<System.DateTime> USFU_DT_IDENTIDADE_CLASSE_EMISSAO { get; set; }
        [StringLength(20, ErrorMessage = "O CERT.RESERVISTA deve ter no máximo 20 caracteres.")]
        public string USFU_NR_RESERVISTA { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DO CERT.RESERVISTA deve ser uma data válida")]
        public Nullable<System.DateTime> USFU_DT_RESERVISTA_DATA { get; set; }
        [StringLength(20, ErrorMessage = "A CATEGORIA DO CERT.RESERVISTA deve ter no máximo 20 caracteres.")]
        public string USFU_NM_RESERVISTA_CATEGORIA { get; set; }
        [StringLength(20, ErrorMessage = "A ORG.MILITAR DO CERT. RESERVISTA deve ter no máximo 20 caracteres.")]
        public string USFU_NM_ORGANIZACAO_MILITAR { get; set; }
        [StringLength(20, ErrorMessage = "A CNH deve ter no máximo 20 caracteres.")]
        public string USFU_NR_CNH { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE EMISSÂO DA CNH deve ser uma data válida")]
        public Nullable<System.DateTime> USFU_DT_CNH_EMISSAO { get; set; }
        [DataType(DataType.Date, ErrorMessage = "DATA DE VALIDADE DA CNH deve ser uma data válida")]
        public Nullable<System.DateTime> USFU_DT_CNH_VALIDADE { get; set; }
        [StringLength(20, ErrorMessage = "A CATEGORIA DA CNH deve ter no máximo 20 caracteres.")]
        public string USFU_NM_CNH_CATEGORIA { get; set; }
        [StringLength(20, ErrorMessage = "A CNS deve ter no máximo 20 caracteres.")]
        public string USFU_NR_CNS { get; set; }
        public Nullable<int> ESCO_CD_ID { get; set; }
        public Nullable<int> ESCI_CD_ID { get; set; }
        public Nullable<int> PROF_CD_ID { get; set; }
        [StringLength(50, ErrorMessage = "O ENDEREÇO deve ter no máximo 50 caracteres.")]
        public string USFU_NM_ENDERECO { get; set; }
        [StringLength(20, ErrorMessage = "O NÙMERO deve ter no máximo 20 caracteres.")]
        public string USFU_NR_NUMERO { get; set; }
        [StringLength(20, ErrorMessage = "O COMPLEMENTO deve ter no máximo 20 caracteres.")]
        public string USFU_NR_COMPLEMENTO { get; set; }
        [StringLength(50, ErrorMessage = "O BAIRRO deve ter no máximo 50 caracteres.")]
        public string USFU_NM_BAIRRO { get; set; }
        [StringLength(50, ErrorMessage = "A CIDADE deve ter no máximo 50 caracteres.")]
        public string USFU_NM_CIDADE { get; set; }
        public Nullable<int> USFU_SG_UF { get; set; }
        [StringLength(10, ErrorMessage = "O CEP deve ter no máximo 10 caracteres.")]
        public string USFU_NR_CEP { get; set; }
        public Nullable<int> USFU_SG_NATUR_UF { get; set; }
        public Nullable<int> USFU_SG_RG_UF { get; set; }
        public Nullable<int> USFU_SG_TITULO_UF { get; set; }
        public Nullable<int> USFU_SG_CARTTRAB_UF { get; set; }

        public virtual USUARIO USUARIO { get; set; }
        public virtual ESCOLARIDADE ESCOLARIDADE { get; set; }
        public virtual ESTADO_CIVIL ESTADO_CIVIL { get; set; }
        public virtual PROFISSAO PROFISSAO { get; set; }
        public virtual UF UF { get; set; }
        public virtual UF UF1 { get; set; }
        public virtual UF UF2 { get; set; }
        public virtual UF UF3 { get; set; }
        public virtual UF UF4 { get; set; }
    }
}