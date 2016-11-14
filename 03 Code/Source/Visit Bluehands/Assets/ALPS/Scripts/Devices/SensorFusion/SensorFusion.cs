using UnityEngine;

class SensorFusion
{
    private MadgwickAHRS m_MadgwickAHRS;

    private float m_FrameCount;
    private float m_DeltaTime;

    private float m_CallibrationTime = 1.2f;

    public SensorFusion(float samplePeriod, float beta)
    {
        m_MadgwickAHRS = new MadgwickAHRS(samplePeriod, beta);
    }

    public Quaternion process(Vector3 gyro, Vector3 acc)
    {
        checkCalibration();
        checkFrameRate();

        m_MadgwickAHRS.Update(gyro.x, gyro.y, gyro.z, acc.x, acc.y, acc.z);

        var quat = m_MadgwickAHRS.Quaternion;
        return new Quaternion(quat[1], quat[2], quat[3], quat[0]);
    }

    public Quaternion process(Vector3 gyro, Vector3 acc, Vector3 mag)
    {
        checkCalibration();
        checkFrameRate();

        m_MadgwickAHRS.Update(gyro.x, gyro.y, gyro.z, acc.x, acc.y, acc.z, mag.x, mag.y, mag.z);

        var quat = m_MadgwickAHRS.Quaternion;
        return new Quaternion(quat[1], quat[2], quat[3], quat[0]);
    }

    internal float getSampleRate()
    {
        return m_MadgwickAHRS.SamplePeriod;
    }

    private void checkCalibration()
    {
        m_CallibrationTime -= Time.deltaTime;
        if (m_CallibrationTime <= 0)
        {
            m_MadgwickAHRS.Beta = 0.01f;
        } else
        {
            m_MadgwickAHRS.Beta = m_CallibrationTime;
        }
    }

    private void checkFrameRate()
    {
        m_DeltaTime += Time.deltaTime;
        m_FrameCount++;
        if (m_DeltaTime >= 1)
        {
            if(m_FrameCount != 0)
            {
                m_MadgwickAHRS.SamplePeriod = 1 / m_FrameCount;
            }
            m_FrameCount = 0;
            m_DeltaTime = m_DeltaTime - 1;
        }
    }
}

