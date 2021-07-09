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
    public class CustomerController : ApiController
    {
        // GET: Customers
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetAllCustomers()
        {
            var custModal = new CustomerModal();
            var output = custModal.CustomerList();
            if (output.Count > 0)
            {
                return Ok(output);
            }
            return Content(HttpStatusCode.InternalServerError, "Error getting customers");
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/saveCustomer")]
        public IHttpActionResult SaveCustomer(List<Customer> customers)
        {
            var custModal = new CustomerModal();
            var output = custModal.SaveCustomers(customers);
            if (output)
            {
                return Ok("Saved customer successfully");
            }
            return Content(HttpStatusCode.InternalServerError, "Error saving customer");
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/updateCustomer")]
        public IHttpActionResult UpdateCustomer(List<Customer> customers)
        {
            var custModal = new CustomerModal();
            var output = custModal.UpdateCustomer(customers);
            if (output)
            {
                return Ok("Updated customers successfully");
            }
            return Content(HttpStatusCode.InternalServerError, "Error updating customer");
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult DeleteCustomer(List<Customer> custIds)
        {
            var custModal = new CustomerModal();
            var output = custModal.DeleteCustomer(custIds);
            if (output)
            {
                return Ok("Deleted customers successfully");
            }
            return Content(HttpStatusCode.InternalServerError, "Error deleting customer");
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/customer-groups")]
        public IHttpActionResult GetCustomerGroups()
        {
            var custModal = new CustomerModal();
            var output = custModal.getCustomerGroups();
            if (output.Count > 0)
            {
                return Ok(output);
            }
            return Content(HttpStatusCode.InternalServerError, "Error getting customer groups");
        }
    }
}