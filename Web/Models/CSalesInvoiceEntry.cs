using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CSalesInvoiceEntry
    {
        public int ID { get; set; }
        public int SalesInvoiceID { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Articles { get; set; }
        public decimal UnitPrice { get; set; }
        public int AddedByID { get; set; }
        public DateTime DateAdded { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public int? AccountEntryID { get; set; }
        public string AccountEntryName { get; set; }
        public string AccountEntryCode { get; set; }

        public decimal Amount
        {
            get
            {
                return this.Quantity * this.UnitPrice;
            }
        }
    }
}