using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	
	public Transform target;
	
	public float distance = 10.0f;
	public float height = 5.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;

	void FixedUpdate () {

		if (!target)return;
		
		// Calculate the current rotation angles
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;
			
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
		
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
	
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
	
		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		Vector3 camPos = target.position;
		camPos -= currentRotation * Vector3.forward * distance;
	
		// Set the height of the camera
		transform.position = new Vector3(camPos.x, currentHeight, camPos.y);
		
		// Always look at the target
		transform.LookAt (target);
	}

}
