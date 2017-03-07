using UnityEngine;

public abstract class FollowingBase : MonoBehaviour {

    void Start () {
        QualitySettings.antiAliasing = 2;

        transform.rotation = getCamera().transform.rotation;
    }

    protected Camera getCamera()
    {
        return Camera.main;
    }

    protected float getDistanceAngle(float rotationCamera, float rotationFollower)
    {
        var disntace = 0f;
        if (Mathf.Abs(rotationCamera - rotationFollower) < 180)
        {
            disntace = rotationCamera - rotationFollower;
        }
        else
        {
            if (rotationCamera < 180)
            {
                disntace = rotationCamera - rotationFollower + 360;
            }
            else
            {
                disntace = rotationCamera - rotationFollower - 360;
            }
        }
        return disntace;
    }

    protected float getCorrection(float distanceAngle, float angle)
    {
        var correction = 0f;
        if (distanceAngle > angle)
        {
            correction = (distanceAngle - angle);
        }
        else if (distanceAngle < -angle)
        {
            correction = (distanceAngle + angle);
        }
        return correction / 10;
    }

    protected float modulo(float dividend, int divisor)
    {
        return (((dividend) % divisor) + divisor) % divisor;
    }
}
