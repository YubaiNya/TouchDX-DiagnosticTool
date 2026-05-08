using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCDiagnosticTool
{
    public enum MaimaiBtn : int
    {
        Btn1 = 0, Btn2 = 1, Btn3 = 2, Btn4 = 3,
        Btn5 = 4, Btn6 = 5, Btn7 = 6, Btn8 = 7,
        TouchA1 = 8, TouchA2 = 9, TouchA3 = 10, TouchA4 = 11,
        TouchA5 = 12, TouchA6 = 13, TouchA7 = 14, TouchA8 = 15,
        TouchB1 = 16, TouchB2 = 17, TouchB3 = 18, TouchB4 = 19,
        TouchB5 = 20, TouchB6 = 21, TouchB7 = 22, TouchB8 = 23,
        TouchC1 = 24, TouchC2 = 25,
        TouchD1 = 26, TouchD2 = 27, TouchD3 = 28, TouchD4 = 29,
        TouchD5 = 30, TouchD6 = 31, TouchD7 = 32, TouchD8 = 33,
        TouchE1 = 34, TouchE2 = 35, TouchE3 = 36, TouchE4 = 37,
        TouchE5 = 38, TouchE6 = 39, TouchE7 = 40, TouchE8 = 41,
        Select = 42, Test = 43, Service = 44, Coin = 45
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DiagnosticForm());
        }
    }

    public class DiagnosticForm : Form
    {
        private Label lblStatus;
        private Label lblInputs;

        public DiagnosticForm()
        {
            this.Text = "TouchDX - Diagnostic Tool";
            this.Size = new Size(600, 400);
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;

            lblStatus = new Label() { Text = "Waiting for connection...", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 24) };
            lblInputs = new Label() { Text = "Raw Mask: 0\nInputs: None", Location = new Point(20, 100), AutoSize = true, Font = new Font("Consolas", 16) };

            this.Controls.Add(lblStatus);
            this.Controls.Add(lblInputs);

            Task.Run(() => StartServer());
        }

        private void StartServer()
        {
            int port = 4321;
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            try
            {
                listener.Start();
                this.Invoke((MethodInvoker)delegate { lblStatus.Text = $"Listening on port {port} (Waiting for ADB forward connection...)"; });

                while (true)
                {
                    using (TcpClient client = listener.AcceptTcpClient())
                    {
                        this.Invoke((MethodInvoker)delegate { 
                            lblStatus.Text = $"Android Client Connected: {client.Client.RemoteEndPoint}"; 
                        });

                        client.NoDelay = true;
                        
                        using (NetworkStream stream = client.GetStream())
                        {
                            byte[] handshake = System.Text.Encoding.ASCII.GetBytes("DIAG");
                            stream.Write(handshake, 0, handshake.Length);
                            stream.Flush();

                            byte[] buffer = new byte[8];
                            int bytesRead = 0;
                            ulong currentState = 0;

                            while (true)
                            {
                                try
                                {
                                    int read = stream.Read(buffer, bytesRead, 8 - bytesRead);
                                    if (read == 0) break;

                                    bytesRead += read;

                                    if (bytesRead == 8)
                                    {
                                        ulong newState = BitConverter.ToUInt64(buffer, 0);
                                        bytesRead = 0;

                                        if (newState == 0xEEEEEEEEEEEEEEEE)
                                        {
                                            stream.Write(BitConverter.GetBytes(0xEEEEEEEEEEEEEEEE), 0, 8);
                                            stream.Flush();
                                        }
                                        else if (newState != currentState)
                                        {
                                            currentState = newState;
                                            UpdateInputsUI(currentState);
                                        }
                                    }
                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }

                        this.Invoke((MethodInvoker)delegate { 
                            lblStatus.Text = "Client disconnected. Waiting for connection..."; 
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate { lblStatus.Text = $"Fatal Error: {ex.Message}"; });
            }
            finally
            {
                listener.Stop();
            }
        }

        private void UpdateInputsUI(ulong state)
        {
            string inputs = "";
            for (int i = 0; i < 64; i++)
            {
                if ((state & (1UL << i)) != 0)
                {
                    string btnName = Enum.IsDefined(typeof(MaimaiBtn), i) 
                                     ? ((MaimaiBtn)i).ToString() 
                                     : $"Bit_{i}";
                    inputs += $"[{btnName}] ";
                }
            }
            if (string.IsNullOrEmpty(inputs)) inputs = "None";

            this.Invoke((MethodInvoker)delegate {
                lblInputs.Text = $"Raw Mask: 0x{state:X16}\nInputs: {inputs}";
            });
        }
    }
}
