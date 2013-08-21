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
    AudioSource sound_Walk;
    AgentMovementMotor agentMotor;
	Animation animComp;
	Vector3 lastPosition = Vector3.zero;
	Vector3 velocity = Vector3.zero;
	Vector3 localVelocity = Vector3.zero;
	float speed = 0;
	float angle = 0;
	float lowerBodyDeltaAngle = 0;
	float idleWeight = 0;

    public float walkingSpeed = 1.25f;

	
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
		lastPosition = rigidbody.transform.position;
		AnimationSetup ();
		Play (8);

      
       
	}
    public void PlayTileAnim(int frame)
    {
        if(animTile ==  null )animTile = GameObject.Find("GUI_GrowFactor_2048").GetComponent<TileAnim>();
        animTile._currentFrame = frame;
        animTile.MESH_refresh();
    }
    bool movin = false;
    bool runin = false;

    void Update()
    {
       if (state == PlayerState.OVER) return;

        //	Debug.DrawLine( transform.position ,transform.position+transform.eulerAngles, Color.green);
        if (Input.GetMouseButtonDown(0)) LEFT_MOUSE = true;
        if (Input.GetMouseButtonUp(0)) LEFT_MOUSE = false;

        if (Input.GetMouseButtonDown(1)) RIGHT_MOUSE = true;
        if (Input.GetMouseButtonUp(1)) RIGHT_MOUSE = false;

        
        if (Input.GetKeyDown(KeyCode.LeftShift) )
        {
            runin = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            runin = false;
        }
        

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
        {
            movin = true;
        }
        else
        {
            movin = false;
            runin = false;
        }

        if (movin)
        {
            if (runin)
            {
                if (RIGHT_MOUSE)
                    Play(5);
                else
                    Play(4);

                agentMotor.walkingSpeed = walkingSpeed * ((!RIGHT_MOUSE)?1.75f:1.25f);
            }
            else
            {
                if (RIGHT_MOUSE)
                    Play(2);
                else
                    Play(1);

                agentMotor.walkingSpeed = walkingSpeed * ((!RIGHT_MOUSE) ? .75f : .5f);
            }
          
            if( !sound_Walk.isPlaying ) sound_Walk.Play();
        }
        else
        {
            if (RIGHT_MOUSE)
                Play(10);
            else
                Play(8);

            agentMotor.walkingSpeed = 0;
            sound_Walk.Stop();
        }



        /*      if (Input.GetKeyDown(KeyCode.Space))
           {
                if (movin)
                   Play(7);
               else
                   Play(11);
           }

           animComp["Idle"].layer = 1;                     // 0

           animComp["Walk"].layer = 1;                     // 1
           animComp["WalkAim"].layer = 1;                  // 2
           animComp["WalkFire"].layer = 1;                 // 3

           animComp["Run"].layer = 1;                      // 4
           animComp["RunAim"].layer = 1;                   // 5
           animComp["RunFire"].layer = 1;                  // 6
           animComp["RunJump"].layer = 1;                  // 7

           animComp["Standing"].layer = 1;                 // 8
           animComp["StandingFire"].layer = 1;             // 9
           animComp["StandingAim"].layer = 1;              // 10
           animComp["StandingJump"].layer = 1;             // 11

           animComp["RelaxedWalk"].layer = 1;              // 12
*/
    //    idleWeight = Mathf.Lerp (idleWeight, Mathf.InverseLerp (.25f, 1.5f, speed), Time.deltaTime * 10);
    //    animComp ["Idle"].weight = idleWeight;

      
    }

	void FixedUpdate ()
	{
		velocity = (rigidbody.transform.position - lastPosition) / Time.deltaTime;
		localVelocity = rigidbody.transform.InverseTransformDirection (velocity);
		localVelocity.y = 0;
		speed = localVelocity.magnitude;
		angle = HorizontalAngle (localVelocity);
	
		lastPosition = rigidbody.transform.position;
	}

    public string[] PLAYER_ANIMATIONS = new string[] 
    {
        "Idle",
        "Walk",
        "WalkAim",
        "WalkFire",
        "Run",
        "RunAim",
        "RunFire",
        "RunJump",
        "Standing",
        "StandingFire",
        "StandingAim",
        "StandingJump",
        "RelaxedWalk"
    };
	private void AnimationSetup ()
	{
        if (sound_Walk == null)
            sound_Walk = transform.FindChild("WalkinSound").GetComponent<AudioSource>();

        if (agentMotor == null)
            agentMotor = GetComponent<AgentMovementMotor>();

		if (animComp == null)
			animComp = GetComponent<Animation> ();

		animComp.wrapMode = WrapMode.Loop;
                                                        // Play ( PLAYER_ANIMATIONS[index] )
		// loop in sync
        animComp["Idle"].layer = 1;                     // 0

        animComp["Walk"].layer = 1;                     // 1
        animComp["WalkAim"].layer = 1;                  // 2
        animComp["WalkFire"].layer = 1;                 // 3

        animComp["Run"].layer = 1;                      // 4
        animComp["RunAim"].layer = 1;                   // 5
        animComp["RunFire"].layer = 1;                  // 6
        animComp["RunJump"].layer = 1;                  // 7

        animComp["Standing"].layer = 1;                 // 8
        animComp["StandingFire"].layer = 1;             // 9
        animComp["StandingAim"].layer = 1;              // 10
        animComp["StandingJump"].layer = 1;             // 11

        animComp["RelaxedWalk"].layer = 1;              // 12

		animComp.SyncLayer (1);
		//animComp.CrossFade ("Idle", 0.5f, PlayMode.StopAll);
	
	}

	public void Play (int id)
    {
        string mode = PLAYER_ANIMATIONS[id];
        if (mode != "")
            animComp.Play(mode);
		/*string mode = "";
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
		}*/
	
	}
	

	static float HorizontalAngle (Vector3 direction)
	{
		return Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
	}
}
