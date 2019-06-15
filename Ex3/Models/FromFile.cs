using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.IO;

namespace Ex3.Models
{
    public class FromFile
    {
        public const string pathFile = "~/App_Data/{0}.txt";

        private Queue<string> locations;
        private string fileName;



        /// <summary>
        /// create this class as singlton
        /// </summary>
        #region Singleton
        private static FromFile infoFileModel = null;
        public static FromFile Instance
        {
            get
            {
                if (infoFileModel == null)
                {
                    infoFileModel = new FromFile();
               
                }
                return infoFileModel;
            }
        }
        #endregion
        /// <summary>
        /// this initializes the new queue
        /// </summary>
        private FromFile()
        {
 
            locations = new Queue<string>();

        }
        /// <summary>
        /// info getter and setter
        /// </summary>
        public Queue<string> Information
        {
            get { return locations; }
            set
            {
                locations = value;
           
            }
        }


        /// <summary>
        /// filename getter and setter
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
            }
        }


        /// <summary>
        /// this reads the file into a queue and inserts it.
        /// </summary>
        public void Read()
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(pathFile, FileName));
            if (File.Exists(path))
            {
                string[] lines = System.IO.File.ReadAllLines(path);        // reading all the lines of the file

                for (int i = 0; i < lines.Length; i++)
                {
                    locations.Enqueue(lines[i]);
                }
            }
        }

        /// <summary>
        /// this gets a single line from the queue
        /// </summary>
        /// <returns> a single line</returns>
        public string Get()
        {
            if (locations.Count > 0)
            {
                return locations.Dequeue();
            }
            return null;
        }

    }
}