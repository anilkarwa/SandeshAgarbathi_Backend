using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SandeshAgarbathiAPI.BusinessEntities
{
    public class Customer
    {
        public Int32 id { get; set; }
        public String code { get; set; }
        public String name { get; set; }
        public String email { get; set; }

        public String addressLine1 { get; set; }
        public String addressLine2 { get; set; }
        public String addressLine3 { get; set; }
        
        public String city { get; set; }
        public String state { get; set; }
        public String country { get; set; }
        public String pincode { get; set; }
        public Int32 groupId { get; set; }
        public String phoneNumber { get; set; }
        public String mobileNumber { get; set; }
        public String contactPerson { get; set; }
        public String gstNo { get; set; }
        public Int32 currency { get; set; }
        public String addedBy { get; set; }
        public String addedOn { get; set;}
        public String changedBy { get; set; }
        public String changedOn { get; set; }
        public String remarks { get; set; }
        public String inActive { get; set; }


    }
}