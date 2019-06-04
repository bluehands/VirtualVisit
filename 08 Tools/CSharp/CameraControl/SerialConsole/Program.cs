using System;
using Serial;

namespace SerialConsole
{
    class Program
    {
        static void Main()
        {
            var serial = new SerialAdapter("COM3");

            var isRunning = true;
            while (isRunning)
            {
                var command = Console.ReadLine();

                if (StringComparer.OrdinalIgnoreCase.Equals("quit", command))
                {
                    isRunning = false;
                }
                else
                {
                    var parts = command?.Split(':');
                    if (parts?.Length == 2)
                    {
                        var operation = parts[0];
                        var argument = parts[1].Trim();
                        if (operation.Equals("send"))
                        {
                            serial.Send(argument);
                        }
                        if (operation.Equals("read"))
                        {
                            serial.Send(argument);
                            var value = serial.Read();
                            Console.WriteLine(value);
                        }
                    }
                }
            }
            serial.Dispose();
        }
    }
}
