using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SandeshAgarbathiAPI.BusinessEntities
{
    public class InvoiceItem
    {
        public Int32 invoiceItemId { get; set; }
        public String itemName { get; set; }
        public Int32 invoiceId { get; set; }
        public Int32 slNo { get; set; }
        public Int32 itemId { get; set; }
        public Decimal quantity { get; set; }
        public Decimal rate { get; set; }
        public Decimal grossAmt { get; set; }
        public Decimal discPer { get; set; }
        public Decimal discAmt { get; set; }
        public Decimal netAmt { get; set; }
        public Decimal cgstPer { get; set; }
        public Decimal sgstPer { get; set; }
        public Decimal cgstAmt { get; set; }
        public Decimal sgstAmt { get; set; }
        public Decimal totalAmt { get; set; }
    }
}