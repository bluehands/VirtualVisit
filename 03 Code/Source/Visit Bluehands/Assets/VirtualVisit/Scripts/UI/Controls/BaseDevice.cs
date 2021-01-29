using UnityEngine;

abstract class BaseDevice : IMobileDevice
{
    protected Gyroscope m_Gyroscope;

    protected Compass m_Compass;

    protected SensorFusion m_SensorFusion;

    public BaseDevice(float samplePeriod)
    {
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        m_Gyroscope = Input.gyro;
        m_Compass = Input.compass;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        m_SensorFusion = new SensorFusion(samplePeriod, 1f);
    }

    public virtual Vector3 Gyro => m_Gyroscope.rotationRateUnbiased;

    public Vector3 Acceleration => Input.acceleration;

    public Vector3 Compass => m_Compass.rawVector;

    public float SampleRate => m_SensorFusion.getSampleRate();

    public int WidthPixels()
    {
        return Screen.currentResolution.width;
    }

    public int HeightPixels()
    {
        return Screen.currentResolution.height;
    }

    public Quaternion Orientation
    {
        get 
        {
            return Quaternion.Euler(-90, 0, 0) * m_SensorFusion.process(Gyro, Acceleration) * Quaternion.Euler(0, 0, 180);
        }
    }
}

