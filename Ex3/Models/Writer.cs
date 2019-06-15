using System;
using System.Diagnostics.Tracing;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;


namespace Ex3.Models
{
    public class Writer
    {

        public int port;
        Socket socket;
        public string ip;
        public string longi;
        public string latitu;
        public IPAddress localAddr;
        public TcpClient client;
        /// <summary>
        /// getter and setter for ip
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
            get => port;
            set => port = value;
        }
        /// <summary>
        /// gette and setter for lon
        /// </summary>
        public double Lon
        {
            get;
            set;
        }
        /// <summary>
        /// getter and setter for lat.
        /// </summary>
        public double Lat
        {
            get;
            set;
        }
        /// <summary>
        /// make this class a singlton.
        /// </summary>
        private static volatile Writer Instance;

        private Writer() { }

        public static Writer getInstance()
        {
            if (Instance == null)
            {
                Instance = new Writer();
            }
            return Instance;
        }

        /// <summary>
        ///     connect.
        /// </summary>
        public void connect()
        {
            IPAddress ipTo = IPAddress.Parse(Ip);
            IPEndPoint ipe = new IPEndPoint(ipTo, Port);
            socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);
        }
        /// <summary>
        /// server socket getter ans setter.
        /// </summary>
        public TcpListener serverSocket { get; set; }
        /// <summary>
        /// clienSocket getter and setter.
        /// </summary>
        public TcpClient clientSocket { get; set; }
   

        /// <summary>
        /// write to the server.
        /// </summary>
        /// <param name="message"></param>
        public double write(string msg)
        {

            string toWrite = CreateUpdatedMessage(msg);
            socket.Send(System.Text.Encoding.ASCII.GetBytes(toWrite));
            Byte[] bytesReceived = new Byte[5000];
            int iR = socket.Receive(bytesReceived);
            string re = Encoding.ASCII.GetString(bytesReceived, 0, iR);
            return createLonAndLat(re);

        }
        /// <summary>
        /// this dosconnects from the server.
        /// </summary>
        public void disconnect()
        {
            socket.Close();
        }


        /// <summary>
        /// this creats the updated message ready for sending.
        /// </summary>
        /// <param name="message">the message to prepare.</param>
        /// <returns> the prepared message with \r\n</returns>
        public string CreateUpdatedMessage(string message)
        {
            if ((message[message.Length - 2] == '\r') && (message[message.Length - 1] == '\n'))
            {
                return message;
            }
            if (message[message.Length - 1] == '\n')
            {
                message = message.Remove(message.Length - 1);
            }
            message += "\r\n";
            return message;
        }

        /// <summary>
        /// this creates the "clean"number from the string given by the server. is only leavs the decimal number
        /// </summary>
        /// <param name="str">the string fro the server to extract the number</param>
        /// <returns>the extracted number.</returns>
        public double createLonAndLat(string str)
        {
            var doubleArray = Regex.Matches(str, @"(?'number'\d*\.\d*)|(?'number'\d+[^\.])");
            string num = Regex.Match(str, @"'(.*?[^\\])'").Value;
            num = num.Trim('\'');
            return Double.Parse(num);
        }
    }
    }