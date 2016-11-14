using UnityEngine;

class WindowsPhoneDevice : BaseDevice
{
    public WindowsPhoneDevice() : base(1 / 60f, 0.01f)
    {
    }

    public override Vector3 getGyro()
    {
        return m_Gyroscope.rotationRateUnbiased * Mathf.Deg2Rad;
    }

    public override Quaternion getOrientation()
    {
        return Quaternion.Euler(-90, 0, 0) * m_SensorFusion.process(getGyro(), getAcc()) * Quaternion.Euler(0, 0, 180);
    }
}
