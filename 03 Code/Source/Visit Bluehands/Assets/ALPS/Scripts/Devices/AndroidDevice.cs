using UnityEngine;

class AndroidDevice : BaseDevice
{
    public AndroidDevice() : base(1 / 16f, 1f)
    {
    }

    public override Vector3 getGyro()
    {
        return base.getGyro();
    }

    public override Quaternion getOrientation()
    {
        return Quaternion.Euler(-90, 0, 0) * m_SensorFusion.process(getGyro(), getAcc()) * Quaternion.Euler(0, 0, 180);
    }
}
