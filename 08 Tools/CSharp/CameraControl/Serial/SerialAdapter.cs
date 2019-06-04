using System;
using System.IO;
using System.IO.Ports;
using static System.String;

namespace Serial
{
    public class SerialAdapter : ISerialAdapter
    {
        static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        SerialPort _serialPort;

        public SerialAdapter(string portName)
        {
            Init(portName, false);
        }

        void Init(string portName, bool useUserConfiguration)
        {
            _serialPort = new SerialPort();

            if (useUserConfiguration)
            {
                AskUserForConfiguration();
            }
            else
            {
                SetDefaultConfiguration(portName);
            }

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
        }

        public void Open()
        {
            try
            {
                _serialPort.Open();
                var message = $"Serial on port: {_serialPort.PortName} opened.";
                Console.WriteLine(message);
                Log.Info(message);
            }
            catch (IOException e)
            {
                var message = $"Can't open serial on port: {_serialPort.PortName}.";
                Console.WriteLine(message);
                Log.Info(message);
            }
            
        }

        void AskUserForConfiguration()
        {
            _serialPort.PortName = SetPortName(_serialPort.PortName);
            _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
            _serialPort.Parity = SetPortParity(_serialPort.Parity);
            _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
            _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
            _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);
        }

        void SetDefaultConfiguration(string portName)
        {
            _serialPort.PortName = portName;
        }

        public void Send(string command)
        {
            if (_serialPort.IsOpen)
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                var bytes = enc.GetBytes(command + "\0");
                _serialPort.Write(bytes, 0, bytes.Length);
            }
        }

        public string Read()
        {
            return ReadLine();
        }

        string ReadLine()
        {
            string readLine = Empty;
            if (_serialPort.IsOpen)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int index = 0;
                    do
                    {
                        buffer[index] = Convert.ToByte(_serialPort.ReadByte());
                    } while (buffer[index++] != 0);

                    readLine = System.Text.Encoding.Default.GetString(buffer, 0, index - 1);
                }
                catch (TimeoutException)
                {
                }
            }

            return readLine;
        }

        public void Dispose()
        {
            _serialPort.Close();
        }

        string SetPortName(string defaultPortName)
        {
            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            var portName = Console.ReadLine();

            if (IsNullOrEmpty(portName) || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }
        int SetPortBaudRate(int defaultPortBaudRate)
        {
            Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
            var baudRate = Console.ReadLine();

            if (IsNullOrEmpty(baudRate))
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        Parity SetPortParity(Parity defaultPortParity)
        {
            Console.WriteLine("Available Parity options:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString());
            var parity = Console.ReadLine();

            if (IsNullOrEmpty(parity))
            {
                parity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parity, true);
        }

        int SetPortDataBits(int defaultPortDataBits)
        {
            Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
            var dataBits = Console.ReadLine();

            if (IsNullOrEmpty(dataBits))
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits.ToUpperInvariant());
        }

        StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            Console.WriteLine("Available StopBits options:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter StopBits value (None is not supported and \n" +
             "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());

            var stopBits = Console.ReadLine();
            if (IsNullOrEmpty(stopBits))
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
        }

        Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            Console.WriteLine("Available Handshake options:");
            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter Handshake value (Default: {0}):", defaultPortHandshake.ToString());

            var handshake = Console.ReadLine();
            if (IsNullOrEmpty(handshake))
            {
                handshake = defaultPortHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
        }
    }
}
