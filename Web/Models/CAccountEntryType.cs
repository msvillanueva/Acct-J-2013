using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;

namespace Web.Models
{
    public class CAccountEntryType
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public int? ParentID { get; set; }
        public AccountEntryType Parent { get; set; }
    }
}