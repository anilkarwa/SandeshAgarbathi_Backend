using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SandeshAgarbathiAPI.Models;
using SandeshAgarbathiAPI.BusinessEntities;

namespace SandeshAgarbathiAPI.Controllers
{
    public class InvoiceController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/getAllInvoice")]
        public IHttpActionResult GetInvoiceList(String prefix)
        {
            var invoiceModal = new InvoiceModal();
            var output = invoiceModal.InvoiceList(prefix);
            if (output.Count > 0)
            {
                return Ok(output);
            }
            return Content(HttpStatusCode.InternalServerError, "Error getting invoice");
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/saveInvoice")]
        public IHttpActionResult SaveInvoice(List<Invoice> invoiceList)
        {
            var invoiceModal = new InvoiceModal();
            var output = invoiceModal.SaveInvoices(invoiceList);
            if (output)
            {
                return Ok("Saved invoice successfully");
            }
            return Content(HttpStatusCode.InternalServerError, "Error saving invoices");
        }
    }
}