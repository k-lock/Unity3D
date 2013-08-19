using UnityEngine;
using System.Collections;

public class MenuScreen : MonoBehaviour
{

	void Start ()
	{
		transform.position = new Vector3 (0, 0, 0);	
	//	Debug.Log (transform.FindChild("Tile_START"));
		//	yield return new WaitForSeconds(2);
		iTween.FadeTo (transform.FindChild("Tile_START").gameObject, iTween.Hash (
			"alpha",0, 
			"easeType", iTween.EaseType.easeInCubic, 
			"loopType", "none", 
			"time", .1f,
			"delay", 0.00001f));
		MoveIn ();
	}
	
	void Update ()
	{
	
	}
	
	void MoveIn()
	{
		iTween.MoveFrom (gameObject, iTween.Hash (
			"x",12, 
			"easeType", iTween.EaseType.easeInCubic, 
			"loopType", "none", 
			"time", .5f,
			"delay", 0.001f));
		

		iTween.FadeTo (transform.FindChild("Tile_START").gameObject, iTween.Hash (
			"alpha",1, 
			"easeType", iTween.EaseType.easeInCubic, 
			"loopType", "none", 
			"time", .5f,
			"delay", .5f));
	}

	public void MoveOut()
	{
		iTween.MoveTo (gameObject, iTween.Hash (
			"x",-20, 
			"easeType", iTween.EaseType.easeInCubic, 
			"loopType", "none", 
			"time", 1f,
			"delay", 0.001f,
			"onComplete", "MenuReady"));
	}
	void MenuReady()
	{
		Application.LoadLevel("kFOV");
	}
}
