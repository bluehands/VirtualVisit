using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class VRInputModule : StandaloneInputModule {

    private Vector2 m_cursorPos;

    // Hand this function your fake mouse position (in screen coords)
    public void UpdateCursorPosition(Vector2 a_pos)
    {
        m_cursorPos = a_pos;
    }

    // This is the real function we want, the two commented out lines (Input.mousePosition) are replaced with m_cursorPos (our fake mouse position, set with the public function, UpdateCursorPosition)
    private readonly MouseState m_MouseState = new MouseState();
    protected override MouseState GetMousePointerEventData(int id = 0)
    {
        MouseState m = new MouseState();

        // Populate the left button...
        PointerEventData leftData;
        var created = GetPointerData(kMouseLeftId, out leftData, true);

        leftData.Reset();

        if (created)
            leftData.position = m_cursorPos;
        //leftData.position = Input.mousePosition;

        //Vector2 pos = Input.mousePosition;
        Vector2 pos = m_cursorPos;
        leftData.delta = pos - leftData.position;
        leftData.position = pos;
        leftData.scrollDelta = Input.mouseScrollDelta;
        //leftData.button = PointerEventData.InputButton.Left;
        eventSystem.RaycastAll(leftData, m_RaycastResultCache);
        var raycast = FindFirstRaycast(m_RaycastResultCache);
        leftData.pointerCurrentRaycast = raycast;
        m_RaycastResultCache.Clear();

        // copy the apropriate data into right and middle slots
        PointerEventData rightData;
        GetPointerData(kMouseRightId, out rightData, true);
        CopyFromTo(leftData, rightData);
        rightData.button = PointerEventData.InputButton.Right;

        PointerEventData middleData;
        GetPointerData(kMouseMiddleId, out middleData, true);
        CopyFromTo(leftData, middleData);
        middleData.button = PointerEventData.InputButton.Middle;

        // Use the "A" button on the xbox controller for selection instead of the left mouse button
        PointerEventData.FramePressState selectState = PointerEventData.FramePressState.NotChanged;
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            selectState = PointerEventData.FramePressState.Pressed;
        else if (Input.GetKeyUp(KeyCode.Joystick1Button0))
            selectState = PointerEventData.FramePressState.Released;

        m_MouseState.SetButtonState(PointerEventData.InputButton.Left, selectState, leftData);
        m_MouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), rightData);
        m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), middleData);

        return m_MouseState;
    }
}
