using UnityEngine;

class EditorDevice : IMobileDevice
{
    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;
    public float sensZ = 200.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;
    float rotationZ = 0.0f;

    public EditorDevice()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public Vector3 Gyro => new Vector3();

    public Vector3 Acceleration => new Vector3();

    public Vector3 Compass => new Vector3();

    public float SampleRate => 0f;

    public int WidthPixels()
    {
        return Screen.width;
    }

    public int HeightPixels()
    {
        return Screen.height;
    }

    public Quaternion Orientation
    {
        get 
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
                rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
                //rotationY = Mathf.Clamp(rotationY, minY, maxY);
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                rotationZ += Input.GetAxis("Mouse X") * sensZ * Time.deltaTime;
            }
            return Quaternion.Euler(-rotationY, rotationX, rotationZ);
        }
    }
}
