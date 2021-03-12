using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;
using EntitiesServices.Attributes;

namespace ERP_Juris.ViewModels
{
    public class VaraViewModel
    {
        [Key]
        public int VARA_CD_ID { get; set; }
        public int ASSI_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo FORO obrigatorio")]
        public int FORO_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo NOME obrigatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O NOME deve conter no minimo 1 e no máximo 50 caracteres.")]
        public string VARA_NM_NOME { get; set; }
        [StringLength(500, ErrorMessage = "A DESCRIÇÂO deve conter no máximo 500 caracteres.")]
        public string VARA_DS_DESCRICAO { get; set; }
        [StringLength(50, ErrorMessage = "O NOME DO JUIZ deve conter no máximo 50 caracteres.")]
        public string VARA_NM_JUIZ { get; set; }
        [StringLength(50, ErrorMessage = "O ENDEREÇO deve conter no máximo 50 caracteres.")]
        public string VARA_NM_ENDERECO { get; set; }
        [StringLength(50, ErrorMessage = "O NÚMERO deve conter no máximo 50 caracteres.")]
        public string VARA_NR_NUMERO { get; set; }
        [StringLength(50, ErrorMessage = "O COMPLEMENTO deve conter no máximo 50 caracteres.")]
        public string VARA_NR_COMPLEMENTO { get; set; }
        [StringLength(50, ErrorMessage = "O BAIRRO deve conter no máximo 50 caracteres.")]
        public string VARA_NM_BAIRRO { get; set; }
        [StringLength(50, ErrorMessage = "A CIDADE deve conter no máximo 50 caracteres.")]
        public string VARA_NM_CIDADE { get; set; }
        [StringLength(10, ErrorMessage = "O CEP deve conter no máximo 10 caracteres.")]
        public string VARA_NR_CEP { get; set; }
        public string VARA_TX_OBSERVACOES { get; set; }
        public Nullable<int> UF_CD_ID { get; set; }
        public int VARA_IN_ATIVO { get; set; }

        public virtual ASSINANTE ASSINANTE { get; set; }
        public virtual FORO FORO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROCESSO> PROCESSO { get; set; }
        public virtual UF UF { get; set; }
    }
}