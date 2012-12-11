using UnityEngine;
using System.Collections;

public class SkateMovement : MonoBehaviour {
	
	
	private float throttle = 0; 
	private float steer = 0;
	
	private int direction = 1;

	private bool canSteer = true;
	private bool canDrive = true;
	
	private WheelFrictionCurve wfc ;
	
	public Transform skater;
//	public Transform centerOfMass;
//	public Transform wheelContainer;
	public WheelCollider[] wheels;
	public Texture[] _textures;

	public float wheelRadius = 0.07f;
	public float suspensionRange = 0.05f;
	public float suspensionDamper = 50f;
	public float suspensionFrontSpring = 2000;
	public float suspensionRearSpring  = 1000;
	
	// Follow Camera Setup
	public float distance = 1.5f;
	public float height = 0.6f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	
	public Camera[] cameras;
	[SerializeField]
	private int activeCamera = 0;
	
	private AnimaTester skaterAni;

	
	private Vector3 startPosition;
	private Quaternion startRotation;
	
	private Transform rampCollider;
	private bool camMovin = false;
	
	[SerializeField]
    bool _drawGizmos = true;
    [SerializeField]
    float _detectionRadius = 2.0f;

	public LayerMask _layersChecked;
	bool _scanView = true;
	
	
	void Awake() {
	
		skaterAni = skater.GetComponent<AnimaTester>();
	
		startRotation = transform.rotation;
		startPosition = transform.position;
	
		direction = 1;
		
		SetupWheelColliders();
	
	}

	void Start() {
		
	}
	
	void Update() {
	
		
		if(canDrive){
		
			print(RampChecker());
		
			if( RampChecker() ){
				if(Mathf.Sign(rigidbody.velocity.x) == 1){
					if( !rampMode_fak && !rampMode_reg ) rampMode_reg = true;
				}else{
					if( !rampMode_fak && !rampMode_reg  )rampMode_fak = true;
				}	
				
				DirectionCorrecter();
				
			}else{
				rampCollider = null;
				rampMode_fak = rampMode_reg = false;
			}
			
			//CopingCheck();
			GetInput();
	
			if(WheelColliderGrounded() ){
				
				Vector3 relativeVelocity = transform.InverseTransformDirection(rigidbody.velocity);
	
				ApplySteering( relativeVelocity );
				ApplyVelocity();
			//	ApplyThrottle();
				
			
			}
		
		}
		
	}
	
	void LateUpdate() {
		CameraActivator();
		
	}
	
	void OnGUI() {
		GUI_INFO_LABELS();
		GUI_Board();
	}
	
	/*void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            Vector3 pos = ((Camera) cameras[ activeCamera ]).transform.position;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(pos, _detectionRadius);
        }
    }*/
	//----------------------------------------------------------------------------------------OBSTABLECHECKER
	
	void CopingCheck()
	{
		RaycastHit hit;
       
		if (Physics.Raycast(transform.position, -Vector3.up, out hit))
		{

			if( hit.transform.gameObject.tag == "Coping 2 Trigger"){
				print("GRIND");	
				
			}
		}
		
	}
	
	bool RampChecker()
	{
		
		RaycastHit hit;
       
		if (Physics.Raycast(transform.position, -Vector3.up, out hit))
		{

			if( hit.transform.gameObject.tag == "GROUND"){
				
				//print( hit.transform.gameObject.tag + " : " + hit.distance);
				
				if(transform.position.y < 0.04f ) {

					transform.rotation = new Quaternion(0,1,0,0);
				
				/*	print("--------------------------");
					print("GROUND CORRECTOR ");	
					print("POS : " + oldPos + " - " + transform.position );
					print("ROT : " + oldRot + " - " + transform.rotation );
				*/
				}
			
			}
			if( hit.transform.gameObject.tag == "RAMP"){
				
				RampCollisionObject( hit.transform );
				
			//	print( hit.transform.gameObject.tag + " : " + hit.distance);
							
				if(transform.position.y < 0.02f ) transform.position = new Vector3( transform.position.x, 0.055f, transform.position.z );
				
				return true;		
			}
			
		}
		
		return false;
	}
	void RampCollisionObject( Transform hiter )
	{
		if(hiter.gameObject.name == "Quater") rampCollider = hiter;
		if(hiter.gameObject.name == "Bank")   rampCollider = hiter;
		if(hiter.gameObject.name == "Mini Set A 1") rampCollider = hiter;
		if(hiter.gameObject.name == "Mini Set A 2") rampCollider = hiter;
		if(hiter.gameObject.name == "Mini Set B 1") rampCollider = hiter;
		if(hiter.gameObject.name == "Mini Set B 2") rampCollider = hiter;
		if(hiter.gameObject.name == "Quater Pipe 1") rampCollider = hiter;		
		if(hiter.gameObject.name == "Quater Pipe 2") rampCollider = hiter;	
		
	}
	
	// ----------------------------------------------------------------------------------------MOVEMENT
	
	void GetInput()
	{
		throttle = Input.GetAxis("Vertical");
		steer = Input.GetAxis("Horizontal");
	}
	void ApplyVelocity()
	{
		Quaternion rota = Quaternion.Euler( 0, transform.eulerAngles.y + ((direction == 1 ) ? 180 : 0), 0);
		Vector3 forward = rota * Vector3.right + transform.position;
		
		Debug.DrawLine( transform.position, forward, Color.green);
	
		if(Input.GetKeyDown(KeyCode.W))
		{
			
		//	Vector3 schub = new Vector3( 5000 * direction, 0, 0 );
	
			rigidbody.velocity = new Vector3(6 * direction,0,0);
		//	rigidbody.AddForce( forward  * (2000 * direction ) );
			
			
			if(direction == 1 && rigidbody.velocity.x < 0 || direction == -1 && rigidbody.velocity.x > 0 ){
				rigidbody.velocity = new Vector3(rigidbody.velocity.x *-1, rigidbody.velocity.y, rigidbody.velocity.z);
			}
			
			//print(rigidbody.velocity );
			
			//rigidbody.centerOfMass = new Vector3(0,0,-1 * direction);

			
		}else{
			
			
			rigidbody.centerOfMass = new Vector3(0,0,0);
			
			
			
		}
		
		
		
		
		/*
		
		ApplyGears();
			
			float motor_max = 100.0f ;
			print("PUSH : "+ EngineTorque / GearRatio[CurrentGear]* direction);
			((WheelCollider)wheels[2]).motorTorque += EngineTorque / GearRatio[CurrentGear] * direction;
			((WheelCollider)wheels[3]).motorTorque += EngineTorque / GearRatio[CurrentGear] * direction;
			
			((WheelCollider)wheels[2]).brakeTorque = 0;
			((WheelCollider)wheels[3]).brakeTorque = 0;
		
			//	print( (((WheelCollider)wheels[2]).motorTorque < 0 && direction == 1)  + " || " + (((WheelCollider)wheels[2]).motorTorque > 0 && direction == -1 ) );
				
				if(((WheelCollider)wheels[2]).motorTorque < 0 && direction == 1 ) ((WheelCollider)wheels[2]).motorTorque = 0;
				if(((WheelCollider)wheels[3]).motorTorque < 0 && direction == 1 ) ((WheelCollider)wheels[3]).motorTorque = 0;
		
				if(((WheelCollider)wheels[2]).motorTorque > 0 && direction == -1 ) ((WheelCollider)wheels[2]).motorTorque = 0;
				if(((WheelCollider)wheels[3]).motorTorque > 0 && direction == -1 ) ((WheelCollider)wheels[3]).motorTorque = 0;
				
				foreach ( WheelCollider wc in wheels){
					if( wc.motorTorque > 1500 ){
						print("WheelCollider Drüber ");
						wc.motorTorque = 1000;	
					}
		}
		*/
		
	}
	void CheckMotorTorque()
	{
		if(((WheelCollider)wheels[0]).motorTorque < 0 && direction == 1 ) ((WheelCollider)wheels[0]).motorTorque = ((WheelCollider)wheels[0]).motorTorque*-1;
		if(((WheelCollider)wheels[1]).motorTorque < 0 && direction == 1 ) ((WheelCollider)wheels[1]).motorTorque = ((WheelCollider)wheels[1]).motorTorque*-1;
		if(((WheelCollider)wheels[2]).motorTorque < 0 && direction == 1 ) ((WheelCollider)wheels[2]).motorTorque = ((WheelCollider)wheels[2]).motorTorque*-1;
		if(((WheelCollider)wheels[3]).motorTorque < 0 && direction == 1 ) ((WheelCollider)wheels[3]).motorTorque = ((WheelCollider)wheels[3]).motorTorque*-1;
		
		if(((WheelCollider)wheels[0]).motorTorque > 0 && direction == -1 ) ((WheelCollider)wheels[0]).motorTorque = ((WheelCollider)wheels[0]).motorTorque*-1;
		if(((WheelCollider)wheels[1]).motorTorque > 0 && direction == -1 ) ((WheelCollider)wheels[1]).motorTorque = ((WheelCollider)wheels[1]).motorTorque*-1;
		if(((WheelCollider)wheels[2]).motorTorque > 0 && direction == -1 ) ((WheelCollider)wheels[2]).motorTorque = ((WheelCollider)wheels[2]).motorTorque*-1;
		if(((WheelCollider)wheels[3]).motorTorque > 0 && direction == -1 ) ((WheelCollider)wheels[3]).motorTorque = ((WheelCollider)wheels[3]).motorTorque*-1;
	}
	
	float EngineTorque = 100.0f;
	float EngineRPM;
	float MaxEngineRPM = 500.0f;
	float MinEngineRPM = 100.0f;
	
	public float[] GearRatio;
	int CurrentGear = 0;
	
	/*
	void ApplyGears()
	{
		
		EngineRPM = (((WheelCollider)wheels[2]).rpm + ((WheelCollider)wheels[3]).rpm)/2 * GearRatio[CurrentGear];
		//ShiftGears();
		
		if ( EngineRPM >= MaxEngineRPM ) {
			
			int AppropriateGear = CurrentGear;
		
			for ( int i = 0; i < GearRatio.Length; i ++ ) {
				print(((WheelCollider)wheels[2]).rpm * GearRatio[i] + " -> " + MaxEngineRPM);
				
				if ( ((WheelCollider)wheels[2]).rpm * GearRatio[i] < MaxEngineRPM ) {
					AppropriateGear = i;
					break;
				}
			}
		
			CurrentGear = AppropriateGear;
		}
		
		//print(CurrentGear);
	}
	
	*/
	void ApplyThrottle()
	{
		if(throttle < 0)
		{
	//		rigidbody.velocity = Vector3.zero;
	//		rigidbody.centerOfMass = new Vector3(0,0,0);
			
		//	foreach ( WheelCollider wc in wheels) wc.motorTorque = 0;	

		}	
		
	}
	void ApplySteering( Vector3 relativeVelocity )
	{

		if(canSteer && WheelColliderGrounded())
		{

		//	transform.Rotate( 90.0f * Vector3.up * Time.deltaTime * steer);
			
		/*	transform.RotateAround(	transform.position, 
									transform.up, 
									90* steer * Time.deltaTime );
			 */
			
			 float steer_max  = 10.0f;
			 float steerin = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
			
			if( direction == 1){
			
				((WheelCollider)wheels[0]).steerAngle = steer_max * steerin;
				((WheelCollider)wheels[1]).steerAngle = steer_max * steerin;	
				
			}else{
				
				((WheelCollider)wheels[2]).steerAngle = steer_max * steerin;
				((WheelCollider)wheels[3]).steerAngle = steer_max * steerin;	
				
			}
			
	/*		Quaternion currentRotation = Quaternion.Euler (0, transform.eulerAngles.y, 0);
			Vector3 debugStartPoint = transform.position;//* steer;
			Vector3 debugEndPoint = currentRotation * Vector3.left + debugStartPoint;
			
			Debug.DrawLine(debugStartPoint, debugEndPoint, Color.red);
			 */
		}
	}
	void DirectionCorrecter()
	{
		
		if( direction == 1 && Mathf.Sign( rigidbody.velocity.x ) == -1 || direction == -1 &&  Mathf.Sign( rigidbody.velocity.x ) == 1  ) {
			
			direction = direction *-1;
			//rigidbody.velocity *= -1; 
			//rigidbody.velocity += new Vector3( direction, 0, 0);
	
			skaterAni.Direction = ( direction == 1 ) ? true : false;
			
			//if(!camMovin)camMovin = true;
		}	
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
	
	void SetupWheelFrictionCurve()
	{
		wfc = new WheelFrictionCurve();
		wfc.extremumSlip = 1;
		wfc.extremumValue = 50;
		wfc.asymptoteSlip = 2;
		wfc.asymptoteValue = 25;
		wfc.stiffness = 1;
	}
	
	void SetupWheelCollider( WheelCollider wc, bool isLeft, bool isFront)
	{

		wc.center = new Vector3( (isLeft) ? -.25f : .25f, wc.center.y,  wc.center.z);
		wc.radius = wheelRadius;
		wc.suspensionDistance = suspensionRange;
		wc.mass = rigidbody.mass/20;
		
		JointSpring js  = wc.suspensionSpring;
		
		js.spring = (isFront) ? suspensionFrontSpring : suspensionRearSpring;
		js.damper = suspensionDamper;
		js.targetPosition = 0;

		wc.suspensionSpring = js;
		//wc.forwardFriction = wfc;
		//wc.sidewaysFriction = wfc;
		
		
	}

	float WheelColliderSpeed
	{
		set
		{
			float push = 1000;
			foreach ( WheelCollider wc in wheels)
			{
				wc.motorTorque += push * direction;
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
			if ( !wc.isGrounded ) return false;
		}

		return true;
	}
	//-----------------------------------------------------------------------------CAMERA SETUP
	
	public float distanceMin = .5f;
    public float distanceMax = 15f;
	
	public float camEulerX = 90;
	public float camEulerY = 10;
	
	private bool rampMode_reg = false;
	private bool rampMode_fak = false;
	
	float wantedAngle_x;
	
	float camPlus = 0;
	
	void CameraActivator()
	{
		if(Input.GetKeyDown (KeyCode.F1)) activeCamera = 0;
		if(Input.GetKeyDown (KeyCode.F2)) activeCamera = 1;
		if(Input.GetKeyDown (KeyCode.F3)) activeCamera = 2;
	//	if(Input.GetKeyDown (KeyCode.F4)) activeCamera = 3;
		
		foreach(Camera cam in cameras){ cam.enabled = false; }
		
		((Camera) cameras[ activeCamera ]).enabled = true;
		
		if(activeCamera == 0) CameraAdjust(((Camera) cameras[ activeCamera ]).transform);
	}
	void CameraAdjust( Transform camera )
	{
	
		//print(transform.eulerAngles.y);
		
		
		if( rampMode_reg ){
			
			if( Mathf.Sign( rigidbody.velocity.x ) == 1){
				
		//		print("UP" );
				wantedAngle_x = 0;
				camEulerX += ( -90 * 0.02f );
				
			//	camPlus += 0.01f;
				
			}else{

		//		print("DOWN" );
				wantedAngle_x = 270;
		//		camEulerX += -90 * 0.02f;

			}
			
			//camEulerX += (( wantedAngle_x == 90)? 180 : -180 ) * 0.02f;
			
		}else if( rampMode_fak ){
			
			if( Mathf.Sign( rigidbody.velocity.x ) == -1){
				
		//		print("UP");
				wantedAngle_x = 180;
				camEulerX += ( -90 * 0.02f );
				
			//	camPlus += 0.01f;
				
			}else{
				
		//		print("DOWN");
				wantedAngle_x = 90;
		//		camEulerX += -90 * 0.02f;
				
			}
				
		}else{
			
	/*		if(direction == 1 && camEulerX != transform.eulerAngles.y - 90){
	//			camEulerX += -90 * 0.02f;
			}else if(direction == -1 && camEulerX != transform.eulerAngles.y - 270){
	//			camEulerX += -90* 0.02f;
			}else{*/
				camEulerX = transform.eulerAngles.y - ((direction == 1 ) ? 90 : 270);	
		//	}
			camPlus = 0;
		}
		
	//	if( rampMode_fak || rampMode_reg )CameraCollision( camera );
		
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
	void CameraCollision( Transform camera )
	{
		
		Debug.DrawLine( camera.position, camera.position+ (-Vector3.up), Color.yellow );
		
		RaycastHit hit;
		if(Physics.Raycast( camera.position, -Vector3.up, out hit, _layersChecked)){
		//	print( hit.transform.gameObject.layer );
		//	if( hit.transform.gameObject.layer != 8 ) return;
	//		print(hit.transform.gameObject.name + " | " + hit.distance);
			
		//	if(hit.distance <0.3 ) camPlus = 1;
			
			if( hit.transform.gameObject.layer != 8 ) return;
			
			MeshCollider meshCollider = hit.collider as MeshCollider; 
			if (meshCollider != null || meshCollider.sharedMesh != null)
			{
				Mesh mesh = meshCollider.sharedMesh; 
				Vector3[] vertices = mesh.vertices; 
				int[] triangles = mesh.triangles; 
				
				Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
				Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];    
				Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];   
				
				Transform hitTransform = hit.collider.transform;
			    
				p0 = hitTransform.TransformPoint(p0);
			    p1 = hitTransform.TransformPoint(p1);
			    p2 = hitTransform.TransformPoint(p2);
				
				float pointDelta =  ( p0.y + p1.y + p2.y ) / 3;
			/*	print("POINT HEIGHT : " + pointDelta  + " ---> " + hit.transform.gameObject.name);
		
			    Debug.DrawLine(p0, p1);
			    Debug.DrawLine(p1, p2);
			    Debug.DrawLine(p2, p0);
			    */
				
				if(pointDelta < 0.05f) camPlus = 1.2f;
			}
		}	
	}
	
	/*float ClampAngle ( float angle, float min, float max ) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}*/
	//-----------------------------------------------------------------------------RAMP INFO
	
	
	
	//-----------------------------------------------------------------------------GUI ELEMENTS
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
		if(GUI.Button(new Rect(10,10,200,25), "RESET")) {
			
			transform.position = startPosition;
			transform.rotation = startRotation;
			
			rigidbody.velocity = Vector3.zero;

			Awake();
		}

		string dir = (direction == 1) ? "REGULAR" : "FAKIE";
		
		if(GUI.Button(new Rect(10,35,200,25), "DIRECTION " + dir)) {
			direction = direction *-1;
			rigidbody.velocity *= -1; 
			
			foreach ( WheelCollider wc in wheels)  wc.motorTorque = wc.motorTorque *-1;
				
			skaterAni.Direction = (skaterAni.Direction) ? false : true;
		}
		if(GUI.Button(new Rect(10,60,200,25), "GRAVITY " + rigidbody.useGravity )) rigidbody.useGravity  = (rigidbody.useGravity)? false : true;		
		if(GUI.Button(new Rect(10,85,200,25), "SUPER TRICKY" )){
			
			rigidbody.useGravity  = false;
			rigidbody.centerOfMass = new Vector3(0,0,0);
			rigidbody.velocity = Vector3.zero;
			
			transform.position = transform.position + Vector3.up * .5f;
		
		}
		
		
		GUI.Label(new Rect( 1700, 200, 200, 25 ), "RPM : " + WheelColliderSpeed);
		GUI.Label(new Rect( 1700, 225, 200, 25 ), "SPEED : " + rigidbody.velocity);
		GUI.Label(new Rect( 1700, 250, 200, 25 ), "DIRECTION : " + transform.forward);
		GUI.Label(new Rect( 1700, 275, 200, 25 ), "POSITION : "  + transform.position);		
		GUI.Label(new Rect( 1700, 300, 200, 25 ), "ROTATION : "  + transform.rotation);		
		if(wheels != null )GUI.Label(new Rect( 1700, 325, 200, 25 ), "GROUNDED : " +  WheelColliderGrounded());
		GUI.Label(new Rect( 1700, 350, 200, 25 ), "RAMP MODE REG: "  + rampMode_reg);	
		GUI.Label(new Rect( 1700, 375, 200, 25 ), "RAMP MODE FAK: "  + rampMode_fak);
	//	GUI.Label(new Rect( 1700, 400, 200, 25 ), "RAMP OBJ : "  + ((rampMode) ? rampCollider.gameObject.name : "NONE"));
		
	}
	
	//----------------------------------------------------------------------PUBLIC METHODS
	
	public float Steer 
	{
		get{
			return steer;	
		}

	}
	
	public bool CanDrive
	{
		get{
			return canDrive;
		}
		set{
			canDrive = value;
		}
	}
	
	public int Direction
	{
		get{
			return direction;
		}
		set{
			direction = value;
		}
	}
}
