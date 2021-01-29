using UnityEngine;

public interface IMobileDevice
{
    Vector3 Gyro {get;}

    Vector3 Acceleration {get;}

    Vector3 Compass {get;}

    Quaternion Orientation {get;}

    float SampleRate {get;}
}
