using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ftp_server_ghost
{
    class ClientConnection
    {

        private TcpClient _controlClient;

        private NetworkStream _controlStream;
        private StreamReader _controlReader;
        private StreamWriter _controlWriter;

        private string _username;

        public ClientConnection(TcpClient client)
        {
            _controlClient = client;
            _controlStream = _controlClient.GetStream();
            _controlReader = new StreamReader(_controlStream);
            _controlWriter = new StreamWriter(_controlStream);
        }

        public void HandleClient(object obj)
        {
            _controlWriter.WriteLine("220 Connection established.");
            _controlWriter.Flush();

            string line;

            try
            {
                while(!string.IsNullOrEmpty(line = _controlReader.ReadLine()))
                {
                    string response = null;
                    string arguments = null;

                    //Splitting input to array
                    string[] command = line.Split(' ');
               
                    string cmd = command[0].ToUpperInvariant();

                    //If input has arguments, save them all to *arguments*, starting after the command
                    //first word length + 1 for space
                    arguments = command.Length > 1 ? line.Substring(command[0].Length + 1) : null;
                    
                    switch(cmd)
                    {
                        case "USER":
                            response = User(arguments);
                            break;
                        case "PASS":
                            response = Password(arguments);
                            break;
                        case "PWD":
                            response = "257 ../ is current directory.";
                            break;

                        default:
                            response = "502 Command not implemented";
                            break;
                    }

                    if (_controlClient == null || !_controlClient.Connected)
                    {
                        break;
                    }
                    else
                    {
                        _controlWriter.WriteLine(response);
                        _controlWriter.Flush();
                    }
                }       
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }


        }

        public string User(string username)
        {
            _username = username;
            return "331 Username ok, need password";
        }

        private string Password(string password)
        {
            if (true)
            {
                return "230 User logged in";
            }
            else
            {
                return "530 Not logged in";
            }
        }

    }
}
