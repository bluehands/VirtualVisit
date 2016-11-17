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
        return Quaternion.Euler(-90, 0, 0) * m_SensorFusion.process(getGyro(), getAcc()) * Quaternion.Euler(0, 0, 180);
    }
}
