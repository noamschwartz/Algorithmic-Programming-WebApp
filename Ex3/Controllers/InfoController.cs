using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ex3.Models;
using System.Net;
using System.Xml;
using System.Text;
using System.IO;

namespace Ex3.Controllers
{
    public class InfoController : Controller
    {

        private const string lonLine = "get /position/longitude-deg";
        private const string latLine = "get /position/latitude-deg";
        private const string throttleLine = "get /controls/engines/current-engine/throttle";
        private const string rudderLine = "get /controls/flight/rudder";

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// this is the normal plane location display method
        /// </summary>
        /// <param name="ip"> ip address to connect to</param>
        /// <param name="port">port to connect to</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult display(string ip, int port)
        {
            IPAddress address;
            //if the first arg is a correct ip address.
            if (IPAddress.TryParse(ip, out address))
            {
                Writer writer = Writer.getInstance();
                writer.Ip = ip;
                writer.Port = port;
                writer.connect();
                ViewBag.Lon = writer.write(lonLine);
                ViewBag.Lat = writer.write(latLine);
                ViewBag.normalDisplay = true;
                ViewBag.time = 0;
                //writer.disconnect();
                return View("~/Views/Info/display.cshtml");

            }
            //if the first arg is a file name. execute task 4.
            else
            {
                string fileName = ip;
                int timesPerSec = port;
                FromFile fromFile = FromFile.Instance;
                fromFile.FileName = fileName;
                //get all of the data from the file
                fromFile.Read();
                ViewBag.numOfLines = fromFile.Information.Count();
                ViewBag.times = timesPerSec;
                //get single line
                var singleLine = fromFile.Get().Split(',');
                //assign to lat and lon
                ViewBag.lat = Double.Parse(singleLine[0]);
                ViewBag.lon = Double.Parse(singleLine[1]);
                return View("~/Views/Info/displayFromFile.cshtml");

            }
 
        }
        /// <summary>
        /// This reads the file line by line and creates an xml from each line
        /// </summary>
        /// <returns> line from file as xml</returns>
        [HttpPost]
        public string ValuesFromFile()
        {
            FromFile fromFile = FromFile.Instance;
            string location = fromFile.Get();
            return ToXml(location);
        }
        /// <summary>
        /// this shows the plane place constantly.
        /// </summary>
        /// <param name="ip" ip address to connect></param>
        /// <param name="port">port to connect</param>
        /// <param name="timesPerSec">how many times to refresh per second.</param>
        /// <returns> view</returns>
        [HttpGet]
        public ActionResult displayPerTime(string ip, int port, int timesPerSec)
        {
            Writer writer = Writer.getInstance();
            writer.Ip = ip;
            writer.Port = port;
            writer.connect();
            ViewBag.Lon = writer.write(lonLine);
            ViewBag.Lat = writer.write(latLine);
            ViewBag.seconds = timesPerSec;
            //writer.disconnect();
            return View();
        }

        /// <summary>
        /// this saves the planes data to a file
        /// </summary>
        /// <param name="ip"> ip address to connect to</param>
        /// <param name="port">port to conect to</param>
        /// <param name="timesPerSec">how many times to get info per second</param>
        /// <param name="secondsToShow">for how kong to get info</param>
        /// <param name="fileName">file name to save al the data to</param>
        /// <returns>view</returns>
        [HttpGet]
        public ActionResult save(string ip, int port, int timesPerSec, int secondsToShow, string fileName)
        {
            SaveInfo saveInfo = SaveInfo.Instance;
            Writer writer = Writer.getInstance();
            writer.Ip = ip;
            writer.Port = port;
            writer.connect();
            ViewBag.Lon = writer.write(lonLine);
            ViewBag.Lat = writer.write(latLine);
            saveInfo.FileName = fileName;
            ViewBag.seconds = timesPerSec;
            ViewBag.times = secondsToShow;
            return View();
        }
        /// <summary>
        /// this saves the values taken from the sumulator to a file.
        /// </summary>
        /// <returns> the info as an xml</returns>
        [HttpPost]
        public string SaveValues()
        {
            var saveFlight = SaveInfo.Instance;
            Writer writer = Writer.getInstance();
            writer.connect();

            saveFlight.Lat = writer.write(latLine);
            saveFlight.Lon = writer.write(lonLine);
            saveFlight.Rudder = writer.write(rudderLine);
            saveFlight.Throttle = writer.write(throttleLine);
            saveFlight.SaveToFile();
     
            return ToXml(saveFlight.Lat.ToString() + "," + saveFlight.Lon.ToString() + "," + saveFlight.Rudder + "," + saveFlight.Throttle.ToString());
        }
        /// <summary>
        /// this assigns all of the values to their correct place and creates an xml.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetValues()
        {
            Writer writer = Writer.getInstance();
            writer.connect();
            double lat = writer.write(latLine);
            double lon = writer.write(lonLine);
            double rudder = writer.write(rudderLine);
            double throttle = writer.write(throttleLine);
            return ToXml(lat.ToString() + "," + lon.ToString() + "," + rudder.ToString() + "," + throttle.ToString());
        }
        /// <summary>
        /// this creates an xml format
        /// </summary>
        /// <param name="info">the info to convert to xml</param>
        /// <returns>the info as an xml</returns>
        private string ToXml(string info)
        {

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Values");
            string[] Values = info.Split(',');
            writer.WriteElementString("Lat", Values[0]);
            writer.WriteElementString("Lon", Values[1]);
            writer.WriteElementString("Rudder", Values[2]);
            writer.WriteElementString("Throttle", Values[3]);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

    }
}