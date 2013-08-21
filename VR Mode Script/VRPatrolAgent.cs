using UnityEngine;
using System.Collections;
using System;

public enum AgentState
{
    ALERT_SEARCH,
    ALERT_AIM,
    ALERT,
    INIT,
    PATROL
}

public class VRPatrolAgent : MonoBehaviour, IkFOV
{
    public int index;
    //   public Transform prefab;
    public Vector3 start;
    public Vector3 end;

    private SphereCollider touchRadius;

    public bool onPatrol = false;
    private AgentState agentState = AgentState.INIT;
    private NavMeshAgent agent;
    private PatrolLocomotion loco;
    private kFOV fov;
    private Transform lookTarget = null;
    private bool _alertMode = false;
    private float ALERT_OFF_WAIT_TIME = 2;

    private float AGENT_SPEED = .5f;
    private bool findAlertLocation = false;
    private Transform[] alertLocationPath = null;
    Transform tileHMM;

    bool isEnd = true;

    void Awake()
    {


    }
    IEnumerator Start()
    {
        PathNet pathNet = GameObject.Find("PATHNET").GetComponent<PathNet>();
        start = pathNet.PathList[index].start;
        end = pathNet.PathList[index].end;

        Transform instance = transform;// Instantiate(prefab, start.position, Quaternion.identity) as Transform;
        instance.localScale = new Vector3(.4f, .4f, .4f);
        //    instance.tag = "kFOV";
        instance.parent = GameObject.Find("ENEMYS").transform;

        fov = instance.FindChild("kFOV").GetComponent<kFOV>();
        fov.index = index;
        fov.listener = this;
        fov.mask = ~((1 << 8) | (1 << 11));
        fov.SCAN_TAG = "kFOV";
        fov.ALERT_TAG = "Player";

        agent = instance.GetComponent<NavMeshAgent>();
        agent.speed = 0;

        loco = instance.GetComponent<PatrolLocomotion>();
        loco.Play(0);

        agentState = AgentState.INIT;

        yield return new WaitForSeconds(2);

        agent.SetDestination(end);
        agent.speed = AGENT_SPEED;
        onPatrol = true;
        loco.Play(12);

        agentState = AgentState.PATROL;

    }


    void Update()
    {
        Mover3();
    }

    void Mover3()
    {
        float dist = agent.remainingDistance;
        float playerDis = Vector3.Distance(GameObject.Find("SoldierBluePlayer").transform.position, agent.transform.position);
        float nearDistance = .75f;




        if (agentState == AgentState.PATROL)
        {

            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < .025f)
            {
                StartCoroutine(WaitToMove());
            }
        }
    }

    void Mover2()
    {
        float dist = agent.remainingDistance;
        float playerDis = Vector3.Distance(GameObject.Find("SoldierBluePlayer").transform.position, agent.transform.position);
        float nearDistance = .75f;
        /* if (fov.ON_ALERT || playerDis < nearDistance)
         {
             if (_alertMode)
             {
                 lookTarget = GameObject.Find("SoldierBluePlayer").transform;
                 SetLookTarget();
             }
             else
             {
                 _alertMode = true;
                 lookTarget = GameObject.Find("SoldierBluePlayer").transform;

                 if (playerDis > nearDistance)
                 {
                     agent.speed = AGENT_SPEED*1.5f;
                     agent.SetDestination(lookTarget.position);
                     if (loco.animID != 5 ) loco.Play(5);
                 }
                 else
                 {
                     agent.speed = 0f;
                     agent.SetDestination(transform.position);
                     loco.Play(10);
                 }
                 onPatrol = false;  
             }
         }
         else if (!fov.ON_ALERT == _alertMode &&  playerDis > nearDistance) {
             findAlertLocation = true;
             _alertMode = true;
             StartCoroutine(SetHmmModeOn(3));

         }else */
        if (_alertMode)
        {
            if (playerDis < nearDistance)
            {
                if (loco.animID != 10)
                {
                    agent.speed = 0f;
                    agent.SetDestination(transform.position);
                    SetLookTarget();
                    loco.Play(10);
                }
            }
            else
            {

                /* if (loco.animID != 5)
                 {
                     agent.speed = AGENT_SPEED * 1.5f;
                     agent.SetDestination(SetLookTarget());

                     loco.Play(5);
                 }*/

                if (tileHMM == null) StartCoroutine(SetHmmModeOn(3));

            }
        }
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < .025f)
        {
            if (onPatrol)
            {
                StartCoroutine(WaitToMove());
            }
            if (findAlertLocation)
                findAlertLocation = false;

        }
        //if (_alertMode) fov.ON_ALERT = true; 
    }



    /// -----------------------------------------------OLD
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>

    /* void Mover()
     {
         ///     
         float dist = agent.remainingDistance;
         float playerDis = Vector3.Distance(GameObject.Find("SoldierBluePlayer").transform.position, agent.transform.position);
         float nearDistance = .75f;

         if (GameObject.Find("SoldierBluePlayer") != null && agent != null)
         {
             playerDis = Vector3.Distance(GameObject.Find("SoldierBluePlayer").transform.position, agent.transform.position);
             if (playerDis < nearDistance)
             {
                 _alertMode = true;
                 lookTarget = GameObject.Find("SoldierBluePlayer").transform;
             }
         }
         //	Debug.Log (dist+""+agent.pathStatus+""+agent.remainingDistance);
         if (fov.ON_ALERT || playerDis < nearDistance)
         {
             if (_alertMode)
             {
                 lookTarget = GameObject.Find("SoldierBluePlayer").transform;
                 SetLookTarget();
              
             }
             else
             {
                 _alertMode = true;
                 lookTarget = GameObject.Find("SoldierBluePlayer").transform;
                
                 agent.speed = 0f;
                 onPatrol = false;
                 loco.Play(10);
       //          SetAlertModeOn(lookTarget);
             }
         }//else  _alertMode = false;
    
         else if (playerDis < dist && findAlertLocation)
         {
             //  loco.animation.CrossFade("Idle", 0.2f, PlayMode.StopAll);
             _alertMode = true;
             onPatrol = findAlertLocation = false;

         }
         if (dist != Mathf.Infinity &&
          agent.pathStatus == NavMeshPathStatus.PathComplete &&
          agent.remainingDistance < .025f)
         {
           
             if (onPatrol) StartCoroutine(WaitToMove());
             if (findAlertLocation)
             {
              //   loco.animation.CrossFade("Idle", 0.2f, PlayMode.StopAll);
                 _alertMode = true;
                 onPatrol = findAlertLocation = false;
             }

         }



         //    Debug.Log(agent.transform.position + " " +agent.pathEndPosition);

     //    Debug.DrawLine(agent.transform.position, agent.pathEndPosition, Color.yellow);
     //    Debug.DrawLine(agent.transform.position, GameObject.Find("SoldierBluePlayer").transform.position, ((playerDis < nearDistance) ? Color.red : Color.cyan));

     }*/

    void SetAlertFocus()
    {
        VRPatrolAgent[] all = GameObject.Find("ENEMYS").GetComponentsInChildren<VRPatrolAgent>();
        //GameObject.Find("ENEMYS").transform.GetComponentsInChildren<VRPatrolAgent>();
        foreach (VRPatrolAgent pg in all)
        {
            if (!pg._alertMode)
                if (pg.fov.index != fov.index)
                {
                    AlertFindPathTo(pg);
                }
        }
    }
    public void AlertFindPathTo(VRPatrolAgent pg)
    {
        //   Debug.Log("AlertFindPathTo " + pg.name + " " +pg.fov.index + " " + fov.index);
        //   if (pg.fov.index == fov.index)
        //   {
        pg.findAlertLocation = true;
        pg._alertMode = true;

        BuildAlertLocationPath(pg);
        //    }

    }
    private void BuildAlertLocationPath(VRPatrolAgent pg)
    {
        alertLocationPath = null;
        PathSingle p1 = GameObject.Find("PATHNET").GetComponent<PathNet>().PathList[0];
        PathSingle p2 = GameObject.Find("PATHNET").GetComponent<PathNet>().PathList[1];

        Transform player = GameObject.Find("SoldierBluePlayer").transform;

        int id = -1;
        float d = 999;
        float dd = Vector3.Distance(p1.start, player.position);
        if (dd < d)
        {
            d = dd; id = 0;
        }
        dd = Vector3.Distance(p1.end, player.position);
        if (dd < d)
        {
            d = dd; id = 1;
        }
        dd = Vector3.Distance(p2.start, player.position);
        if (dd < d)
        {
            d = dd; id = 2;
        }
        dd = Vector3.Distance(p2.end, player.position);
        if (dd < d)
        {
            d = dd; id = 3;
        }

        Vector3 p = (p1.start + p1.end + p2.start + p2.end) / 4;

        switch (id)
        {
            case 0: p = p1.start;
                break;
            case 1: p = p1.end;
                break;
            case 2: p = p2.start;
                break;
            case 3: p = p2.end;
                break;
        }
        pg.lookTarget = player;
        pg.agent.SetDestination(p);
        pg.agent.speed = .5f;

        pg.loco.Play(5);
        pg._alertMode = false;
    }

    /*public void SetAlertModeOn(Transform posi)
    {
     //   if (findAlertLocation == true) return;

        lookTarget = posi;
        Vector3 lookAtPos = lookTarget.position;

        // check for free view to target
        Ray ray = new Ray(agent.transform.position, lookAtPos - agent.transform.position + new Vector3(0, .5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                fov.GetComponent<MeshRenderer>().material.color = fov.color2;
                agent.SetDestination(posi.position);
                agent.speed = 0f;
                onPatrol = false;
                loco.Play(10);
                _alertMode = true;
           
                SetAlertFocus();
                CutPlayerLife();

                if (tileHMM == null)
                {
                    GameObject go = Instantiate(Resources.Load("Tile_Hmm")) as GameObject;
                    tileHMM = go.transform;
                    tileHMM.localPosition = new Vector3(agent.transform.position.x, 1.5f, agent.transform.position.z);
                    tileHMM.parent = agent.transform;
                }
            }
            else
            {
                StartCoroutine(SetAlertModeOff(ALERT_OFF_WAIT_TIME));
            }
        }
        //if(fov.showDebug) Debug.DrawRay(agent.transform.position, lookAtPos - agent.transform.position,  (hit.collider.tag == "Player")  ? Color.red :Color.yellow);
    }
    IEnumerator SetAlertModeOff(float alertTime)
    {
        yield return new WaitForSeconds(alertTime);

        if (_alertMode)
        {
            StartCoroutine(SetHmmModeOn(2));
        }
    }*/
    IEnumerator SetHmmModeOn(float time)
    {
        loco.Play(8);
        //look around
        // find point last nown player position , path, own position
        if (tileHMM == null)
        {
            GameObject go = Instantiate(Resources.Load("Tile_Hmm")) as GameObject;
            tileHMM = go.transform;
            tileHMM.localPosition = new Vector3(agent.transform.position.x, 1.5f, agent.transform.position.z);
            tileHMM.parent = agent.transform;
        }

        yield return new WaitForSeconds(time);

        StartCoroutine(SetHmmModeOff(1));
    }
    IEnumerator SetHmmModeOff(float time)
    {
        yield return new WaitForSeconds(time);
        if (_alertMode)
        {
            agent.SetDestination(SetLookTarget());
            //agent.SetDestination (posi);
            agent.speed = 0.75f;
            onPatrol = true;
            loco.Play(2);
            lookTarget = null;
            _alertMode = false;

            if (tileHMM != null)
            {
                Destroy(tileHMM.gameObject);
            }
        }
    }
    /// -----------------------------------------------OLD
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    Vector3 SetLookTarget()
    {
        Transform player = GameObject.Find("SoldierBluePlayer").transform;
        Vector3 lookAtPos = player.position;
        lookAtPos.y = agent.transform.position.y;

        agent.transform.LookAt(lookAtPos);
        lookTarget = player;

        return lookAtPos;
    }

    IEnumerator WaitToMove()
    {
        //	float dist2 = Vector3.Distance (agent.transform.position, end.position);
        //	bool isEnd = ((dist2 < 1) ? true : false);
        //  Debug.Log(fov.index + " " + isEnd);
        //   loco.animation.CrossFade(loco.AI_ANIMATIONS[8], 0.2f, PlayMode.StopAll);
        loco.Play(0);

        onPatrol = false;

        if (isEnd)
        {
            agent.SetDestination(start);
            isEnd = !isEnd;
        }
        else
        {
            agent.SetDestination(end);
            isEnd = !isEnd;
        }
        agent.transform.LookAt(agent.destination);

        yield return new WaitForSeconds(2);

        onPatrol = true;

        //loco.animation.CrossFade(loco.AI_ANIMATIONS[1], 0.5f, PlayMode.StopAll);
        loco.Play(1);
    }

    void CutPlayerLife()
    {
        PlayerAnimation pl = GameObject.Find("SoldierBluePlayer").GetComponent<PlayerAnimation>();
        float l = (pl.life -= .025f);
        if (l < 0) pl.life = 0;
        l = Mathf.Min(l, 35);
        l = Mathf.Max(l, 0);
        int frame = Mathf.RoundToInt(l);
        if (pl.State != PlayerState.OVER)
        {
            pl.PlayTileAnim(frame);
            if (frame == 0) pl.State = PlayerState.OVER;
        }
    }

    // iKOV Listener
    public void onColliderEnter(Collider collider)
    {
        if (collider.transform.name == "SoldierBluePlayer")
        {
            if (GetComponent<VRPatrolAgent>() != null)
            {
                //    SetAlertModeOn(collider.transform);
            }
        }
    }
    public void onColliderStay(Collider collider)
    {

      //  if (agentState == AgentState.PATROL)
      //  {
     SetLookTarget();

            float dist = agent.remainingDistance;
            float playerDis = Vector3.Distance(lookTarget.position, agent.transform.position);
            float nearDistance = .75f;
            float aimDistance = 1.5f;
            if (playerDis > nearDistance)
            {
                if (playerDis > aimDistance)
                {
                    agentState = AgentState.ALERT_SEARCH;
                    agent.SetDestination(lookTarget.position);
                    agent.speed = AGENT_SPEED * 1.25f;

                    loco.Play(5);
                }
            }
            else
            {
                agentState = AgentState.ALERT_AIM;
                agent.speed = 0;
                loco.Play(10);
            }

    //    }


        //      _alertMode = true;
        //       lookTarget = collider.transform;
        /*       if (collider.transform.name == "SoldierBluePlayer" )
              {
                  Debug.Log("SCAN PLAYER ");
              //    onPatrol = false;
              //    _alertMode = true;
                  findAlertLocation = true;
                  
                  agent.SetDestination(lookTarget.position);
                  SetLookTarget();
              }
       
              {
                  if (GetComponent<VRPatrolAgent>() != null)
                  {*/
        //     SetAlertModeOn(collider.transform);


        //     }
        // }
    }
    public void onColliderExit(Collider collider)
    {
        if (collider.transform.name == "SoldierBluePlayer")
        {
            if (GetComponent<VRPatrolAgent>() != null)
            {
                // if (_alertMode) StartCoroutine(SetAlertModeOff(ALERT_OFF_WAIT_TIME));
            }
        }
    }
}
