using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace VRepClient

{/*
    public class TcpConnection
    {
        public TcpClient tc;
        private StreamReader reader;
        private StreamWriter writer;
        public Thread tc_thread;
        private const string confirmation = "#ok#";
        private const string time_check = "#time_check#";
        public delegate void EventDel(string info);
        private EventDel onConnected, onDataReceived, onDisconnect;
        private System.Windows.Forms.Timer timer_keep_alive;

        public bool IsConnected()
        {
            if (tc == null || tc_thread == null) return false;

            bool res = (DateTime.Now - t_last_msg_from_server).Seconds < 60;
            return res;
        }

        private Form1 f1;
        int ID;

        public TcpConnection(int ID, Form1 f1, EventDel onConnected, EventDel onDataReceived, EventDel onDisconnect)
        {
            this.ID = ID;
            this.f1 = f1;
            this.onConnected = onConnected;
            this.onDataReceived = onDataReceived;
            this.onDisconnect = onDisconnect;
        }

        public void Dispose()
        {

            if (tc_thread != null)
            {
                tc_thread.Abort();
                tc_thread = null;
            }
            if (reader != null) { reader.Dispose(); reader = null; }
            if (writer != null) { writer.Dispose(); writer = null; }
        }

        public bool IsDisposed { get { return tc_thread == null; } }

        public void Connect(string ip, string port)
        {
            tc = new TcpClient();
            var serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));

            if (tc.Client.RemoteEndPoint == null || tc.Client.RemoteEndPoint.ToString() != serverEndPoint.ToString())
            {
                try
                {
#warning limitar la duración de un intento de conexión
                    tc.Connect(serverEndPoint);
                }
                catch (Exception)
                {
                    MessageBox.Show("Bad endpoint: ID = " + (ID + 1));
                    tc = null;
                    return;
                }

                reader = new StreamReader(tc.GetStream());
                writer = new StreamWriter(tc.GetStream());

                tc_thread = new Thread(WaitTcpFromServer);
                tc_thread.Start();
            }

            t_last_msg_from_server = DateTime.Now;

            if (!IsConnected()) throw new Exception("Couldn't connect");
            if (onConnected != null) onConnected("Connected!");
            Send("#start#");

            timer_keep_alive = new System.Windows.Forms.Timer();
            timer_keep_alive.Interval = 1000;
            timer_keep_alive.Enabled = true;
            timer_keep_alive.Tick += (s, e) =>
            {
                Send(confirmation);
                if (tcs == 0)
                {
                    hpt.Start();
                    Send(time_check);
                }
                tcs = (tcs + 1) % time_check_skipper;
            };
        }

        int time_check_skipper = 5, tcs = 0;

        public void Disconnect(string reason, bool show_mb)
        {
            timer_keep_alive.Enabled = false;

            if (tc_thread != null && tc_thread.IsAlive)
            {
                try
                {
                    tc_thread.Abort();
                }
                catch
                {
                }
            }
            if (tc != null && tc.Connected)
            {
                tc.Close();
                reader.Close(); reader = null;
                writer.Close(); writer = null;
            }

            tc = null;
            tc_thread = null;

            if (onDisconnect != null) onDisconnect(reason);

            if (show_mb) MessageBox.Show("Disconnect: " + reason);
        }

        private object read_write_sem = 777;

        private DateTime t_last_msg_from_server;

        private void WaitTcpFromServer()
        {
            //read

            while (true)
            {
                if (!IsConnected())
                {
                    Disconnect("Connection timeout or smth else", true);
                    break;
                }

                string data = null;
                lock (read_write_sem)
                {
                    if (tc.Available > 0)
                    {
                        try
                        {
                            data = reader.ReadLine();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                }
                if (data != null)
                {
                    // Una cadena que contiene la respuesta del servidor.
                    if (data == confirmation)
                    {
                        t_last_msg_from_server = DateTime.Now;
                    }
                    else if (data == time_check)
                    {
                        hpt.Stop();
                        delay = 0.5f * delay + 0.5f * (float)hpt.Duration;
                    }
                    else
                    {
                        if (onDataReceived != null) onDataReceived(data.Replace("^^^", "\r\n"));
                    }
                }
                else
                {
                    Thread.Sleep(30);
                }
            }
        }

        public float delay;
        HiPerfTimer hpt = new HiPerfTimer();

        private bool msg_box_shown = false;

        public void Send(string s)
        {
            if (tc == null)
            {
                msg_box_shown = true;
                if (!msg_box_shown) MessageBox.Show("Not connected");
                return;
            }

            lock (read_write_sem)
            {
                try
                {
                    s = s.Replace("\r\n", "\n");
                    writer.Write(s + "^^^");
                    writer.Flush();
                }
                catch
                {
                    Disconnect("Can't send (server stopped?): ID = " + (ID + 1), true);
                }
            }
        }

    }
    #region HiPerfTimer
    public class HiPerfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long startTime, stopTime;
        private long freq;

        // Constructor
        public HiPerfTimer()
        {
            startTime = 0; stopTime = 0;
            if (QueryPerformanceFrequency(out freq) == false)
            {
                // high-performance counter not supported
                throw new System.ComponentModel.Win32Exception();
            }
        }

        // Start the timer
        public void Start()
        {
            // lets do the waiting threads their work
            Thread.Sleep(0);
            QueryPerformanceCounter(out startTime);
        }

        // Stop the timer
        public void Stop()
        {
            QueryPerformanceCounter(out stopTime);
        }

        // Returns the duration of the timer (in seconds)
        public double Duration
        { get { return (double)(stopTime - startTime) / (double)freq; } }
    }
    #endregion

    */
   
}