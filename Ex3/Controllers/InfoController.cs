using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class InfoController : Controller
    {
        // GET: Info
        
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult display(string ip, int port)
        {
            //write stuff
            //create client
            //open client server
            //pass lon and lat
            //disconnect
            return View();
        }
        
        
        [HttpGet]
        public ActionResult save()
        {
            //write stuff

            return View();
        }
        
    }
}