using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CPayee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Tin { get; set; }
        public DateTime? DateAdded { get; set; }
        public bool? IsActive { get; set; }
    }
}