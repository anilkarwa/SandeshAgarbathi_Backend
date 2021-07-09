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
    public class ItemController : ApiController
    {
        // GET: Items
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetAllItems()
        {
            var itemModal = new ItemModal();
            var output = itemModal.GetAllItems();
            if (output.Count > 0)
            {
                return Ok(output);
            }
            return Content(HttpStatusCode.InternalServerError, "Error getting items");
        }
    }
}