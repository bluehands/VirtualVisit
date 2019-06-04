using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;

namespace Camera
{
    public interface IImageReceiver
    {
        Bitmap GetFrame();
    }
}
