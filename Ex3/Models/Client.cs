using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Ex3.Models
{
    public class Client
    {
        public TcpListener serverSocket { get; set; }
        public TcpClient clientSocket { get; set; }
        private int PORT;
        private string ip;
        private IPAddress localAddr;
        private Dictionary<string, double> parameters;

        /// <summary>
        /// getter and setter for the ip.
        /// </summary>
        public string Ip
        {
            get => ip;
            set => ip = value;
        }
        /// <summary>
        /// getter and setter for the port.
        /// </summary>
        public int Port
        {
            get => PORT;
            set => PORT = value;
        }

        /// <summary>
        /// the info class constructor. it creates a dictionary.
        /// </summary>
        public Client(string ip, int port)
        {
            this.PORT = port;
            this.ip = ip;

            parameters = new Dictionary<string, double>();
            parameters.Add("longitude", 0);
            parameters.Add("latitude", 0);
            parameters.Add("throttle", 0);
            parameters.Add("rudder", 0);
            StartServer();
            StartListening();
            
            

        }
        /// <summary>
        /// make this class a singleton class.
        /// </summary>
        private static volatile Client instance;
        /// <summary>
        /// get instance function for the singleton class
        /// </summary>
        /// <returns></returns>
        //public static Client GetInstance()
        //{
            //return instance ?? (instance = new Client());
        //}
        /// <summary>
        /// this function seperates the value recieved as a string from the simulator.
        /// is seperates it by ',' sign.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        double[] separateValues(string data)
        {
            double[] currentValues = new double[25];
            int i;
            string[] words = data.Split(',');
            if (words[24][words[24].Length - 1] != '\n')
            {
                return currentValues;
            }
            for (i = 0; i <= 24; i++)
            {
                currentValues[i] = Convert.ToDouble(words[i]);
            }
            return currentValues;
        }
        /// <summary>
        /// get the value of lon from the dictionary.
        /// </summary>
        /// <returns></returns>
        public double getLon()
        {
            return parameters["longitude"];
        }
        /// <summary>
        /// get the value of lat from the dictionary.
        /// </summary>
        /// <returns></returns>
        public double getLat()
        {
            return parameters["latitude"];
        }
        /// <summary>
        /// this method updated the dictionaries values according to the string received from the simulator.
        /// </summary>
        /// <param name="data"></param>
        void updateParameters(string data)
        {
            double[] prms = separateValues(data);
            if (prms.Length != 25)
            {
                return;
            }

            //_flightBoardViewModel.Lon = prms[0];
            //_flightBoardViewModel.Lat = prms[1];
            parameters["longitude"] = prms[0];
            parameters["latitude"] = prms[1];
            parameters["rudder"] = prms[21];
            parameters["throttle"] = prms[23];
        }
        /// <summary>
        /// this method starts the server.
        /// </summary>

        public void StartServer()
        {
            try
            {
                localAddr = IPAddress.Parse(ip);
                serverSocket = new TcpListener(localAddr, PORT);
                serverSocket.Start();
                Console.WriteLine(DateTime.Now.ToString() + " >> Server started");
                serverSocket.Start();
                clientSocket = serverSocket.AcceptTcpClient();
            }
            catch (Exception e)
            {
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    Console.WriteLine(DateTime.Now.ToString() + " [ERR]: Internet connection unavailable! (" +
                                      e.ToString() + ")");
                else
                    Console.WriteLine(DateTime.Now.ToString() + " [ERR]: Server can't be started! (" + e.ToString() +
                                      ")");
            }
        }
        /// <summary>
        /// this method listens to the info given from the siulator.
        /// </summary>
        public void StartListening()
        {
           // Thread th = Thread.CurrentThread;
           //th.Name = "Reader";
            {
                try
                {
                    while (true)
                    {
                        NetworkStream nwStream = clientSocket.GetStream();
                        byte[] buffer = new byte[clientSocket.ReceiveBufferSize];
                        //---read incoming stream---
                        int bytesRead = nwStream.Read(buffer, 0, clientSocket.ReceiveBufferSize);
                        //---convert the data received into a string---
                        string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        updateParameters(dataReceived);
                        System.Threading.Thread.Sleep(100);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " [ERR]: " + e.ToString() + ")");
                }
            }
        }
    }
}
