using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SandeshAgarbathiAPI.Models;

namespace SandeshAgarbathiAPI.Controllers
{
    public class LoginController : ApiController
    {
        // GET: Login
        [System.Web.Http.HttpGet]
        public IHttpActionResult login(String email, String password)
        {
            var loginModal = new LoginModal();
            var output = loginModal.checkLogin(email, password);
            if (output.id > 0)
            {
                return Ok(output);
            }
            return Content(HttpStatusCode.InternalServerError, "Error in login");
        }
    }
}