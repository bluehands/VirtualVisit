using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motion;
using Serial;
using static System.Int32;

namespace MotionConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var serial = new SerialAdapter("COM3");
            var motion = new MotionController(serial);

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
                    var parts = command?.Split(',');
                    if (parts?.Length == 2)
                    {
                        var rotate = Parse(parts[0]);
                        var tilt = Parse(parts[1]);
                        Console.WriteLine($"Move to rotate: {rotate} and tilt: {tilt}.");
                        motion.Move(rotate, tilt);
                        while (motion.IsMoving())
                        {
                        }
                        Console.WriteLine($"Finished moving.");
                    }
                }
            }
            motion.Dispose();
            
        }
    }
}
