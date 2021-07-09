using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SandeshAgarbathiAPI.BusinessEntities
{
    public class Item
    {
        public Int32 id { get; set; }
        public String code { get; set; }
        public String name { get; set; }
        public Decimal rate { get; set; }

        public Decimal cgst { get; set; }
        public Decimal sgst { get; set; }
        public Decimal discount { get; set; }
    }
}