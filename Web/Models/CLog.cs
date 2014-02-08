using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CLog
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Table { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserFullName { get; set; }
    }
}