using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Camera;
using Motion;
using Serial;

namespace CameraControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MotionController _motion;
        bool _cuntion;
        CameraAdapter _cameraLeft;
        CameraAdapter _cameraRight;

        public delegate void UpdateCurrentRotateCallback(int rotate);
        public delegate void UpdateFrameCallback(Bitmap frame);

        public MainWindow()
        {
            InitializeComponent();
            _cuntion = true;
            var serial = new SerialAdapter("COM3");
            _motion = new MotionController(serial);
            _cameraLeft = new CameraAdapter(1, CameraAdapter.Orientation.Orientation90);
            _cameraRight = new CameraAdapter(2, CameraAdapter.Orientation.Orientation90);

            _cameraLeft.Start();
            _cameraRight.Start();

            StartRotateUpdateThread();
            StartCameraLeftUpdateThread();
            StartCameraRightUpdateThread();

            CalibrateMotors();
            CalibrateCameras();
        }

        void CalibrateMotors()
        {
            var rotateDefault = 50;
            var tiltDefault = 2500;
            RotateSlider.Value = rotateDefault;
            TiltSlider.Value = tiltDefault;
            _motion.SetRotate(rotateDefault);
            _motion.SetTilt(tiltDefault);
        }

        void CalibrateCameras()
        {
            var exposure = _cameraLeft.GetExposure();
            ExposureSlider.Value = exposure;
            ExposureValue.Content = $"{exposure:0.00}";
            _cameraRight.SetExposure(exposure);

            var gain = _cameraLeft.GetGain();
            GainSlider.Value = gain;
            GainValue.Content = $"{gain:0.00}";
            _cameraRight.SetGain(gain);
        }

        void StartRotateUpdateThread()
        {
            Task.Run(() =>
            {
                while (_cuntion)
                {
                    var currentRotate = _motion.GetCurrentRotate();
                    RotateCurrentValue.Dispatcher.Invoke(new UpdateCurrentRotateCallback(UpdateCurrentRotate), currentRotate);
                    Thread.Sleep(100);
                }
            });
        }


        void UpdateCurrentRotate(int rotate)
        {
            RotateCurrentValue.Content = rotate;
        }

        void StartCameraLeftUpdateThread()
        {
            Task.Run(() =>
            {
                while (_cuntion)
                {
                    var frame = _cameraLeft.GetFrame();
                    if (frame != null)
                    {
                        CameraLeft.Dispatcher.Invoke(new UpdateFrameCallback(UpdateLeftFrame), frame.Clone());
                    }
                    Thread.Sleep(100);
                }
            });
        }

        void UpdateLeftFrame(Bitmap frame)
        {
            CameraLeft.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(frame.GetHbitmap(),
                IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(frame.Width, frame.Height));
        }

        void StartCameraRightUpdateThread()
        {
            Task.Run(() =>
            {
                while (_cuntion)
                {
                    var frame = _cameraRight.GetFrame();
                    if (frame != null)
                    {
                        CameraRight.Dispatcher.Invoke(new UpdateFrameCallback(UpdateRightFrame), frame.Clone());
                    }
                    Thread.Sleep(100);
                }
            });
        }

        void UpdateRightFrame(Bitmap frame)
        {
            CameraRight.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(frame.GetHbitmap(),
                IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(frame.Width, frame.Height));
        }

        void RotateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (RotateValue != null)
            {
                var rotate = Convert.ToInt32(e.NewValue);
                RotateValue.Content = rotate;
                _motion.SetRotate(rotate);
            }
        }

        void TiltSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TiltValue != null)
            {
                var tilt = Convert.ToInt32(e.NewValue);
                TiltValue.Content = tilt;
                _motion.SetTilt(tilt);
            }
        }

        void ExposureSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ExposureValue != null)
            {
                var exposure = e.NewValue;
                ExposureValue.Content = $"{exposure:0.00}";
                _cameraLeft.SetExposure(exposure);
                _cameraRight.SetExposure(exposure);
            }
        }

        void GainSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (GainValue != null)
            {
                var gain = e.NewValue;
                GainValue.Content = $"{gain:0.00}";
                _cameraLeft.SetGain(gain);
                _cameraRight.SetGain(gain);
            }
        }
    }
}
