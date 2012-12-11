using UnityEngine;
using System.Collections;

public class MotionTester : MonoBehaviour {
	
	private float throttle = 0; 
	private float steer = 0;
	private int direction = 1;
	private bool canSteer = true;
	private bool canDrive = true;
	private Transform rampCollider;
	private Vector3 startPosition;
	private Quaternion startRotation;
	
	public Transform deck;
	
	public WheelCollider[] wheels;
	public Texture[] _textures;
	public float wheelRadius = 0.08f;
	public float suspensionRange = 0.08f;
	public float suspensionDamper = 50.0f;
	public float suspensionFrontSpring = 100000.0f;
	public float suspensionRearSpring  = 100000.0f;
	
	void Awake () {
	
		canSteer = canDrive = true;
		
		direction = 1;
		
		startPosition = transform.position;
		startRotation = transform.rotation;
		
		SetupWheelColliders();
		
	}
	
	void Start () {
	
	}

	void Update () {
		
		if(canDrive){
		
			GetInput();
	
			if( WheelColliderGrounded() ){
	
				ApplySteering();

				if(RampChecker()){
					foreach ( WheelCollider wc in wheels) 
					{
						wc.motorTorque = 0;
						wc.brakeTorque = wc.brakeTorque / 3;
					}
				
					DirectionCorrecter();
				
				}else{
					
					ApplyVelocity();
					ApplyThrottle();
		
				}
				
				SpeedCheck();
			}
			
		}
		
	//	float angle = Vector3.Angle( ((WheelCollider)wheels[0]).transform.position, ((WheelCollider)wheels[2]).transform.position);
		//print((WheelCollider)wheels[0]).transform.position + " - " + (WheelCollider)wheels[2]).transform.position);
	}
	
	void LateUpdate()
	{
		CameraActivator();
	}
	
	//------------------------------------------------------------------------->

	void GetInput()
	{
		throttle = Input.GetAxis("Vertical");
		steer = Input.GetAxis("Horizontal");
	}
		
	
	
	void ApplySteering( )
	{

		if( canSteer )
		{
			
		float horizontal = Input.GetAxis("Horizontal");
	
		transform.Rotate(0, horizontal * 90 * Time.deltaTime, 0);
		
		int maxSwap = 8;
		float swap = maxSwap* horizontal * direction;
			  swap = ( swap < -maxSwap ) ? -maxSwap : swap;
			  swap = ( swap >  maxSwap ) ?  maxSwap : swap;
		print(deck.localEulerAngles.y + swap);	
		
		Vector3 deckRotation = new Vector3( deck.localEulerAngles.x, 180 + swap, deck.localEulerAngles.z);
		
		deck.localEulerAngles = deckRotation;
			
		/*	float steer_max  = 40.0f;
			float steerin = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
			
			if( direction == 1){
			
				((WheelCollider)wheels[0]).steerAngle = steer_max * steerin;
				((WheelCollider)wheels[1]).steerAngle = steer_max * steerin;	
				
			}else{
				
				((WheelCollider)wheels[2]).steerAngle = steer_max * steerin;
				((WheelCollider)wheels[3]).steerAngle = steer_max * steerin;	
				
			}*/
		}
	}	
	private float _speed = 0;
	private float _brake = 0;
	void ApplyVelocity()
	{
		Quaternion rota = Quaternion.Euler( 0, transform.eulerAngles.y + ((direction == 1 ) ? 180 : 0), 0);
		Vector3 forward = rota * Vector3.right + transform.position;
		
		Debug.DrawLine( transform.position, forward, Color.green);
	
		if(Input.GetKeyDown(KeyCode.W))
		{
			foreach ( WheelCollider wc in wheels) 
			{
				wc.motorTorque += 5000 * direction;
				wc.brakeTorque = 0;
			//	rigidbody.velocity =  new Vector3(direction,0,0);
				
				
				//wc.motorTorque = 0;
				
				 _speed += wc.motorTorque;
			}
			
		
	
		}	
		if(Input.GetKeyDown(KeyCode.S))	{
				
			foreach ( WheelCollider wc in wheels) 
			{
				wc.motorTorque = 0;
				wc.brakeTorque = 1000 * direction;
				
				 _brake = 1000 * direction;
			}
				
		}
		if(transform.position.y < 0.02f ) transform.position = new Vector3( transform.position.x, 0.055f, transform.position.z );
					
		
/*		//	rigidbody.velocity =  forward + new Vector3(direction,0,0);
		//	rigidbody.centerOfMass = new Vector3(0,0,-1 * direction);
		}else{
			rigidbody.centerOfMass = new Vector3(0,0,0);
		}
*/
	}
	void ApplyThrottle()
	{
		if( WheelColliderSpeed == 0 ) return;
		
		foreach ( WheelCollider wc in wheels) 
		{
			
			if(wc.motorTorque != 0)	wc.motorTorque -= 10 * direction;
			
			if(WheelColliderSpeed != 0)	wc.brakeTorque += 5;//* direction;
			
		}
		
	}
	
	float maxMotor = 2000;
	float maxBrake = 2500;
		
	void SpeedCheck()
	{
		foreach ( WheelCollider wc in wheels){
			
			if( direction == 1 ) {
				if( wc.motorTorque > maxMotor ){
				
			//		print("_MOTOR GREATER THEN maxMotor");
					wc.motorTorque = maxMotor;
				}else if( wc.motorTorque < 0) wc.motorTorque = 0;
				
			}else{
				if( wc.motorTorque < -maxMotor ){
					wc.motorTorque = -maxMotor;	
					
			//		print("_MOTOR MINUS GREATER THEN ---maxMotor");
				}else if(wc.motorTorque > 0 ){
				print("----->");
					wc.motorTorque = 0;
				}
			}
			if(wc.brakeTorque < 0 ) wc.brakeTorque = 0;
			if(wc.brakeTorque > maxBrake )wc.brakeTorque = maxBrake;
		
		//	print(wc.name + " : MOTOR [ " + wc.motorTorque + " ] / BRAKE [ "+ wc.brakeTorque + " ] ");
		
		}
		
	//	if( WheelColliderSpeed > 1500 )  foreach ( WheelCollider wc in wheels)wc.brakeTorque += 5;
	
	}	
	// ------------------------------------------------------------------------------------------>
	void DirectionCorrecter()
	{
		
		
		if( direction == 1 && Mathf.Sign( rigidbody.velocity.x ) == -1 || direction == -1 &&  Mathf.Sign( rigidbody.velocity.x ) == 1  ) {
			
			direction = direction *-1;
			foreach ( WheelCollider wc in wheels){
				wc.motorTorque = 200 + wc.motorTorque*direction;
				wc.brakeTorque = 0;
			}
			//rigidbody.velocity *= -1; 
			//rigidbody.velocity  = new Vector3( direction * 500, 0, 0);
	
			//skaterAni.Direction = ( direction == 1 ) ? true : false;
			//print(" < -- >  DIRECTION CHANGE " + direction + " | " + rigidbody.velocity);
			//if(!camMovin)camMovin = true;
		}	
	}
	bool RampChecker()
	{
		
		RaycastHit hit;
       
		if (Physics.Raycast(transform.position, -Vector3.up, out hit))
		{

			if( hit.transform.gameObject.tag == "GROUND"){

				if(transform.position.y < 0.04f ) transform.rotation = new Quaternion(0,1,0,0);
		
			}
			if( hit.transform.gameObject.tag == "RAMP"){
				
				RampCollisionObject( hit.transform );
			
				if(transform.position.y < 0.02f ) transform.position = new Vector3( transform.position.x, 0.055f, transform.position.z );
				
		//		if(direction == 1 && rigidbody.velocity.x < 0 || direction == -1 && rigidbody.velocity.x > 0 ){
		//			rigidbody.velocity = new Vector3(rigidbody.velocity.x *-1, rigidbody.velocity.y, rigidbody.velocity.z);
		//		}
				
				return true;		
			}
			
		}
		
		return false;
	}
	
	void RampCollisionObject( Transform hiter )
	{
		if(hiter.gameObject.name == "Quater") 		rampCollider = hiter;
		if(hiter.gameObject.name == "Bank")   		rampCollider = hiter;
		if(hiter.gameObject.name == "Mini Set A 1") rampCollider = hiter;
		if(hiter.gameObject.name == "Mini Set A 2") rampCollider = hiter;
		if(hiter.gameObject.name == "Mini Set B 1") rampCollider = hiter;
		if(hiter.gameObject.name == "Mini Set B 2") rampCollider = hiter;
		if(hiter.gameObject.name == "Quater Pipe 1") rampCollider = hiter;		
		if(hiter.gameObject.name == "Quater Pipe 2") rampCollider = hiter;	
		
	}
		
	// ----------------------------------------------------------------------------------------WHELLCOLLIDERS

	void SetupWheelColliders()
	{
		SetupWheelFrictionCurve();
	
		int index = 0;
		foreach ( WheelCollider wc in wheels) 
		{
			SetupWheelCollider(wc, ( index == 0 || index == 2 )? true : false, ( index < 2 )? true : false );
			index++;
		}
		
	}
	
	WheelFrictionCurve SetupWheelFrictionCurve()
	{
		WheelFrictionCurve wfc = new WheelFrictionCurve();
		wfc.extremumSlip = 1;
		wfc.extremumValue = 50000;
		wfc.asymptoteSlip = 20;
		wfc.asymptoteValue = 50000;
		wfc.stiffness = 4;
		
		return wfc;
	}
	
	void SetupWheelCollider( WheelCollider wc, bool isLeft, bool isFront)
	{

		wc.center = new Vector3( (isLeft) ? -.25f : .25f, wc.center.y,  wc.center.z);
		wc.radius = wheelRadius;
		wc.suspensionDistance = suspensionRange;
		wc.mass = rigidbody.mass/5;
		
		JointSpring js  = wc.suspensionSpring;
		
		js.spring = (isFront) ? suspensionFrontSpring : suspensionRearSpring;
		js.damper = suspensionDamper;
		js.targetPosition = 0;

		wc.suspensionSpring = js;
		wc.motorTorque = 0;
		wc.brakeTorque = 0;
		
		WheelFrictionCurve wfc = SetupWheelFrictionCurve();
	
		wc.forwardFriction = wfc;
		wc.sidewaysFriction = wfc;
		
		
	}

	float WheelColliderSpeed
	{
		set
		{
			float push = 1000;
			foreach ( WheelCollider wc in wheels)
			{
				//wc.motorTorque += push * direction;
			}	
		}
		get
		{
			float deltaSpeed = 0;
			foreach ( WheelCollider wc in wheels)
			{
				deltaSpeed += wc.rpm;
			}	
			return deltaSpeed / 4;
		}
	}
	
	bool WheelColliderGrounded()
	{
		foreach ( WheelCollider wc in wheels)
		{
			
			if ( !wc.isGrounded )return false;
			
		}

		return true;
	}
	//-----------------------------------------------------------------------------CAMERA SETUP
	
	public float distanceMin = .5f;
    public float distanceMax = 15f;
	
	public Camera camera;
	
	public float distance = 1.5f;
	public float height = 0.6f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	public float camEulerX = 90;
	public float camEulerY = 10;
	
	private bool rampMode_reg = false;
	private bool rampMode_fak = false;

	float wantedAngle_x;
	float camPlus = 0;
	
	void CameraActivator()
	{
		CameraAdjust( camera.transform );
	}
	void CameraAdjust( Transform camera )
	{

		camEulerX = transform.eulerAngles.y - ((direction == 1 ) ? 90 : 270);	

		distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);
	
		float wantedHeight  = transform.position.y + height + camPlus;
		float currentHeight = camera.position.y;
				
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		Quaternion rotation = Quaternion.Euler(camEulerY, camEulerX, 0);

		Vector3 camPos = new Vector3(transform.position.x, transform.position.y + height/2 + camPlus, transform.position.z );
		Vector3 negDistance = new Vector3(0 , 0, -distance);
        Vector3 position = rotation * negDistance + camPos;
 
    //  camera.rotation = rotation;
        camera.position = position;
		
		camera.LookAt( new Vector3(transform.position.x, transform.position.y + height/2, transform.position.z ));

	}
	
	
	
	//-----------------------------------------------------------------------------GUI ELEMENTS
	void OnGUI()
	{
		GUI_Board();
		GUI_INFO_LABELS();
	}
	
	void GUI_Board()
	{
		Rect viewRect = new Rect(50, 150, 183, 365);
		Rect boardRect = new Rect( 0, 0, 183, 365 );

		GUI.BeginGroup(	viewRect );
	
			GUI.DrawTexture( boardRect, _textures[ ( direction == 1 ) ? 2 : 3 ] );
			GUI.DrawTexture( boardRect, _textures[1]);
		//	GUI.DrawTexture( boardRect, _textures[0]);	
	
		GUI.EndGroup();
		
	}
	void GUI_INFO_LABELS()
	{
		if(GUI.Button(new Rect(10,10,200,20), "RESET")) {
			
			transform.position = startPosition;
			transform.rotation = startRotation;
			
			rigidbody.velocity = Vector3.zero;

			Awake();
		}
		
		if(wheels != null )GUI.Label(new Rect( 1700, 10, 200, 20 ), "GROUNDED : " +  WheelColliderGrounded());
		
		float wcSpeed= 0;
		float brake=0;
		
		foreach ( WheelCollider wc in wheels){
			wcSpeed += wc.motorTorque;
			brake += wc.brakeTorque;
		}		
		
		GUI.Label(new Rect( 1700, 30, 200, 20 ), "RPM   : " + Mathf.Round(WheelColliderSpeed));
		GUI.Label(new Rect( 1700, 50, 200, 20 ), "MOTOR : " + wcSpeed/4 );
		GUI.Label(new Rect( 1700, 70, 200, 20 ), "BRAKE : " + brake/4 );
	}
}
