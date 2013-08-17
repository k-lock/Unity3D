using UnityEngine;
using System.Collections;

public class CameraOrient : MonoBehaviour {
	
	public Camera m_Camera;
	public Transform m_Player;
	void Start () {
		if(!m_Camera) m_Camera = Camera.main;
		//if(!m_Player) m_Player = transform.Find("SoldierRed");
		//transform.localRotation = new Quaternion(0,180,0,0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		//Quaternion rotations = Quaternion.LookRotation(-m_Camera.transform.forward, m_Camera.transform.up);
		transform.localRotation = new Quaternion(0,m_Camera.transform.localRotation.y,0,m_Camera.transform.localRotation.w);
		transform.position = m_Player.position + Vector3.up*.9f;
		// fade out for ugly angles
		//float dist = (m_Camera.transform.position-transform.position).sqrMagnitude;
		//transform.renderer.material.color = new Color(0F,0F,0F, dist*0.00000000F);

	}
}
