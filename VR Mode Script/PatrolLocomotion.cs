using UnityEngine;
using System.Collections;

public class PatrolLocomotion : MonoBehaviour
{
    #region variables
    private string _locoState = "Locomotion_Stand";
    public Animation _anim;
    public int animID = 0;
    public float animWalk = .6f;
    public float animRun = 1.2f;
    public string[] AI_ANIMATIONS = new string[] 
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
    #endregion
    #region unity

    void Awake()
    {
        AnimationSetup();
       
    }

    void Update()
    {

    }


    #endregion
    public void Stop()
    {

        _anim.Stop();

    }


    public void Play(int id)
    {
        string mode = AI_ANIMATIONS[id];
        if (mode != "")
            _anim.Play(mode);
        animID = id;
        AnimSpeed(id);
    }
    public void AnimSpeed(int id)
    {
        switch (id)
        {
            case 0:
            case 9:
            case 8:
            case 10:
                GetComponent<NavMeshAgent>().speed = 0;
                break;
            case 1:
                GetComponent<NavMeshAgent>().speed = animWalk;
                break;
            case 2:
            case 3:
                GetComponent<NavMeshAgent>().speed = animWalk*.5f;
                break;
            case 4:
                GetComponent<NavMeshAgent>().speed = animRun;
                break;
            case 5:
            case 6:
                GetComponent<NavMeshAgent>().speed = animRun * .5f;
                break;
        }

    }
    private void AnimationSetup()
    {
        if (_anim == null) _anim = GetComponent<Animation>();
        _anim.wrapMode = WrapMode.Loop;

        // loop in sync
        _anim["Idle"].layer = 1;                     // 0

        _anim["Walk"].layer = 1;                     // 1
        _anim["WalkAim"].layer = 1;                  // 2
        _anim["WalkFire"].layer = 1;                 // 3

        _anim["Run"].layer = 1;                      // 4
        _anim["RunAim"].layer = 1;                   // 5
        _anim["RunFire"].layer = 1;                  // 6
        _anim["RunJump"].layer = 1;                  // 7

        _anim["Standing"].layer = 1;                 // 8
        _anim["StandingFire"].layer = 1;             // 9
        _anim["StandingAim"].layer = 1;              // 10
        _anim["StandingJump"].layer = 1;             // 11

        _anim["RelaxedWalk"].layer = 1;              // 12

        //GetComponent<NavMeshAgent>().speed = .35f;

        _anim.SyncLayer(1);
        //_anim.CrossFade ("Idle", 0.5f, PlayMode.StopAll);

    }
    /*	#region Loco Blends
        private void Locomotion_Stand ()
        {
            do {
                UpdateAnimationBlend ();
			
            } while(_agent.remainingDistance == 0);

            _locoState = "Locomotion_Move";
		
        }

        private void Locomotion_Move ()
        {
            do {
                UpdateAnimationBlend ();
			

                if (_agent.isOnOffMeshLink) {
                    _locoState = SelectLinkAnimation ();
			
                }
            } while(_agent.remainingDistance != 0);

            _locoState = "Locomotion_Stand";
		
        }

        private void UpdateAnimationBlend ()
        {
            float speedThreshold = 0.01f;
            Vector3 velocityXZ = new Vector3 (_agent.velocity.x, 0.0f, _agent.velocity.z);
            float speed = velocityXZ.magnitude;
	
            _anim ["Run"].speed = speed / animRun;
            _anim ["Walk"].speed = speed / animWalk;

            if (speed > (animWalk + animRun) / 1.0f) {
                _anim.CrossFade ("Run");
            } else if (speed > speedThreshold) {
                _anim.CrossFade ("Walk");
            } else {
                _anim.CrossFade ("Idle", 0.1f, PlayMode.StopAll);
            }
        }
        #endregion
        #region Animation
        private void AnimationSetup ()
        {
		

            // loop in sync
            _anim ["Walk"].layer = 1;
            _anim ["Run"].layer = 1;

            _anim.SyncLayer (1);

            // speed up & play once
            _anim ["RunJump"].wrapMode = WrapMode.ClampForever;
            _anim ["RunJump"].speed = animWalk;
	
            _anim.CrossFade ("Idle", 0.1f, PlayMode.StopAll);
        }
	
        private string SelectLinkAnimation ()
        {
	
            OffMeshLinkData link;
            //_agent.GetCurrentOffMeshLinkData(link);
            link = _agent.currentOffMeshLinkData;
	
            float distS = (transform.position - link.startPos).magnitude;
            float distE = (transform.position - link.endPos).magnitude;
	
            if (distS < distE) {
                _linkStart = link.startPos;
                _linkEnd = link.endPos;
            } else {
                _linkStart = link.endPos;
                _linkEnd = link.startPos;
            }

            Vector3 alignDir = _linkEnd - _linkStart;
            alignDir.y = 0;
            _linkRot = Quaternion.LookRotation (alignDir);

            if (link.linkType == OffMeshLinkType.LinkTypeManual) {
                return "Locomotion_LadderAnimation";
            } else {
                return "Locomotion_JumpAnimation";
            }
        }
        #endregion*/

}
