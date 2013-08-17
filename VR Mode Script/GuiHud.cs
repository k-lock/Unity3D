using UnityEngine;
using System.Collections;

public class GuiHud : MonoBehaviour {

	
	void Awake () {
		Transform vr = transform.Find("vr_Logo");
		GUITexture gt = vr.GetComponent<GUITexture>();
		
		gt.pixelInset = new Rect((Screen.width-290)*.5f,-(Screen.height-50)*.5f,128,53);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
