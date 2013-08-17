using UnityEngine;
using System.Collections;
using System;

public enum AgentState
{

    ALERT,
    INIT,
    WALK
}

public class VRPatrolAgent : MonoBehaviour, IkFOV
{
    public int index;
    public Transform tree;
    public Transform prefab;
    public Transform start;
    public Transform end;
    public bool onPatrol = false;
    private NavMeshAgent agent;
    private PatrolLocomotion loco;
    private kFOV fov;
    private Transform lookTarget = null;
    private bool _alertMode = false;
    private float ALERT_OFF_WAIT_TIME = 2;

    private bool findAlertLocation = false;
    private Transform[] alertLocationPath = null;

    void Awake()
    {

    }

    void SetAlertFocus()
    {
        VRPatrolAgent[] all = GameObject.Find("ENEMYS").transform.GetComponentsInChildren<VRPatrolAgent>();
          foreach (VRPatrolAgent pg in all)
          {
              if( !pg._alertMode )
              if (pg.fov.index != fov.index )
              {
                  AlertFindPathTo(pg);
              }
          }
    }

   public void AlertFindPathTo(VRPatrolAgent pg)
    {
        Debug.Log("AlertFindPathTo " + pg.name + " " +pg.fov.index + " " + fov.index);
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
       VRPatrolAgent p1 = GameObject.Find("ENEMYS").transform.FindChild("PATH-01").GetComponent<VRPatrolAgent>();
       VRPatrolAgent p2 = GameObject.Find("ENEMYS").transform.FindChild("PATH-02").GetComponent<VRPatrolAgent>();

       Transform player = GameObject.Find("SoldierBluePlayer").transform;

       int id = -1;
       float d = 999;
       float dd = Vector3.Distance( p1.start.position, player.position);
       if (dd < d)
       {
           d = dd;id = 0;
       }
       dd = Vector3.Distance(p1.end.position, player.position);
       if (dd < d) {
           d = dd;  id = 1;
       }
       dd = Vector3.Distance(p2.start.position, player.position);
       if (dd < d)
       {
           d = dd; id = 2;
       }
       dd = Vector3.Distance(p2.end.position, player.position);
       if (dd < d)
       {
           d = dd; id = 3;
       }

       Vector3 p = (p1.start.position + p1.end.position + p2.start.position + p2.end.position) / 4;

       switch (id)
       {
           case 0: p = p1.start.position;
               break;
           case 1: p = p1.end.position;
               break;
           case 2: p = p2.start.position;
               break;
           case 3: p = p2.end.position;
               break;
       }
       pg.lookTarget = player;
       pg.agent.SetDestination(p);
       pg.agent.speed = .5f;

       pg.loco.Play(1);
       pg._alertMode = false;
   }

    public void SetAlertModeOn(Transform posi)
    {
        if (findAlertLocation == true) return;
        lookTarget = posi;
        Vector3 lookAtPos = lookTarget.position;
        //	Debug.DrawLine (agent.transform.position, lookAtPos, Color.yellow);

        // check for free view to taarget
        Ray ray = new Ray(agent.transform.position, lookAtPos - agent.transform.position + new Vector3(0, .5f, 0));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Red")
            {
                fov.GetComponent<MeshRenderer>().material.color = fov.color2;
                agent.SetDestination(posi.position);
                agent.speed = 0f;
                onPatrol = false;
                loco.Play(4);
                _alertMode = true;

                SetAlertFocus();

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
        //if(fov.showDebug) Debug.DrawRay(agent.transform.position, lookAtPos - agent.transform.position,  (hit.collider.tag == "Red")  ? Color.red :Color.yellow);
    }

    IEnumerator SetAlertModeOff(float alertTime)
    {
        yield return new WaitForSeconds(alertTime);

        if (_alertMode)
        {
            StartCoroutine(SetHmmModeOn(2));
        }
    }
    Transform tileHMM;
    IEnumerator SetHmmModeOn(float time)
    {
        loco.Play(0);
        //look around
        // find point last nown player position , path, own position
        
        yield return new WaitForSeconds(time);

        StartCoroutine(SetHmmModeOff(1));
    }
    IEnumerator SetHmmModeOff(float time)
    {
        yield return new WaitForSeconds(time);
        if (_alertMode)
        {
            agent.SetDestination(end.position);
            //agent.SetDestination (posi);
            agent.speed = 0.5f;
            onPatrol = true;
            loco.Play(1);
            lookTarget = null;
            _alertMode = false;

            fov.GetComponent<MeshRenderer>().material.color = fov.color1;

            Destroy(tileHMM.gameObject);
        }
    }
    IEnumerator Start()
    {

        Transform instance = Instantiate(prefab, start.position, Quaternion.identity) as Transform;
        instance.localScale = new Vector3(.4f, .4f, .4f);
        instance.tag = "kFOV";
        instance.parent = transform;
        fov = instance.GetComponent<kFOV>();
        fov.index = index;
        fov.listener = this;
        fov.mask = ~((1 << 8));

        agent = instance.GetComponent<NavMeshAgent>();
        agent.speed = 0;

        loco = instance.GetComponent<PatrolLocomotion>();
        loco.Play(0);


        yield return new WaitForSeconds(2);

        if (!_alertMode)
        {
            //	Debug.Log ("Start Patrol Path");

            agent.SetDestination(end.position);
            agent.speed = .5f;
            onPatrol = true;
            loco.Play(1);
        }
    }
    public void onColliderEnter(Collider collider)
    {
        if (collider.transform.name == "SoldierBluePlayer")
        {
            if (GetComponent<VRPatrolAgent>() != null)
            {
                SetAlertModeOn(collider.transform);
            }
        }
    }
    public void onColliderStay(Collider collider)
    {
        if (collider.transform.name == "SoldierBluePlayer")
        {
            if (GetComponent<VRPatrolAgent>() != null)
            {
                SetAlertModeOn(collider.transform);
            }
        }
    }
    public void onColliderExit(Collider collider)
    {
        if (collider.transform.name == "SoldierBluePlayer")
        {
            if (GetComponent<VRPatrolAgent>() != null)
            {
              if(_alertMode)  StartCoroutine(SetAlertModeOff(ALERT_OFF_WAIT_TIME));
            }
        }
    }

    void Update()
    {

   ///     Debug.DrawLine(agent.transform.position, agent.nextPosition, Color.yellow);
   ///     
        float dist = agent.remainingDistance;
        float playerDis = Vector3.Distance(GameObject.Find("SoldierBluePlayer").transform.position, agent.transform.position);
        //	Debug.Log (dist+""+agent.pathStatus+""+agent.remainingDistance);
        if (_alertMode && lookTarget != null)
        {

            Vector3 lookAtPos = lookTarget.position;
            lookAtPos.y = agent.transform.position.y;
            agent.transform.LookAt(lookAtPos);



        }
        else if( playerDis < dist && findAlertLocation)
        {
          //  loco.animation.CrossFade("Idle", 0.2f, PlayMode.StopAll);
            _alertMode = true;
            onPatrol = findAlertLocation = false;

        }else if (dist != Mathf.Infinity &&
          agent.pathStatus == NavMeshPathStatus.PathComplete &&
          agent.remainingDistance == 0  )
        {

            if( onPatrol ) StartCoroutine(WaitToMove());
            if (findAlertLocation)
            {
                loco.animation.CrossFade("Idle", 0.2f, PlayMode.StopAll);
                _alertMode = true;
                onPatrol = findAlertLocation = false;
            }

        }
    }

    bool isEnd = false;

    IEnumerator WaitToMove()
    {
        //	float dist2 = Vector3.Distance (agent.transform.position, end.position);
        //	bool isEnd = ((dist2 < 1) ? true : false);

        loco.animation.CrossFade("Idle", 0.2f, PlayMode.StopAll);

        onPatrol = false;

        yield return new WaitForSeconds(2);

        loco.animation.CrossFade("Walk", 0.5f, PlayMode.StopAll);

        onPatrol = true;

        if (isEnd)
        {
            agent.SetDestination(start.position);
            isEnd = !isEnd;
        }
        else
        {
            agent.SetDestination(end.position);
            isEnd = !isEnd;
        }
    }



}
