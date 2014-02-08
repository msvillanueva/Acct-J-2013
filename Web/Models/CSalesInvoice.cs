using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core;

namespace Web.Models
{
    public class CSalesInvoice
    {
        public int ID { get; set; }
        public string InvoiceNo { get; set; }
        public int AddedByID { get; set; }
        public string AddedByName { get; set; }
        public int? ProjectID { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string TIN { get; set; }
        public string Business { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectConsolidated { get; set; }
        public DateTime DateAdded { get; set; }
        public string Terms { get; set; }
        public string PWDNo { get; set; }
        public string PWDSignature { get; set; }
        public int PercentCompletion { get; set; }
        public decimal PWDDiscount { get; set; }
        public decimal VatExemptSales { get; set; }
        public decimal ZeroRatedSales { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public bool IsCollected { get; set; }
        public DateTime? DateCollected { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CSalesInvoiceEntry> Entries { get; set; }
        public bool? Vatable { get; set; }
        public string ORNo { get; set; }

        public string Status
        {
            get
            {
                if (this.IsCollected)
                    return "Collected";
                else if (this.IsSubmitted)
                    return "Submitted";
                else
                    return "Pending";
            }
        }

        public decimal VATAmount
        {
            get
            {
                if (this.IsSubmitted)
                {
                    if (this.Vatable == true)
                        return (this.TotalAmount / ((decimal)1 + Constants.VATPercent())) * Constants.VATPercent();
                }
                return 0;
            }
        }

        public decimal NetAmount
        {
            get
            {
                return this.TotalAmount - this.VATAmount;
            }
        }

        public decimal AmountDue
        {
            get
            {
                return this.NetAmount - this.PWDDiscount;
            }
        }

        public decimal TotalAmountDue
        {
            get
            {
                return this.AmountDue + this.VATAmount;
            }
        }
    }
}