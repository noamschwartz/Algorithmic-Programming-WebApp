
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class SaveInfo
    {
        public const string filePath = "~/App_Data/{0}.txt";
        private double _Lat;
        private double _Lon;
        private string _ip;
        private int _Port;
        private double _throttle;
        private double _rudder;
        private string _fileName;

        /// <summary>
        /// create this class as a singleton
        /// </summary>
        #region Singleton
        private static SaveInfo _flightValues = null;
        public static SaveInfo Instance
        {
            get
            {
                if (_flightValues == null)
                {
                    _flightValues = new SaveInfo();
                }
                return _flightValues;
            }
        }
        #endregion
        /// <summary>
        /// This initializes the info
        /// </summary>
        private SaveInfo()
        {
            _fileName = null;
            _Port = 5402;
            _throttle = 0;
            _rudder = 0;
            _Lat = 0;
            _Lon = 0;
            _ip = "127.0.0.1";

        }
        /// <summary>
        /// this saves the info to the file.
        /// </summary>
        public void SaveToFile()
        {
            if (FileName != null)
            {
                string path = HttpContext.Current.Server.MapPath(String.Format(filePath, FileName));
                string line = (Lat.ToString() + "," + Lon.ToString() + "," + Throttle.ToString() + "," + Rudder.ToString());
                using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(line);
                }
            }

        }

        /// <summary>
        /// this asks the server for the specific atribute.
        /// </summary>
        public void SampleValues()
        {

            Writer writer = Writer.getInstance();
            Lat = writer.write("/position/latitude-deg");
            Lon = writer.write("/position/longitude-deg");
            Rudder = writer.write("/controls/flight/rudder");
            Throttle = writer.write("/controls/engines/current-engine/throttle");

        }
        /// <summary>
        /// this creates the xml from the values.
        /// </summary>
        /// <param name="writer"></param>
        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Values");
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Rudder", this._rudder.ToString());
            writer.WriteElementString("Throttle", this._throttle.ToString());
            writer.WriteEndElement();
        }
        /// <summary>
        /// getter and setter for the file name.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        /// <summary>
        /// getter and setter for the port.
        /// </summary>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
        /// <summary>
        /// getter and setter for lon
        /// </summary>
        public double Lon
        {
            get { return _Lon; }
            set { _Lon = value; }
        }
        /// <summary>
        /// getter and setter for lat
        /// </summary>
        public double Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }
        /// <summary>
        /// getter and setter for the ruddder.
        /// </summary>
        public double Rudder
        {
            get { return _rudder; }
            set { _rudder = value; }
        }
        /// <summary>
        /// getter and seter for throttle
        /// </summary>
        public double Throttle
        {
            get { return _throttle; }
            set { _throttle = value; }
        }
        /// <summary>
        /// getter and setter for the ip.
        /// </summary>

        public string ip
        {
            get { return ip; }
            set { _ip = value; }
        }

    }
}