﻿/************************************************************************
	ALPSController is the main class which manages custom rendering

    Copyright (C) 2014  ALPS VR.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ALPSController : MonoBehaviour {

	//=====================================================================================================
	// Attributes
	//=====================================================================================================

	/**Public**/
	//The current device configuration
	public ALPSConfig deviceConfig = CardboardDevice.GetConfig(CardboardType.DEFAULT);

	//One camera for each eye
	private GameObject cameraLeft;
    private GameObject cameraRight;

    //Render textures
    private RenderTexture srcTex;
    private RenderTexture destTex;

	//Screen size
	public static int screenWidthPix;
	public static int screenHeightPix;

    //Material
    private Material mat;

	/**Private**/
	private Rect rectLeft,rectRight;
	private float DPI;

    public MobileDevice Device;

    //=====================================================================================================
    // Functions
    //=====================================================================================================

    /// <summary>
    /// Initializes side-by-side rendering and head tracking. 
    /// </summary>
    public void Awake(){
		ALPSCamera.deviceConfig = deviceConfig;
		ALPSBarrelMesh.deviceConfig = deviceConfig;

#if UNITY_EDITOR
        Device = new EditorDevice();
#elif UNITY_ANDROID
		Device = new AndroidDevice();
#elif UNITY_WP_8_1 || UNITY_WSA_10_0
        Device = new WindowsPhoneDevice();
#elif UNITY_IOS
        Device = new iOSDevice();
#endif

        screenWidthPix = Device.WidthPixels();
        screenHeightPix = Device.HeightPixels();

        //Make sure the longer dimension is width as the phone is always in landscape mode
        if (screenWidthPix<screenHeightPix){
			int tmp = screenHeightPix;
			screenHeightPix = screenWidthPix;
			screenWidthPix = tmp;
		}

		for (var i=0; i<2; i++) {
			bool left = (i==0);
			GameObject OneCamera = new GameObject(left? "MainCamera Left" : "MainCamera Right");
			OneCamera.AddComponent<Camera>();
			OneCamera.AddComponent<ALPSCamera>();
			(OneCamera.GetComponent("ALPSCamera") as ALPSCamera).leftEye = left;
            OneCamera.transform.parent = transform;
            OneCamera.transform.position = transform.position;
            if (left) cameraRight = OneCamera;
			else cameraLeft = OneCamera;
        }

        ALPSCamera[] ALPSCameras = FindObjectsOfType(typeof(ALPSCamera)) as ALPSCamera[];
		foreach (ALPSCamera cam in ALPSCameras) {
			cam.Init();
		}

		mat = Resources.Load ("Materials/ALPSDistortion") as Material;

		DPI = Screen.dpi;

		//Render Textures
		srcTex = new RenderTexture (2048, 1024, 16);
		destTex = GetComponent<Camera>().targetTexture;
		cameraLeft.GetComponent<Camera>().targetTexture = cameraRight.GetComponent<Camera>().targetTexture = srcTex;

		// Setting the main camera
		GetComponent<Camera>().aspect = 1f;
		GetComponent<Camera>().backgroundColor = Color.black;
		GetComponent<Camera>().clearFlags =  CameraClearFlags.Nothing;
		GetComponent<Camera>().cullingMask = 0;
		GetComponent<Camera>().eventMask = 0;
		GetComponent<Camera>().orthographic = true;
		GetComponent<Camera>().renderingPath = RenderingPath.Forward;
		GetComponent<Camera>().useOcclusionCulling = false;
		cameraLeft.GetComponent<Camera>().depth = 0;
		cameraRight.GetComponent<Camera>().depth = 1;
		GetComponent<Camera>().depth = Mathf.Max (cameraLeft.GetComponent<Camera>().depth, cameraRight.GetComponent<Camera>().depth) + 1;

		AudioListener[] listeners = FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
		if (listeners.Length < 1) {
			gameObject.AddComponent <AudioListener>();
		}

        ClearDirty();
	}

    public void LateUpdate()
    {
        transform.localRotation = Device.getOrientation();
    }

	/// <summary>
	/// Renders scene for both cameras.
	/// </summary>
	public void OnPostRender(){
		RenderTexture.active = destTex;
		GL.Clear(false, true, Color.black);
		RenderEye(true, srcTex);
		RenderEye(false, srcTex);
		srcTex.DiscardContents();
	}

	/// <summary>
	/// Renders scene for one camera.
	/// </summary>
	/// <param name="_leftEye">True if renders for the left camera, false otherwise.</param>
	/// <param name="_source">Source texture on which the camera renders.</param>
	private void RenderEye(bool _leftEye, RenderTexture _source){
		mat.mainTexture = _source;
		mat.SetVector("_SHIFT",new Vector2(_leftEye?0:0.5f,0));
		float convergeOffset = ((deviceConfig.Width * 0.5f) - deviceConfig.IPD) / deviceConfig.Width;
		mat.SetVector("_CONVERGE",new Vector2((_leftEye?1f:-1f)*convergeOffset,0));
		mat.SetFloat("_AberrationOffset",deviceConfig.enableChromaticCorrection?deviceConfig.chromaticCorrection:0f);
		float ratio = (deviceConfig.IPD*0.5f) / deviceConfig.Width;
		mat.SetVector("_Center",new Vector2(0.5f+(_leftEye?-ratio:ratio),0.5f));

		GL.Viewport(_leftEye ? rectLeft : rectRight);

		GL.PushMatrix();
		GL.LoadOrtho();
		mat.SetPass(0);
		if(_leftEye)cameraLeft.GetComponent<ALPSCamera>().Draw();
		else cameraRight.GetComponent<ALPSCamera>().Draw();
		GL.PopMatrix();
	}

	/// <summary>
	/// Resets all the settings and applies the current DeviceConfig
	/// </summary>
	public void ClearDirty(){
		//We give the current DPI to the new ALPSConfig
		deviceConfig.DPI = DPI;
		if (deviceConfig.DPI <= 0) {
			deviceConfig.DPI = ALPSConfig.DEFAULT_DPI;
		}
	
		if(cameraLeft!=null && cameraRight!=null){
			float widthPix = deviceConfig.WidthPix();
			float heightPix = deviceConfig.HeightPix();

			rectLeft  = new Rect (screenWidthPix*0.5f-widthPix*0.5f,screenHeightPix*0.5f-heightPix*0.5f,widthPix*0.5f,heightPix);
			rectRight = new Rect (screenWidthPix*0.5f,screenHeightPix*0.5f-heightPix*0.5f,widthPix*0.5f,heightPix);

			Vector3 camLeftPos = cameraLeft.transform.localPosition; 
			camLeftPos.x = -deviceConfig.ILD*0.0005f;
			cameraLeft.transform.localPosition = camLeftPos;
			
			Vector3 camRightPos = cameraRight.transform.localPosition;
			camRightPos.x = deviceConfig.ILD*0.0005f;
			cameraRight.transform.localPosition = camRightPos;
			
			cameraLeft.GetComponent<Camera>().fieldOfView = deviceConfig.fieldOfView;
			cameraRight.GetComponent<Camera>().fieldOfView = deviceConfig.fieldOfView;

			cameraLeft.GetComponent<ALPSCamera>().UpdateMesh();
			cameraRight.GetComponent<ALPSCamera>().UpdateMesh();
		}
	}
}