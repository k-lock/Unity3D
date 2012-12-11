using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class Test: MonoBehaviour {
	
	public float skateSpeed = 12.0f;
	public float turnSpeed = 45.0f;
	public float jumpSpeed = 9.0f;
	
	public float gravity = 20.0f;
	
	private Vector3 moveDirection;
	private CharacterController controller;
	
	
	private Vector3 startPosition;
	
	private Animation animator;
	private Transform board;
	
	
	
	void Awake () {
		
		controller = gameObject.GetComponent<CharacterController>();	
		
		board = transform.Find("Board");
		
		animator = GetComponent<Animation>();
		animator.wrapMode = WrapMode.Loop;

		animator["Clip_idle"].wrapMode     = WrapMode.Clamp;
		animator["Clip_ollie"].wrapMode    = WrapMode.Clamp;
		animator["Clip_kickFlip"].wrapMode = WrapMode.Clamp;
		animator["Clip_heelFlip"].wrapMode = WrapMode.Clamp;
	//	animator["Clip_FSvarial"].wrapMode = WrapMode.Clamp;
	//	animator["Clip_FSvarial_B"].wrapMode = WrapMode.Clamp;
		
		animator["Clip_idle"].layer = -1;
		animator["Clip_ollie"].layer = -1;
		animator["Clip_kickFlip"].layer = -1;
		animator["Clip_heelFlip"].layer = -1;
	//	animator["Clip_FSvarial"].layer = -2;
	//	animator["Clip_FSvarial_B"].layer = -2;
		
		startPosition = transform.position;
		
		animation.Stop();

	//	rigidbody.centerOfMass = new Vector3(0, 0, 0 );
	}

	void FixedUpdate ()
	{

		if (controller.isGrounded) {
       
			moveDirection = transform.forward;
           	moveDirection *= Input.GetAxis ("Vertical") * skateSpeed;
        
			if(Input.GetAxis ("Vertical")  > .5f)transform.Rotate (turnSpeed * Vector3.up * Time.deltaTime * Input.GetAxis ("Horizontal"));
			if (Input.GetButton("Jump")) moveDirection.y = jumpSpeed;
		
				
			Ray ray = new Ray (transform.position, -Vector3.up);
			RaycastHit hit;
			Quaternion nRot;
					
			if (Physics.Raycast (ray,out hit)){
	//			if(hit.transform.gameObject.name =="Park"){
					nRot = Quaternion.Lerp( transform.rotation, Quaternion.FromToRotation ( Vector3.up, hit.normal), 10.0f * Time.deltaTime);
					transform.rotation = new Quaternion( nRot.x , transform.rotation.y ,transform.rotation.z ,transform.rotation.w);
					print(hit.transform.gameObject.name + " " + nRot);
	//			}
			}else{
				
		//		transform.rotation = Quaternion.identity;
		//		transform.eulerAngles = new Vector3( 0, transform.eulerAngles.y, 0);
				
			
			}
			
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	
	
	}
	
}
