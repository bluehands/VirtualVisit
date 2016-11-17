using UnityEngine;

abstract class BaseDevice : MobileDevice
{
    protected Gyroscope m_Gyroscope;

    protected Compass m_Compass;

    protected SensorFusion m_SensorFusion;

    public BaseDevice(float samplePeriod, float beta)
    {
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        m_Gyroscope = Input.gyro;
        m_Compass = Input.compass;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        m_SensorFusion = new SensorFusion(samplePeriod, beta);
    }

    public virtual Vector3 getGyro()
    {
        return m_Gyroscope.rotationRateUnbiased;
    }

    public Vector3 getAcc()
    {
        return Input.acceleration;
    }

    public Vector3 getMag()
    {
        return m_Compass.rawVector;
    }

    public float getSampleRate()
    {
        return m_SensorFusion.getSampleRate();
    }

    public int WidthPixels()
    {
        return Screen.currentResolution.width;
    }

    public int HeightPixels()
    {
        return Screen.currentResolution.height;
    }

    public abstract Quaternion getOrientation();
}

