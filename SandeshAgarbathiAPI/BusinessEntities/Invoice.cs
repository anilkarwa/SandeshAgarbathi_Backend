using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SandeshAgarbathiAPI.BusinessEntities
{
    public class Invoice
    {
        public Int32 invoiceId { get; set; }
        public String invoiceNo { get; set; }
        public String prefix { get; set; }
        public String invoiceDate { get; set; }
        public String time { get; set; }
        public Int32 custId { get; set; }
        public String partyName { get; set; }
        public String addressLine1 { get; set; }
        public String addressLine2 { get; set; }
        public String addressLine3 { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public String country { get; set; }
        public String pincode { get; set; }
        public String addedBy { get; set; }
        public String remarks { get; set; }
        public String addedOn { get; set; }
        public Decimal  grossAmt { get; set; }
        public Decimal totalAmt { get; set; }
        public Decimal roundOffAmt { get; set; }
        public Decimal grandTotalAmt { get; set; }
        public Decimal totalCGSTAmt { get; set; }
        public Decimal totalSGSTAmt { get; set; }
        public Decimal discAmt { get; set; }
        public String agent { get; set; }
        public List<InvoiceItem> items { get; set; }

    }
}