using UnityEngine;
using System.Collections;

public class SkaterNewMover : MonoBehaviour {
	
	public float rotateSpeed = 90;
	public float pedalInput = 3.5f;
	public float maxSpeed = 12;
	public float decayRate = 0.3f;
	public LayerMask raycastMask;
	
	private CharacterController character;
	private Transform trans;
	public float speed = 0.0f;
	public int direction;
	
	private Vector3 startPosition;
	private Quaternion startRotation;
	
	private Quaternion targetRot;
	private bool vertical = false;
	
	void Awake () {
	
		character = GetComponent<CharacterController>();
		trans = transform;
		
		speed = 0.0f;
		direction = 1;
		startPosition = trans.position;
		startRotation = trans.rotation;
		
		targetRot = Quaternion.identity;
		
	}
	
	void Start () {
	
	}
	void Draw_Rays()
	{
		
		Debug.DrawLine( trans.position, trans.position + trans.forward, Color.blue);	
		
		Debug.DrawLine( trans.position, trans.position + trans.up, Color.green);	
		Debug.DrawLine( trans.position + Vector3.up, trans.position - Vector3.up, Color.red);	
		
		
		
		
	}

	void Update () {
		
		Draw_Rays();
		
		
		if( character.isGrounded && Input.GetKeyDown(KeyCode.LeftShift)) Pedal();
		
		Steer();

		Force();
		Throttle();
		
	
	
		
	}
	
	void Steer()
	{

		float horizontal = Input.GetAxis("Horizontal");
		trans.Rotate(0, horizontal * rotateSpeed * Time.deltaTime,  0);		

	}
	
	void Throttle()
	{
		if(character.isGrounded){
			
			if(speed < 0.3f){
				speed = 0;
			}else{
				speed -= decayRate * Time.deltaTime * speed; 
			}
		}
	}


	void Force()
	{
		
		Ray ray = new Ray(trans.position + Vector3.up, -Vector3.up);  
		RaycastHit hit;

		if(character.isGrounded && Physics.Raycast(ray, out hit, 2, raycastMask)){
			
			Vector3 targetRight = Vector3.Cross(hit.normal, trans.forward);
			Vector3 targetForward =  Vector3.Cross(targetRight, hit.normal);
	
			targetRot = Quaternion.LookRotation( targetForward);
			trans.rotation = Quaternion.Slerp( trans.rotation, targetRot, 5 * Time.deltaTime );

		}
		float angle = Vector3.Angle( trans.position + Vector3.up, trans.position + trans.up);
	//	direction = (Mathf.Sign(ax_Angle) > 0 ) ? 1 : -1;
		
		print( angle + " " + Mathf.Sign(angle) + "| " + trans.rotation.x);
		
		Vector3 moveDirection = trans.forward * speed * direction;
		moveDirection += Physics.gravity;
		
		character.Move( moveDirection * Time.deltaTime);
	
	}
	
	
	void Pedal()
	{
		speed += pedalInput;
		speed = Mathf.Min(speed, maxSpeed);
	}
	
	
	
	// GUI STUFF ----------------------------------------------------------->
	
	void OnGUI() {
		GUI_INFO();
	}
	void GUI_INFO()
	{
		if(GUI.Button(new Rect(10,10,200,20), "RESET")) {
			
			transform.position = startPosition;
			transform.rotation = startRotation;

			//Awake();
		}
		float upright = Vector3.Dot( Vector3.up, trans.forward  );
		GUI.Label(new Rect(10,30,200,20), "UP-RIGHT : "+  Mathf.Sign(upright) );
		GUI.Label(new Rect(10,50,200,20), "isGrounded : " + character.isGrounded);
		GUI.Label(new Rect(10,70,200,20), "isVerted : " + vertical);
		if(GUI.Button(new Rect(10,90,200,20), "ROTATIN RESET")) {
			
			transform.rotation = Quaternion.identity;

			//Awake();
		}
	}
	
	
}
 /* 
if (dir != Vector3.zero) {
    transform.rotation = Quaternion.Slerp(
        transform.rotation,
        Quaternion.LookRotation(dir),
        Time.deltaTime * rotationSpeed
    );
}
  * */