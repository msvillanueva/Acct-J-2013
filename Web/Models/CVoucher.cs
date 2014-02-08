using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CVoucher
    {
        public int ID { get; set; }
        public int PayeeID { get; set; }
        public int ProjectID { get; set; }
        public string Remarks { get; set; }
        public string CheckNo { get; set; }
        public decimal? CheckAmount { get; set; }
        public DateTime? CheckDate { get; set; }
        public int AddedByID { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedByName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string PayeeName { get; set; }
        public bool Submitted { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public List<CVoucherEntry> Entries { get; set; }
        public string ProjectConsolidated { get; set; }

        public string Status
        {
            get
            {
                return this.Submitted ? "Submitted" : "Pending";
            }
        }
    }
}