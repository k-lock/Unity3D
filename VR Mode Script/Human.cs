using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour
{
	public bool		move = true;
	public float 	movementSpeed = 6.0f;
	public Vector3  initPosition = Vector3.zero;
	public Vector3  initScale = new Vector3 (1, 1, 1);
	
	void Awake ()
	{
		transform.localScale = initScale;
		transform.position = initPosition;
		animation.Play ("idle");
	}
	
	void Update ()
	{
		if (!move)
			return;
		/*Vector3 forward = Quaternion.Euler (0, Camera.mainCamera.transform.eulerAngles.y, 0) * Vector3.forward;
		Vector3 sideward = Quaternion.Euler (0, Camera.mainCamera.transform.eulerAngles.y + 90, 0) * Vector3.forward;
		
		Quaternion q = transform.rotation;
		transform.rotation = new Quaternion(q.x,Camera.mainCamera.transform.eulerAngles.y + 90,q.z,q.w);
		*/
		Vector3 newPos = Vector3.zero;
	
		float keyboardX = Input.GetAxis ("Horizontal") * movementSpeed * Time.deltaTime;
		float keyboardY = Input.GetAxis ("Vertical") * movementSpeed * Time.deltaTime;
			
		if (keyboardX != 0 || keyboardY != 0) {
			animation.CrossFade ("walk", .1f, PlayMode.StopAll);
			newPos = rigidbody.position + new Vector3 (keyboardX, -.005f, keyboardY);
			rigidbody.MovePosition (newPos);
			transform.LookAt(newPos);
			
		
			
			StartCoroutine(callIdle ());
			
			
		}		
		Quaternion q = transform.rotation;
			transform.rotation = new Quaternion(0,q.y,0,q.w);
		
	}

	IEnumerator callIdle ()
	{
		yield return new WaitForSeconds(.8f);
			
		animation.CrossFade ("idle", .25f, PlayMode.StopAll);
	}
	
}
