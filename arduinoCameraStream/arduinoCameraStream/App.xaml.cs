﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace arduinoCameraStream
{
    public partial class App : Application
    {
        static SerialPort _serialPort = new SerialPort();
        static public List<String> LDeviceId = new List<String>();
        static public int deviceIdIndex = -1;
        static public Canvas iCanvas;
        static string serialBuffer = "";
        static string frameBuffer = "";
        static string imageBuffer = "";
        static public ProgressBar hSerialProgress;
        static public TextBlock hSerialProgressText;
        static int totalFrameLength = 640 * 480 + "*FRAME_START*".Length + "*FRAME_STOP*".Length;
        static public void openSerial()
        {
            if (deviceIdIndex == -1)
                return;

            _serialPort.PortName = LDeviceId[deviceIdIndex];
            _serialPort.BaudRate = 1000000; //250000
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.DtrEnable = true;
            _serialPort.RtsEnable = true;
            _serialPort.NewLine = "\r\n";

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            _serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(ErrorReceivedHandler);
            _serialPort.Open();
        }
        public static void setPixel(WriteableBitmap wbm, int x, int y, Color c)
        {
             if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1) return;
             if (y < 0 || x < 0) return;
             if (!wbm.Format.Equals(PixelFormats.Bgra32))return;
             wbm.Lock();
             IntPtr buff = wbm.BackBuffer;
             int Stride = wbm.BackBufferStride;
             unsafe
             {
                  byte* pbuff = (byte*)buff.ToPointer();
                  int loc= y * Stride  + x*4;
                  pbuff[ loc]=c.B;
                  pbuff[loc+1]=c.G;
                  pbuff[loc+2]=c.R;
                  pbuff[loc+3]=c.A;
             }
             wbm.AddDirtyRect(new Int32Rect(x,y,1,1));
             wbm.Unlock();
        }
        static public void openSerialHandler(object sender, RoutedEventArgs e)
        {
            openSerial();

            /*List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
            colors.Add(System.Windows.Media.Colors.Red);
            colors.Add(System.Windows.Media.Colors.Blue);
            colors.Add(System.Windows.Media.Colors.Green);
            BitmapPalette myPalette = new BitmapPalette(colors);
            WriteableBitmap writeableBmp = new WriteableBitmap(640, 480, 72, 72, PixelFormats.Bgra32, myPalette);

            int z = 0;
            for (int y = 0; y < 480; y++)
            {
                for (int x = 0; x < 640; x++)
                {
                    setPixel(writeableBmp, x, y, Color.FromArgb(255, 255, 255, 255));
                    z += 3;
                }
            }

            Image img = new Image();
            img.Source = writeableBmp;
            img.Width = 640;
            img.Height = 480;
            Canvas.SetLeft(img, 0);
            Canvas.SetTop(img, 0);
            iCanvas.Children.Add(img);*/
        }
        static void setProgress(int current, int total)
        {
            double percent = Math.Round(current * 100.0 / total);
            hSerialProgressText.Text = current + "/" + total + " (" + percent + "%)";
            hSerialProgress.Value = percent;
        }
        private static void receiveMainThread(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            if (sp.BytesToRead != 0)
            {
                serialBuffer += sp.ReadExisting();
                setProgress(serialBuffer.Length, totalFrameLength);
                if (serialBuffer.Length > "*FRAME_START*".Length + "*FRAME_STOP*".Length  &&
                    serialBuffer.IndexOf("*FRAME_START*") > - 1 && serialBuffer.IndexOf("*FRAME_STOP*") > -1 &&
                    serialBuffer.IndexOf("*FRAME_STOP*") > serialBuffer.IndexOf("*FRAME_START*")
                    )
                {
                    setProgress(totalFrameLength, totalFrameLength);
                    int indexFStart = serialBuffer.IndexOf("*FRAME_START*");
                    int FStartLen = "*FRAME_START*".Length;
                    int FStop = serialBuffer.IndexOf("*FRAME_STOP*");
                    int serialBufferLen = serialBuffer.Length;
                    int start = serialBuffer.IndexOf("*FRAME_START*") + "*FRAME_START*".Length;
                    int len = serialBuffer.IndexOf("*FRAME_STOP*") - serialBuffer.IndexOf("*FRAME_START*") - "*FRAME_START*".Length;
                    frameBuffer = serialBuffer.Substring(start, len);
                    serialBuffer = serialBuffer.Substring(serialBuffer.IndexOf("*FRAME_STOP*") + "*FRAME_STOP*".Length);

                    console.append("\\fs22 \\f0 \\cf0 \\par\\pard received a frame with a size of " + ((len == 640 * 480) ? "" + len : "\\cf2 \\ul\\b " + len + "\\b0\\ul0\\cf0 ") + " byte");

                    byte[] Out = new byte[640 * 480 * 3];
                    byte[] bFrameBuffer = new byte[640*480];
                    for (int i = 0; i < 640 * 480; i++)
                    {
                            bFrameBuffer[i] = (i < len)? Convert.ToByte(frameBuffer[i]) : Convert.ToByte(0);
                    }

                    decoding.deBayerHQl(bFrameBuffer, Out);

                    List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
                    colors.Add(System.Windows.Media.Colors.Red);
                    colors.Add(System.Windows.Media.Colors.Blue);
                    colors.Add(System.Windows.Media.Colors.Green);
                    BitmapPalette myPalette = new BitmapPalette(colors);
                    WriteableBitmap writeableBmp = new WriteableBitmap(640, 480, 72, 72, PixelFormats.Bgra32, myPalette);

                    int z = 0;
                    for (int y = 0; y < 480; y++)
                    {
                        for (int x = 0; x < 640; x++)
                        {
                            setPixel(writeableBmp, x, y, Color.FromArgb(255, Out[z], Out[z+1], Out[z+2]));
                            z += 3;
                        }
                    }

                    Image img = new Image();
                    img.Source = writeableBmp;
                    img.Width = 640;
                    img.Height = 480;
                    Canvas.SetLeft(img, 0);
                    Canvas.SetTop(img, 0);
                    iCanvas.Children.Add(img);
                }
            }
        }
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            var d = Application.Current.Dispatcher;

            if (d.CheckAccess())
                receiveMainThread(sender, e);
            else
                d.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => receiveMainThread(sender, e)));
        }
        private static void ErrorReceivedHandler(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.Write("ErrorReceivedHandler");
            SerialPort sp = (SerialPort)sender;
            Console.WriteLine("Data Received: " + sp.BytesToRead);
            string indata = sp.ReadExisting();
            Console.Write(indata);
        }
        static public void serialSelectorChanged(object sender, SelectionChangedEventArgs e)
        {
            var COMList = sender as ComboBox;
            //string text = COMList.SelectedItem as string;
            deviceIdIndex = COMList.SelectedIndex;
        }
    }
}
