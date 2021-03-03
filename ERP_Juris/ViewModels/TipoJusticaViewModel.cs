using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;

namespace ERP_Juris.ViewModels
{
    public class TipoJusticaViewModel
    {
        [Key]
        public int TIJU_CD_ID { get; set; }
        public int ASSI_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo NOME obrigatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O NOME deve ter no minimo 1 caractere e no máximo 50 caracteres.")]
        public string TIJU_NM_NOME { get; set; }
        [StringLength(500, ErrorMessage = "A DESCRIÇÃO deve ter no máximo 500 caracteres.")]
        public string TIJU_DS_DESCRICAO { get; set; }
        public int TIJU_IN_ATIVO { get; set; }

        public virtual ASSINANTE ASSINANTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROCESSO> PROCESSO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REGIAO_JUSTICA> REGIAO_JUSTICA { get; set; }

    }
}