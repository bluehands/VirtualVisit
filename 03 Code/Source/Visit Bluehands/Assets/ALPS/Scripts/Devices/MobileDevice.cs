using UnityEngine;

public interface MobileDevice
{
    Vector3 getGyro();

    Vector3 getAcc();

    Vector3 getMag();

    Quaternion getOrientation();

    float getSampleRate();

    int WidthPixels();

    int HeightPixels();
}
