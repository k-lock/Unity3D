using UnityEngine;

public class RayHitPerFrame : MonoBehaviour
{

    private RaycastHit hitInfo;
    private Transform tr;
   // private LayerMask mask = ~((1<<8));

    void Awake()
    {
        tr = transform;
    }
    void Update () {
	    // Cast a ray to find out the end point of the laser
        hitInfo = RayHiter();
    }
    RaycastHit RayHiter()
    {
        RaycastHit hit;
        if (Physics.Raycast(tr.position, tr.forward, out hit))//, mask))
        {
            return hit;
        }
        return hit;
    }

    public RaycastHit GetHitInfo ()  {
	    return hitInfo;
    }
}

