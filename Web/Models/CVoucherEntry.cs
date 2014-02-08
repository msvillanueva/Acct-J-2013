using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CVoucherEntry
    {
        public int ID { get; set; }
        public int VoucherID { get; set; }
        public int AccountEntryID { get; set; }
        public int? BankID { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int AddedByID { get; set; }
        public DateTime DateAdded { get; set; }
        public string AccountEntryName { get; set; }
        public string AccountEntryCode { get; set; }
        public int? AccountEntryTypeID { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AddedByName { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string CheckNo { get; set; }
        public DateTime? CheckDate { get; set; }
        public string PayeeName { get; set; }
    }
}