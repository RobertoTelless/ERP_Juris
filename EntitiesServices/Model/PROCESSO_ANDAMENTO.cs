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
    
    public partial class PROCESSO_ANDAMENTO
    {
        public int PRAP_CD_ID { get; set; }
        public int PROC_CD_ID { get; set; }
        public Nullable<System.DateTime> PRAP_DT_DATA { get; set; }
        public string PRAP_DESCRICAO { get; set; }
        public Nullable<int> TIAN_CD_ID { get; set; }
        public int PRAP_IN_ATIVO { get; set; }
    }
}
