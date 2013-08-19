using UnityEngine;
using System.Collections;

public enum PlayerState
{

    INIT,
    ALIVE,
    OVER
}
public class PlayerAnimation : MonoBehaviour
{
	
	Animation animComp;
	Vector3 lastPosition = Vector3.zero;
	Vector3 velocity = Vector3.zero;
	Vector3 localVelocity = Vector3.zero;
	float speed = 0;
	float angle = 0;
	float lowerBodyDeltaAngle = 0;
	float idleWeight = 0;
	
	public float life = 35;
    private TileAnim animTile;
    private PlayerState state;
    public PlayerState State { get { return state; } set { state=value; }}

    public bool RIGHT_MOUSE = false;
    public bool LEFT_MOUSE = false;

	void Awake ()
	{
        state = PlayerState.ALIVE;
        PlayTileAnim(35);
      //  Debug.Log(GameObject.Find("GUI_GrowFactor_2048").GetComponent<TileAnim>());
		lastPosition = rigidbody.transform.position;
		AnimationSetup ();
		Play (0);
	}
    public void PlayTileAnim(int frame)
    {
        if(animTile ==  null )animTile = GameObject.Find("GUI_GrowFactor_2048").GetComponent<TileAnim>();
        animTile._currentFrame = frame;
        animTile.MESH_refresh();
    }
	void Update ()
	{
        if (state == PlayerState.OVER) return;
	//	Debug.DrawLine( transform.position ,transform.position+transform.eulerAngles, Color.green);
        if (Input.GetMouseButtonDown(0)) LEFT_MOUSE = true;
        if (Input.GetMouseButtonUp(0)) LEFT_MOUSE = false;

        if (Input.GetMouseButtonDown(1)) RIGHT_MOUSE = true;
        if (Input.GetMouseButtonUp(1)) RIGHT_MOUSE = false;

		if(Input.GetAxis ("Horizontal") !=0 || Input.GetAxis ("Vertical") !=0 ){
            if (RIGHT_MOUSE)
                Play(6);
            else
                Play (1);
		}else{
            if (RIGHT_MOUSE)
                Play(4);
            else
			    Play (0);
		}
        if (RIGHT_MOUSE)
        {
            
        }else{

        }
     
		/*idleWeight = Mathf.Lerp (idleWeight, Mathf.InverseLerp (.25f, 1.5f, speed), Time.deltaTime * 10);
		animComp ["Idle"].weight = idleWeight;*/
	}

	void FixedUpdate ()
	{
	/*	velocity = (rigidbody.transform.position - lastPosition) / Time.deltaTime;
		localVelocity = rigidbody.transform.InverseTransformDirection (velocity);
		localVelocity.y = 0;
		speed = localVelocity.magnitude;
		angle = HorizontalAngle (localVelocity);
	
		lastPosition = rigidbody.transform.position;*/
	}

	private void AnimationSetup ()
	{
		if (animComp == null)
			animComp = GetComponent<Animation> ();
		animComp.wrapMode = WrapMode.Loop;
		
		// loop in sync
		animComp ["Walk"].layer = 1;
		animComp ["Run"].layer = 1;
		animComp ["Standing"].layer = 1;
		animComp ["Idle"].layer = 1;
		animComp ["StandingFire"].layer = 1;
		animComp ["StandingAim"].layer = 1;
        animComp["WalkAim"].layer = 1;
        animComp["WalkFire"].layer = 1;

		animComp.SyncLayer (1);
		//animComp.CrossFade ("Idle", 0.5f, PlayMode.StopAll);
	
	}

	public void Play (int id)
	{
		string mode = "";
		switch (id) {
		case 0:
			mode = "Idle";
			break;
		case 1:
			mode = "Walk";
			break;
		case 2:
			mode = "Standing";
			break;
			case 3:
			mode = "StandingFire";
			break;
			case 4:
			mode = "StandingAim";
			break;
            case 5:
            mode = "WalkFire";
            break;
            case 6:
            mode = "WalkAim";
            break;
		}
		if (mode != "")
			animComp.Play (mode);
	}
	

	static float HorizontalAngle (Vector3 direction)
	{
		return Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
	}
}
