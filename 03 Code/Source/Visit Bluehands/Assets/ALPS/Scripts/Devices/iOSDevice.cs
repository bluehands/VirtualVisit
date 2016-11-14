using UnityEngine;

class iOSDevice : BaseDevice
{
    public iOSDevice() : base(1 / 16f, 0.01f)
    {
    }

    public override Vector3 getGyro()
    {
        return m_Gyroscope.rotationRateUnbiased;
    }

    public override Quaternion getOrientation()
    {
        return Quaternion.Euler(-90, 0, 0) * m_SensorFusion.process(getGyro(), getAcc()) * Quaternion.Euler(0, 0, 180);
    }
}
