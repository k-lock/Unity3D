using UnityEngine;
using System.Collections;

public class Mover_2 : MonoBehaviour
{

	
	public float skateSpeed = 12.0f;
	public float turnSpeed = 45.0f;
	public float gravity = 20.0f;
	public float jumpPower = 9.0f;
	
	public Transform model;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	
	Vector3 startPos;
	Quaternion startRot;
	
	private float throttle = 0; 
	private float steer = 0;
	private float drive = 0; 
	
	private int direction = 1;
	
	
	void Awake()
	{
	
		startPos = transform.position;
		startRot = transform.rotation;
		
		controller = GetComponent<CharacterController>();
		
		
	}
	
	
	
	void Update ()
	{
		//print(controller.isGrounded);
		
		steer = Mathf.Clamp((Input.GetAxis ("Horizontal")), -1,1);
		drive = Mathf.Clamp((Input.GetAxis ("Vertical")), -1,1);
		
		Quaternion currentRotation = Quaternion.Euler (0, transform.eulerAngles.y, 0);
		Vector3 debugStartPoint = transform.position;//* steer;
		Vector3 debugEndPoint = currentRotation * Vector3.right + debugStartPoint;
			
		Debug.DrawLine(debugStartPoint, debugEndPoint, Color.red);
		
		print( transform.position + " / " + transform.TransformDirection(currentRotation * Vector3.right + debugStartPoint));
		
		
		if (controller.isGrounded) {
			
			transform.Rotate (turnSpeed * Vector3.up * Time.deltaTime * steer);
			
			moveDirection = transform.InverseTransformDirection(currentRotation * Vector3.right);
			moveDirection *= (skateSpeed / 2) * drive;
			
			
		
			
			
			//if (Input.GetButton ("Jump")) moveDirection = new Vector3(moveDirection.x, jumpPower, moveDirection.z);
			
			
			//
			
	
			Ray ray = new Ray (transform.position, -Vector3.up);
			RaycastHit hit;
		
			if (Physics.Raycast (ray,out hit))
				
				model.rotation = Quaternion.Lerp( model.rotation, Quaternion.FromToRotation ( Vector3.up, hit.normal), 9.0f * Time.deltaTime);
			}else
			
			model.rotation = Quaternion.identity;
			model.eulerAngles = new Vector3( 0, transform.eulerAngles.y, 0);
			
			moveDirection.y -= gravity * Time.deltaTime;
			
			controller.Move( moveDirection * Time.deltaTime);
		
			
			
	/*
			//moveDirection.y += .05f ;//gravity * Time.deltaTime;
		
	//	print(moveDirection);
		
		moveDirection.y = -.045f;// = new Vector3(moveDirection.x, 0, moveDirection.z);
		
		controller.Move( moveDirection * Time.deltaTime );
		
	*/
		/*Vector3 forward = new Vector3(.00001f * Time.fixedTime, 0, 0);//transform.TransformDirection();
		forward.y = 0;
			controller.Move( forward );
		*/
		
		
	}
	
	void OnGUI()
	{
		
		if(GUI.Button(new Rect(5,5, 200, 20), "RESET"))
		{
			transform.position = startPos;
			transform.rotation = startRot;
		}
		
		
	}
	
}

