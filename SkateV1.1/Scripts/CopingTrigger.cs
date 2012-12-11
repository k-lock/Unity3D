using UnityEngine;
using System.Collections;

public class CopingTrigger : MonoBehaviour {
	
	
	public float slideXmin, slideXmax, slideY, slideZmin, slideZmax;
	
	public  Transform 		skater;
	private SkateMovement 	skaterScript;
	
	private Vector3 tempPos = Vector3.zero;
	private Vector3 tempRot;
	
	private int direction;
	private bool grinded = false;
	
	void Awake()
	{
	
		slideZmin = collider.bounds.center.z - .005f;
		slideZmax = collider.bounds.center.z + .005f;
		
		slideXmin = collider.bounds.center.x - ( collider.bounds.size.x * .5f );
		slideXmax = collider.bounds.center.x + ( collider.bounds.size.x * .5f );
		
		slideY = transform.position.y + .03f;
		
		skaterScript = skater.GetComponent<SkateMovement>();
	}
	
	void Start () {
	
	}
	
	void Update () {
		
	}
	
	void LateUpdate () {
		if( grinded ){
			
			skater.eulerAngles = tempRot;
			skater.position = new Vector3( skater.position.x, slideY, tempPos.z );
			skater.rigidbody.velocity += Vector3.left*( .09f * -skaterScript.Direction);

		}
	}	
	
	void OnTriggerEnter(Collider other) { OnTriggerEnter_EVENT(other);   }
	
	void OnTriggerEnter_EVENT(Collider other) {

		if( skaterScript.CanDrive ){
			
			tempRot = new Vector3(0, (skaterScript.Direction == 1) ? 180 : -180 ,0); // skater.eulerAngles.y
			tempPos  = new Vector3( Mathf.Clamp( skater.position.x, slideXmin, slideXmax), slideY, Mathf.Clamp( skater.position.z, slideZmin, slideZmax));
			
			grinded = true;
			skaterScript.CanDrive = false;
		
			skater.position = tempPos;
   			skater.eulerAngles = tempRot;
		
		}
    }
	
	void OnTriggerExit(Collider other) {
       	
		if( !skaterScript.CanDrive ){	
		
			skaterScript.CanDrive = true;
			grinded = false;
	
			tempPos = tempRot = Vector3.zero;
			
			print("KILL GRIND");
			
		}
	}
	
	
	
	
}
