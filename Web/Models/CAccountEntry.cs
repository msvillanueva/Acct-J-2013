using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core;
using Web.Core.Entities;

namespace Web.Models
{
    public class CAccountEntry
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }
        public string TypeName { get; set; }
        public int? AccountEntryTypeID { get; set; }
        public string ClassName { get; set; }
        public CAccountEntryType AcctEntryType { get; set; }
    }
}