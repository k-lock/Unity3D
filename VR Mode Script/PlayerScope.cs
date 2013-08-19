using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScope : MonoBehaviour
{

    public float scrollSpeed = 0.5F;
    public float pulseSpeed = 1.5F;
    public float noiseSize = 1.0F;
    public float maxWidth = 0.5F;
    public float minWidth = 0.2F;
    private float maxDist = 20.0f;

    public Transform gunPos;
    public Transform gunDirection;
    public GameObject pointer = null;

    private Renderer  renderer;
    private LineRenderer lRenderer;
    private float aniTime = 0.0F;
    private float aniDir = 1.0F;
    private float LASER_Y_POS = 1.6f;
    

    private bool RIGHT_MOUSE = false;
    private bool LEFT_MOUSE = false;

    private bool SHOW_DEBUG = true;

    void Start()
    {
        lRenderer = gameObject.GetComponent<LineRenderer>();// as LineRenderer;	
        renderer = gameObject.renderer;
	    aniTime = 0.0f;
	
	    // Change some animation values here and there
	   // ChoseNewAnimationTargetCoroutine();
	
       
    }
    void Update()
    {
        UserInput();
        DrawLaserScope();
    }
   /* IEnumerator ChoseNewAnimationTargetCoroutine () {
	    while (true) {
		    aniDir = aniDir * 0.9 +  Random.Range (0.5, 1.5) * 0.1;
		    yield ;
		    minWidth = minWidth * 0.8 + Random.Range (0.1, 1.0) * 0.2;
		   return new yield WaitForSeconds (1.0 + Random.value * 2.0 - 1.0);	
	    }	
    }*/

    void UserInput()
    {
        if (Input.GetMouseButtonDown(1)) RIGHT_MOUSE = true;
        if( Input.GetMouseButtonUp(1)) RIGHT_MOUSE = false;

    }

    void DrawLaserScope(){

       
        if (!RIGHT_MOUSE)
        {
            lRenderer.renderer.enabled = false;
            pointer.renderer.enabled = false;
            return;
        }


        renderer.material.mainTextureOffset = new Vector2(
            renderer.material.mainTextureOffset.x +( Time.deltaTime * aniDir * scrollSpeed ),
            renderer.material.mainTextureOffset.y );
        renderer.material.SetTextureOffset("_NoiseTex",new Vector2(-Time.time * aniDir * scrollSpeed, 0.0f));

        float aniFactor = Mathf.PingPong(Time.time * pulseSpeed, 1.0f);
	    aniFactor = Mathf.Max (minWidth, aniFactor) * maxWidth;

        lRenderer.renderer.enabled = true;
        lRenderer.SetPosition(0, transform.InverseTransformPoint(gunPos.transform.position));
        lRenderer.SetWidth (aniFactor, aniFactor);

        RaycastHit hitInfo = RayHiter();
      
        if ( hitInfo.transform.gameObject.layer != 10 )
        {
            Vector3 hitPoint = new Vector3(hitInfo.point.x, gunPos.transform.position.y, hitInfo.point.z);
            Debug.Log(hitInfo.transform.name);
            lRenderer.SetPosition(1, transform.InverseTransformPoint(hitPoint));
         
		    renderer.material.mainTextureScale = new Vector2( 0.1f * (hitInfo.distance+.5f), renderer.material.mainTextureScale.y);   
            renderer.material.SetTextureScale ("_NoiseTex", new Vector2 (0.1f * hitInfo.distance * noiseSize, noiseSize));		
		
		    // Use point and normal to align a nice & rough hit plane
		    if (pointer) {
			    pointer.renderer.enabled = true;
                pointer.transform.position = hitPoint + (transform.position - hitPoint) * 0.01f;
                pointer.transform.rotation = Quaternion.LookRotation(hitPoint, transform.up);
			    pointer.transform.eulerAngles = new Vector3( pointer.transform.eulerAngles.y, 90.0f, pointer.transform.eulerAngles.z);
		    }
	    } else {
            if (pointer)
            {
                pointer.renderer.enabled = false;
            }
		    lRenderer.SetPosition (1, (maxDist * Vector3.forward));
		    renderer.material.mainTextureScale= new Vector2( 0.1f * (maxDist), renderer.material.mainTextureScale.y);		
		    renderer.material.SetTextureScale ("_NoiseTex", new Vector2 (0.1f * (maxDist) * noiseSize, noiseSize));		
	    }
    }

    RaycastHit RayHiter()
    {
        RaycastHit hit;

        if (Physics.Raycast(gunPos.position, gunDirection.position - gunPos.position, out hit))//, mask))
        {
     //       if (SHOW_DEBUG) Debug.DrawRay(gunPos.position, gunDirection.position - gunPos.position, Color.blue);

            return hit;
        }
        return hit;
    }
}

