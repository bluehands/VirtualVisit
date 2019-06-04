using System;
using System.Drawing;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Camera
{
    public class CameraAdapter
    {
        public enum Orientation
        {
            Orientation0,
            Orientation90,
            Orientation180,
            Orientation270
        }

        static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        readonly Orientation _orientation;
        readonly int _cameraId;
        VideoCapture _camera;
        Mat _copyFrame;
        Mat _frame;
        public CameraAdapter(int cameraId, Orientation orientation)
        {
            _cameraId = cameraId;
            _orientation = orientation;
            Init();
        }

        void Init()
        {
            _camera = new VideoCapture(_cameraId);
            _camera.ImageGrabbed += ProcessFrame;
            _copyFrame = new Mat();
            _frame = new Mat();
        }

        public void Start()
        {
            _camera.Start();
            Thread.Sleep(2);
            var message = _camera.IsOpened
                ? $"Camera with id: {_cameraId} is opened."
                : $"Camera with id: {_cameraId} failed to open.";

            Console.WriteLine(message);
            Log.Info(message);
        }

        void SetConfig()
        {
            _camera.SetCaptureProperty(CapProp.FrameWidth, 1920);
            _camera.SetCaptureProperty(CapProp.FrameHeight, 1080);
            //_camera.SetCaptureProperty(CapProp.FrameWidth, 1280);
            //_camera.SetCaptureProperty(CapProp.FrameHeight, 960);
            _camera.SetCaptureProperty(CapProp.Brightness, 125);
            _camera.SetCaptureProperty(CapProp.Contrast, 25);
            _camera.SetCaptureProperty(CapProp.Saturation, 50);
            _camera.SetCaptureProperty(CapProp.Gain, 0);
            _camera.SetCaptureProperty(CapProp.Backlight, 0);
        }

        public void Stop()
        {
            if (_camera.IsOpened)
            {
                _camera.Stop();
                _camera.Dispose();

                var message = $"Camera with id: {_cameraId} closed.";
                Console.WriteLine(message);
                Log.Info(message);
            }
        }

        void ProcessFrame(object sender, EventArgs arg)
        {
            if (_camera != null && _camera.Ptr != IntPtr.Zero)
            {
                _camera.Retrieve(_copyFrame, 0);
                _frame = Flip(_copyFrame).Clone();
            }
        }

        Mat Flip(Mat mat)
        {
            switch (_orientation)
            {
                case Orientation.Orientation0:
                    break;
                case Orientation.Orientation90:
                    CvInvoke.Rotate(mat, mat, RotateFlags.Rotate90Clockwise);
                    break;
                case Orientation.Orientation180:
                    CvInvoke.Rotate(mat, mat, RotateFlags.Rotate180);
                    break;
                case Orientation.Orientation270:
                    CvInvoke.Rotate(mat, mat, RotateFlags.Rotate90CounterClockwise);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return mat;
        }

        public Bitmap GetFrame()
        {
            return _frame.Bitmap;
        }

        public double GetExposure()
        {
            return _camera.GetCaptureProperty(CapProp.Exposure) * -1.0;
        }

        public void SetExposure(double exposure)
        {
            _camera.SetCaptureProperty(CapProp.Exposure, exposure * -1.0);
        }

        public double GetGain()
        {
            return _camera.GetCaptureProperty(CapProp.Gain);
        }

        public void SetGain(double gain)
        {
            _camera.SetCaptureProperty(CapProp.Gain, gain);
        }
    }
}
