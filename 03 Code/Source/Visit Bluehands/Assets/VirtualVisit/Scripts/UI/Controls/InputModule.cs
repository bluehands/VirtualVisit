using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using System.Linq;

public class InputModule : BaseInputModule
{
    public static IVRPointer Pointer {get;set;}
    private PointerEventData _pointerData;
    private Vector2 _lastHeadPosition;

    public override void Process()
    {
        var previousGaze = GetCurrentGameObject();
        CastRayFromGaze();
        UpdateCurrentObject();
        UpdateReticle(previousGaze);
    }

    private GameObject GetCurrentGameObject()
    {
        if (_pointerData?.enterEventCamera == null)
        {
            return null;
        }
        return _pointerData.pointerCurrentRaycast.gameObject;
    }
    private void CastRayFromGaze()
    {
        Quaternion headOrientation;
#if UNITY_ANDROID || UNITY_EDITOR
        // headOrientation = InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head);
        var devices = new System.Collections.Generic.List<InputDevice>();
        // InputDevices.GetDevicesWithRole(InputDeviceRole.HardwareTracker, devices);
        var filter = InputDeviceCharacteristics.None;
        InputDevices.GetDevicesWithCharacteristics(filter, devices);
        if (devices.Any() && devices.First().TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation))
        {
            headOrientation = rotation;
        }
        else 
        {
            headOrientation = Quaternion.identity; //TODO
        }
#else
        headOrientation = Quaternion.identity; //TODO
#endif
        var headPosition = NormalizedCartesianToSpherical(headOrientation * Vector3.forward);
        if (_pointerData == null)
        {
            _pointerData = new PointerEventData(eventSystem);
            _lastHeadPosition = headPosition;
        }

        _pointerData.Reset();
        _pointerData.position = GetGazePointerPosition();
        eventSystem.RaycastAll(_pointerData, m_RaycastResultCache); //?
        _pointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_RaycastResultCache.Clear();
        _pointerData.delta = headPosition - _lastHeadPosition;
        _lastHeadPosition = headPosition;
    }

    private Vector2 NormalizedCartesianToSpherical(Vector3 cartesianCoordinates)
    {
        cartesianCoordinates.Normalize();
        if (cartesianCoordinates.x == 0)
        {
            cartesianCoordinates.x = Mathf.Epsilon;
        }
        var outPolar = Mathf.Atan(cartesianCoordinates.z / cartesianCoordinates.x);
        if (cartesianCoordinates.x < 0)
        {
            outPolar += Mathf.PI;
        }
        var outElevation = Mathf.Asin(cartesianCoordinates.y);
        return new Vector2(outPolar, outElevation);
    }

    private Vector2 GetGazePointerPosition()
    {
        int width = Screen.width;
        int height = Screen.height;
#if UNITY_ANDROID
        if (UnityEngine.XR.XRSettings.enabled)
        {
            width = UnityEngine.XR.XRSettings.eyeTextureWidth;
            height = UnityEngine.XR.XRSettings.eyeTextureHeight;
        }
#endif
        return new Vector2(0.5f * width, 0.5f * height);
    }

    private void UpdateCurrentObject()
    {
        var gameObject = _pointerData.pointerCurrentRaycast.gameObject;
        HandlePointerExitAndEnter(_pointerData, gameObject);
        var selected = ExecuteEvents.GetEventHandler<ISelectHandler>(gameObject);
        if (selected == eventSystem.currentSelectedGameObject)
        {
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, GetBaseEventData(), ExecuteEvents.updateSelectedHandler);
        }
        else 
        {
            eventSystem.SetSelectedGameObject(null, _pointerData);
        }
    }

    private void UpdateReticle(GameObject previousGaze)
    {
        if (Pointer == null)
        {
            return;
        }
        var camera = _pointerData.enterEventCamera;
        var gazeObject = GetCurrentGameObject();
        var intersectionPosition = GetIntersectionPosition();
        bool isInteractive = _pointerData.pointerPress != null || ExecuteEvents.GetEventHandler<IPointerClickHandler>(gazeObject) != null;

        if (gazeObject == previousGaze)
        {
            if (gazeObject != null)
            {
                Pointer.OnGazeStay(camera, gazeObject, intersectionPosition, isInteractive);
            }
        }
        else 
        {
            if (previousGaze != null)
            {
                Pointer.OnGazeExit(camera, previousGaze);
            }
            if (gazeObject != null)
            {
                Pointer.OnGazeStart(camera, gazeObject, intersectionPosition, isInteractive);
            }
        }
    }

    private Vector3 GetIntersectionPosition()
    {
        var camera = _pointerData.enterEventCamera;
        if (camera == null)
        {
            return Vector3.zero;
        }
        var distance = _pointerData.pointerCurrentRaycast.distance + camera.nearClipPlane;
        var position = camera.transform.position + camera.transform.forward * distance;
        return position;
    }
}