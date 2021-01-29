using UnityEngine;

public class EditorDevice : MouseDevice
{
    protected override bool ShouldRotate => Input.GetKey(KeyCode.LeftAlt); 
}
