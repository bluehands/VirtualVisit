using UnityEngine;

class WindowsPhoneDevice : BaseDevice
{
    public WindowsPhoneDevice() : base(1 / 60f, 1f)
    {
    }

    public override Vector3 getGyro()
    {
        return base.getGyro() * Mathf.Deg2Rad;
    }

    public override Quaternion getOrientation()
    {
        if (getGyro()[0].Equals(0) && getGyro()[1].Equals(0) && getGyro()[2].Equals(0))
        {
            return Quaternion.Euler(90, 0, 0) * m_Gyroscope.attitude * Quaternion.Euler(0, 0, 180);
        }
        return Quaternion.Euler(-90, 0, 0) * m_SensorFusion.process(getGyro(), getAcc()) * Quaternion.Euler(0, 0, 180);

    }
}
