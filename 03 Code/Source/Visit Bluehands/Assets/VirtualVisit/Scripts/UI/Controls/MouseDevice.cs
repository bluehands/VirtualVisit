using UnityEngine;

public abstract class MouseDevice : IMobileDevice
{
    protected virtual float sensX => 100.0f;
    protected virtual float sensY => 100.0f;

    private float rotationY = 0.0f;
    private float rotationX = 0.0f;

    protected MouseDevice()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public Vector3 Gyro => new Vector3();

    public Vector3 Acceleration => new Vector3();

    public Vector3 Compass => new Vector3();

    public float SampleRate => 0f;
    
    protected abstract bool ShouldRotate {get;}

    public Quaternion Orientation
    {
        get 
        {
            if (ShouldRotate)
            {
                Cursor.lockState = CursorLockMode.Locked;
                rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
                rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            }
            else 
            {
                Cursor.lockState = CursorLockMode.None;
            }
            return Quaternion.Euler(-rotationY, rotationX, 0f);
        }
    }
}