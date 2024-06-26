using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PopupServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 5000;
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();

            Console.WriteLine($"Server in ascolto sulla porta {port}...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                //ThreadPool.QueueUserWorkItem(HandleClient, client);
                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Ricevuto: " + message);

                // Mostra il popup e ottieni la risposta
                string response = ShowPopup(message);

                // Invia la risposta al client
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);

                stream.Close();
                client.Close();

            }
        }

        //di può tenere handle client?
        private static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Ricevuto: " + message);

            ShowPopup(message);
            byte[] response = Encoding.ASCII.GetBytes("Messaggio ricevuto");
            stream.Write(response, 0, response.Length);

            stream.Close();
            client.Close();
        }

        private static string ShowPopup(string message)
        {
            // Mostra un MessageBox per visualizzare il messaggio e chiedere una risposta
            string response = Microsoft.VisualBasic.Interaction.InputBox(message, "Rispondi al messaggio", "");

            return response;
        }
    }
}
