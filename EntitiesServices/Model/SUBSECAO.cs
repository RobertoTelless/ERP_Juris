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
    
    public partial class SUBSECAO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUBSECAO()
        {
            this.PROCESSO = new HashSet<PROCESSO>();
        }
    
        public int SUSE_CD_ID { get; set; }
        public int ASSI_CD_ID { get; set; }
        public int SECA_CD_ID { get; set; }
        public string SUSE_NM_NOME { get; set; }
        public string SUSE_DS_DESCRICAO { get; set; }
        public int SUSE_IN_ATIVO { get; set; }
    
        public virtual ASSINANTE ASSINANTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROCESSO> PROCESSO { get; set; }
        public virtual SECAO SECAO { get; set; }
    }
}
