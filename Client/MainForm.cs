using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var checkedItems = checkedListBox1.CheckedItems;

            foreach (var serverIp in checkedItems)
            {
                Task.Run(async () =>
                {
                    int port = 5000;
                    using (TcpClient client = new TcpClient(serverIp.ToString(), port))
                    using (NetworkStream stream = client.GetStream())
                    {
                        Console.Write("Inserisci il messaggio da inviare: ");
                        string message = "test";

                        var msg = await SendMessageAsync(serverIp.ToString(), port, message, client, stream);
                        AppendTextToRichTextBox(msg + "-");
                    }
                });
            }
        }

        private static async Task<string> SendMessageAsync(string serverIp, int port, string message, TcpClient client, NetworkStream stream)
        {
            try
            {
                // Send the message to the server
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(buffer, 0, buffer.Length);
                Console.WriteLine("Messaggio inviato!");

                // Buffer for reading the response
                byte[] responseBuffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

                // Display the response
                Console.WriteLine("Risposta dal server: " + response);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore: " + ex.Message);
                return "Errore: " + ex.Message;
            }
        }

        private void AppendTextToRichTextBox(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendTextToRichTextBox), new object[] { text });
                return;
            }

            richTextBox1.Text += text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test.csv");
            var text = richTextBox1.Text;
            CreateCsvFile(filePath, text);

            if (File.Exists(filePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = "test.csv",
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    DefaultExt = "csv",
                    AddExtension = true
                };

                File.Copy(filePath, saveFileDialog.FileName, true);
                MessageBox.Show($"CSV file downloaded to: {saveFileDialog.FileName}");

            }
            else
            {
                MessageBox.Show("CSV file does not exist. Please create it first.");
            }
        }

        private void CreateCsvFile(string filePath, string text)
        {
            StringBuilder csvContent = new StringBuilder();

            var array = text.Split('-');

            foreach( var item in array )
            {
                csvContent.AppendLine(item);
            }


            File.WriteAllText(filePath, csvContent.ToString());
        }
    }
}
