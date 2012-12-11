using UnityEngine;
using System.Collections;

public class Movin : MonoBehaviour {

	float speed = 50.0f;
	float rotatationSpeed = 200.0f;
	private float curSpeed = 0.0f;
	private CharacterController controller;
	
	
	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	
	void Update () {
		
		float newRotation = Input.GetAxis("Horizontal") * rotatationSpeed;
		transform.Rotate(0, newRotation * Time.deltaTime, 0);
	
		// Calculate speed
		float newSpeed = Input.GetAxis("Vertical") * speed;
		newSpeed = Mathf.Clamp( newSpeed, 0, 1);
		if (Input.GetKey("left shift"))
			newSpeed *= 1.5f;
	
		// Move the controller
		
		Vector3 forward = transform.TransformDirection(1,0,0);
		//forward.y -=  20.0f * Time.deltaTime;
		print( forward * newSpeed );

		controller.SimpleMove( forward * newSpeed );
		print(controller.isGrounded);
		
		
		if(!controller.isGrounded) transform.position += new Vector3(0,0.01f * Time.deltaTime,0);
	}
}
