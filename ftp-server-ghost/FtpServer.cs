using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ftp_server_ghost
{
    class FtpServer
    {

        private TcpListener _listener;

        public FtpServer()
        {

        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, 21);
            _listener.Start(); //starting TcpListener to listen for conn
            _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener); //handling conn
        }
        public void Stop()
        {
            if (_listener != null)
            {
                _listener.Stop();
            }
        }

        public void HandleAcceptTcpClient(IAsyncResult asyncResult)
        {
            //returning reference to tcpClient for further communication
            TcpClient tcpClient = _listener.EndAcceptTcpClient(asyncResult);

            //keep listening for more connections
            _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);

            ClientConnection conn = new ClientConnection(tcpClient);
            ThreadPool.QueueUserWorkItem(conn.HandleClient, tcpClient);




            /*
              provide stream of data for network access
            NetworkStream stream = tcpClient.GetStream();

            //unmanaged resources
            using(StreamWriter writer = new StreamWriter(stream, Encoding.ASCII))
            using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
            {
                call flush, to actually send it to the client,
                  otherwise it stays in local buffer
                

                writer.WriteLine("Connection established");
                writer.Flush();
                writer.WriteLine("Send a message and I'll echo it!");
                writer.Flush();

                string line = null;

                while(!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    writer.WriteLine("You sent: {0}", line);
                    writer.Flush();
                }
                */

        }


        public static void Main(string[] args)
        {
           
            FtpServer ftpServer = new FtpServer();

            while (true)
            {
                Console.WriteLine("Hello! Press 1 to open connection. 2 to exit");
                string option = Console.ReadLine();

                if (option.Equals("1"))
                {
                    Console.WriteLine("\nYou seelected 1!\n");

                    ftpServer.Start();
                }
                else if (option.Equals("2"))
                {
                    ftpServer.Stop();
                    System.Environment.Exit(1);
                }
            }

           

        }

    }
}

