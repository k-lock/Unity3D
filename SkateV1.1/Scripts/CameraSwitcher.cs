using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour {

	public Camera[] cameras;
	public int activeCamera = 0;

	void Update()
	{

		if(Input.GetKeyDown (KeyCode.F1)) activeCamera = 0;
		if(Input.GetKeyDown (KeyCode.F2)) activeCamera = 1;
	//	if(Input.GetKeyDown (KeyCode.F3)) activeCamera = 2;
	//	if(Input.GetKeyDown (KeyCode.F4)) activeCamera = 3;
		
		SetupCameras();
		
	}
	
	void SetupCameras()
	{

		foreach(Camera cam in cameras)	
			cam.enabled = false;
		
		((Camera) cameras[ activeCamera ]).enabled = true;
		
	}
	
}
