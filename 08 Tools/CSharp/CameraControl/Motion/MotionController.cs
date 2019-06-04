using System;
using System.Threading;
using System.Threading.Tasks;
using Serial;

namespace Motion
{
    public class MotionController
    {
        readonly ISerialAdapter _serialAdapter;

        int _rotate;
        int _tilt = 2500;

        public MotionController(ISerialAdapter serialAdapter)
        {
            _serialAdapter = serialAdapter;
            _serialAdapter.Open();
        }

        public void Move(int rotate, int tilt)
        {
            SetRotate(rotate);
            SetTilt(tilt);
        }

        public void SetRotate(int rotate)
        {
            if (0 <= rotate && rotate <= 250)
            {
                _rotate = rotate;
                _serialAdapter.Send($"a{rotate}");
            }
        }

        public void SetTilt(int tilt)
        {
            if (1000 <= tilt && tilt <= 4500)
            {
                _tilt = tilt;
                _serialAdapter.Send($"b{_tilt}");
                //if (_tilt < tilt)
                //{
                //    Task.Run(() =>
                //    {
                //        while (_tilt < tilt)
                //        {
                //            _tilt += 100;
                //            _serialAdapter.Send($"b{_tilt}");
                //            Thread.Sleep(100);
                //        }
                        
                //    });
                //}
                //else
                //{
                //    Task.Run(() =>
                //    {
                //        while (_tilt > tilt)
                //        {
                //            _tilt -= 100;
                //            _serialAdapter.Send($"b{_tilt}");
                //            Thread.Sleep(100);
                //        }

                //    });
                //}
            }
        }

        public bool IsMoving()
        {
            var currentRotate = GetCurrentRotate();
            return _rotate != currentRotate;
        }

        public int GetCurrentRotate()
        {
            _serialAdapter.Send("e");
            var line = _serialAdapter.Read();

            if (!string.IsNullOrEmpty(line) && line[0].Equals('f'))
            {
                var valueStr = line.Substring(1);
                return int.Parse(valueStr);
            }

            return 0;
        }

        public void Dispose()
        {
            _serialAdapter.Dispose();
        }
    }
}
