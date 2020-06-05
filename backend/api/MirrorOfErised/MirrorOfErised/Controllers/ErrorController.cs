using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MirrorOfErised.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult ErrorPage(int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    ViewBag.ErrorMessage = "Sorry, this resource is non existing.";
                    break;
                case 401:
                    ViewBag.ErrorMessage = "Sorry, You are not authorised";
                    break;
                case 403:
                    ViewBag.ErrorMessage = "Sorry, Acces forbidden. Try verifying your email address";
                    break;
                case 404:
                    ViewBag.ErrorMessage = "Sorry, resource not available or not found.";
                    break;
                default:
                    ViewBag.ErrorMessage = "Sorry, a server error occurred.";
                    
            break;
            }
            return View();
        }
    }
}