using UnityEngine;

public class StandaloneDevice : MouseDevice
{
    protected override bool ShouldRotate => true;
    protected override float sensX => 3 * base.sensX;
    protected override float sensY => 3 * base.sensY;

    public StandaloneDevice() : base()
    {
        Cursor.visible = false;
    }
}