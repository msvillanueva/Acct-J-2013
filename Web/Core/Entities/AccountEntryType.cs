//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Core.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccountEntryType
    {
        public AccountEntryType()
        {
            this.AccountEntries = new HashSet<AccountEntry>();
            this.AccountEntryType1 = new HashSet<AccountEntryType>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public Nullable<int> ParentID { get; set; }
    
        public virtual ICollection<AccountEntry> AccountEntries { get; set; }
        public virtual ICollection<AccountEntryType> AccountEntryType1 { get; set; }
        public virtual AccountEntryType AccountEntryType2 { get; set; }
    }
}
