using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CProject
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Decimal ContractAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}