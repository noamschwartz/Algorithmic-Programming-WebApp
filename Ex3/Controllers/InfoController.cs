using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ex3.Models;

namespace Ex3.Controllers
{
    public class InfoController : Controller
    {

        Client client;
        // GET: Info
        
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult display(string ip, int port)
        {

            this.client = new Client(ip, port);
    
            //create client
            //open client
            //pass lon and lat
            //disconnect
            return View();
        }


        [HttpGet]
        public ActionResult displayPerTime(string ip, int port, int timesPerSec)
        {
            //write stuff
            //create client
            //open client
            //pass relative lon and lat
            //disconnect
            return View();
        }


        [HttpGet]
        public ActionResult save(string ip, int port, int timesPerSec, int numOfSec, string fileName)
        {
            //write stuff
            //get the lon and lat for 10 seconds 4 times a sec and write to file
            //probably wont need to show anything
            return View();
        }

        /*
        [HttpGet]
        public ActionResult displayFromFile(string fileName, int timesPerSec)
        {
            //write stuff
            //load file
            //open client
            //pass path made by lon and lat
            //disconnect
            return View();
        }
        */
    }
}