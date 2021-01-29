using UnityEngine;
using UnityEngine.UI;

class SensorFusion
{
    //private MadgwickAHRS m_MadgwickAHRS;
    private MahonyAHRS m_MahonyAHRS;

    private float m_FrameCount;
    private float m_DeltaTime;

    public SensorFusion(float samplePeriod, float beta)
    {
        m_MahonyAHRS = new MahonyAHRS(samplePeriod, beta);
    }

    public Quaternion process(Vector3 gyro, Vector3 acc)
    {
        update(acc);

        m_MahonyAHRS.Update(gyro.x, gyro.y, gyro.z, acc.x, acc.y, acc.z);

        return getQuaternion();
    }

    public Quaternion process(Vector3 gyro, Vector3 acc, Vector3 mag)
    {
        update(acc);

        m_MahonyAHRS.Update(gyro.x, gyro.y, gyro.z, acc.x, acc.y, acc.z, mag.x, mag.y, mag.z);

        return getQuaternion();
    }

    private void update(Vector3 acc)
    {
        if (!isInit() && isValied(acc))
        {
            init(acc);
        }
        updateFrameRate();
    }

    private void init(Vector3 acc)
    {
        var q = Quaternion.FromToRotation(Vector3.up, acc);
        m_MahonyAHRS.Quaternion = new float[] { q.w, q.x, q.y, q.z };
    }

    private bool isValied(Vector3 data)
    {
        return data.x != 0 && data.y != 0 && data.z != 0;
    }

    private bool isInit()
    {
        return m_MahonyAHRS.Quaternion[1] != 0
            && m_MahonyAHRS.Quaternion[2] != 0
            && m_MahonyAHRS.Quaternion[3] != 0;
    }

    private Quaternion getQuaternion()
    {
        var quat = m_MahonyAHRS.Quaternion;
        return new Quaternion(quat[1], quat[2], quat[3], quat[0]);
    }

    internal float getSampleRate()
    {
        return m_MahonyAHRS.SamplePeriod;
    }

    private void updateFrameRate()
    {
        m_DeltaTime += Time.deltaTime;
        m_FrameCount++;
        if (m_DeltaTime >= 1)
        {
            if(m_FrameCount != 0)
            {
                m_MahonyAHRS.SamplePeriod = 1f / m_FrameCount;
            }
            m_FrameCount = 0;
            m_DeltaTime = m_DeltaTime - 1;
        }
    }
}

