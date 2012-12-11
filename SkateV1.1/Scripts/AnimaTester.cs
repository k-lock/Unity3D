using UnityEngine;
using System.Collections;

public class AnimaTester : MonoBehaviour {

	[SerializeField]
	private Transform skater;
	[SerializeField]
	private Transform mainRoot;
	
	private Animation animator;
	
	private bool _direction = true;
	
	private bool _trickPosition = false;	
	private bool _varialPosition = true;
	
	
	
	public bool Direction
	{
		
		get
		{
			return _direction;
		}
		
		set
		{
			_direction = value;
			print("START - new Diretion : " + _direction );
			
			animator.CrossFade(( _direction ) ? "idle" : "fakie");
			
		}
		
	}

	
	void Start () 
	{
		
		animator = skater.gameObject.GetComponent<Animation>();
		animator.wrapMode = WrapMode.Clamp;
		
		animator["idle"].wrapMode = WrapMode.Once;
		animator["idle"].layer = -1;

		animator["idle2push"].wrapMode = WrapMode.Clamp;
		animator["idle2push"].layer = -1;
		
		animator["idlePush"].wrapMode    = WrapMode.Clamp;
		animator["idlePush"].layer = -1;
		
		animator["push2idle"].wrapMode = WrapMode.Clamp;
		animator["push2idle"].layer = -1;

		animator["fakie"].wrapMode = WrapMode.Once;
		animator["fakie"].layer = -1;
		
		animator["fakie2push"].wrapMode = WrapMode.Clamp;
		animator["fakie2push"].layer = -1;

		animator["fakiePush"].wrapMode    = WrapMode.Clamp;
		animator["fakiePush"].layer = -1;
	
		animator["push2fakie"].wrapMode = WrapMode.Clamp;
		animator["push2fakie"].layer = -1;

		animator["idle_TrickPosition"].wrapMode = WrapMode.Clamp;
		animator["idle_TrickPosition"].layer = -1;
		
		animator["fakie_TrickPosition"].wrapMode = WrapMode.Clamp;
		animator["fakie_TrickPosition"].layer = -1;
		
		animator["idle_bremsenIn"].wrapMode = WrapMode.Once;
		animator["idle_bremsenIn"].layer = -1;
		
		animator["idle_bremsenOut"].wrapMode = WrapMode.Once;
		animator["idle_bremsenOut"].layer = -1;
		
		animator["idle_HeelFlip"].wrapMode 	 = WrapMode.Clamp;
		animator["idle_KickFlip"].wrapMode 	 = WrapMode.Clamp;
		animator["idle_Ollie"].wrapMode 	 = WrapMode.Clamp;
		animator["idle_Varial_In"].wrapMode  = WrapMode.Clamp;
		animator["idle_Varial_Out"].wrapMode = WrapMode.Clamp;
		
		animator["fakie_HeelFlip"].wrapMode   = WrapMode.Clamp;
		animator["fakie_KickFlip"].wrapMode   = WrapMode.Clamp;
		animator["fakie_Ollie"].wrapMode 	  = WrapMode.Clamp;
		animator["fakie_Varial_In"].wrapMode  = WrapMode.Clamp;
		animator["fakie_Varial_Out"].wrapMode = WrapMode.Clamp;

		//animator.SyncLayer(-1);
		
		animator.Stop( );
	 
		//PlayClip( 0 );
		
		
	}
	
	void Update () 
	{
		
		if( Input.GetKeyDown(KeyCode.W)) PlayClip( 1 ); // push 
		if( Input.GetKeyDown(KeyCode.S)) PlayClip( 4 ); // bremsenIn
		if( Input.GetKeyUp	(KeyCode.S)) PlayClip( 5 ); // bremsenOut
		
	//	if( Input.GetKeyDown(KeyCode.A)) PlayClip( 4 );
		
		
		
		if( Input.GetKeyUp(KeyCode.Alpha1)) PlayClip( 14 ); 	// 	kickflip
		if( Input.GetKeyUp(KeyCode.Alpha2)) PlayClip( 15 ); 	// 	heelflip
		if( Input.GetKeyUp(KeyCode.Alpha3)) PlayClip( 16 ); 	// 	ollie
		if( Input.GetKeyUp(KeyCode.Alpha4)) PlayClip( 17 ); 	// 	varial		
		
		if(DownKey_Check()) 
		{
			if( !_trickPosition ) PlayClip( 2 );
		}
			
	}
	private bool DownKey_Check()
	{

		if( Input.GetKeyDown(KeyCode.Alpha0)) return true;
		if( Input.GetKeyDown(KeyCode.Alpha1)) return true;
		if( Input.GetKeyDown(KeyCode.Alpha2)) return true;
		if( Input.GetKeyDown(KeyCode.Alpha3)) return true;
		if( Input.GetKeyDown(KeyCode.Alpha4)) return true;
		if( Input.GetKeyDown(KeyCode.Alpha5)) return true;
		if( Input.GetKeyDown(KeyCode.Alpha6)) return true;
		if( Input.GetKeyDown(KeyCode.Alpha7)) return true;
		if( Input.GetKeyDown(KeyCode.Alpha8)) return true;
		if( Input.GetKeyDown(KeyCode.Alpha9)) return true;

		return false;
	}
	public void PlayClip(int clipIndex)
	{
		//print("PLAY CLIP : " + clipIndex + " ISplay : " + animator.isPlaying + "  | " + animator.clip.length);
		
		if(animator.isPlaying) return;
		
		switch(clipIndex)
		{
		case 0:
			animator.CrossFade(( Direction ) ? "idle" : "fakie");
			_trickPosition = false;
		break;
		case 1:
		
			animator.Play( ( Direction ) ? "idlePush" : "fakiePush");
			animator.PlayQueued( ( Direction ) ? "push2idle" : "push2fakie", QueueMode.CompleteOthers);
			_trickPosition = false;
		break;
		case 2:
			animator.Play(( Direction ) ?  "idle_TrickPosition" : "fakie_TrickPosition");
			_trickPosition = true;
		break;
		case 3:
				
		break;
		case 4:
			animator.Play("idle_bremsenIn");	
		break;
		case 5:
			animator.Play("idle_bremsenOut");
		break;		
		case 6:
			animator.CrossFade("fakie");
		break;	
	/*	case 7:
			
			animator.Play("idle2fakie");
			animator.PlayQueued("fakie", QueueMode.CompleteOthers);
		
		break;	
		case 8:
			
			animator.Play("fakie2idle");
			animator.PlayQueued("idle", QueueMode.CompleteOthers);
			
		break;	
	*/		
		
		
		
		case 14:
			animator.PlayQueued( ( Direction ) ? "idle_HeelFlip" 	: "fakie_HeelFlip",   QueueMode.CompleteOthers);
			_trickPosition = false;
			 AddWorldForce();
		break;
		case 15:
			animator.PlayQueued( ( Direction ) ? "idle_KickFlip" 	: "fakie_KickFlip",   QueueMode.CompleteOthers);
			_trickPosition = false;
			AddWorldForce();
		break;
		case 16:
			animator.PlayQueued( ( Direction ) ? "idle_Ollie" 	 	: "fakie_Ollie", 	  QueueMode.CompleteOthers);
			_trickPosition = false;
			 AddWorldForce();
		break;
		case 17:
			animator.PlayQueued( ( Direction ) ? ( !_varialPosition ) ? "idle_Varial_Out" : "idle_Varial_In" : ( !_varialPosition ) ? "fakie_Varial_Out" : "fakie_Varial_In",  QueueMode.CompleteOthers);
			_varialPosition = (_varialPosition) ? false : true;
			_trickPosition = false;
			 AddWorldForce();
			
		break;	
	

		}
		
		//_isPlayin = true;
		
		//print("|| -> ANIMATION SWITCH -> : " + _isPlayin);

	}
	void AddWorldForce()
	{
		//float speedDelta = ( mainRoot.rigidbody.velocity.x < 0 ) ? mainRoot.rigidbody.velocity.x*-1 : mainRoot.rigidbody.velocity.x;
		mainRoot.rigidbody.AddForce(0,5000,0);	
		
		
	}
	
	IEnumerator Wait(float seconds, int clipIndex) 
	{
		yield return new WaitForSeconds (seconds);
		PlayClip( clipIndex );
		
		print("PLAY NOW - " + clipIndex);
		
	}
	
}
