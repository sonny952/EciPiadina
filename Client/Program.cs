using Client;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace PopupClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Run(new MainForm());

            //string serverIp = "192.168.168.38"; // IP del computer server
            //int port = 5000;

            //Console.Write("Inserisci il messaggio da inviare: ");
            //string message = Console.ReadLine();

            //SendMessage(serverIp, port, message);

            //TcpClient client = new TcpClient("192.168.168.38", 5000);
            //NetworkStream stream = client.GetStream();

            //string message = "Ciao, server!";
            //byte[] data = Encoding.ASCII.GetBytes(message);
            //stream.Write(data, 0, data.Length);

            //byte[] buffer = new byte[1024];
            //int bytesRead = stream.Read(buffer, 0, buffer.Length);
            //string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //Console.WriteLine("Risposta: " + response);

            //stream.Close();
            //client.Close();

        }

        private static void SendMessage(string serverIp, int port, string message)
        {
            TcpClient client = new TcpClient(serverIp, port);
            NetworkStream stream = client.GetStream();

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);

            stream.Close();
            client.Close();

            Console.WriteLine("Messaggio inviato!");
        }
    }
}
