using UnityEngine;
using System;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	private float _introDelay = 5f;
	private float _outerValue = 12f;
	
	void Start () {
	
		transform.position= new Vector3(0,0,0);
		
	//	yield return new WaitForSeconds(2);
		MoveIn();
		
	}
	
	
	void Update () {
	
	}
	
	void MoveIn()
	{
		iTween.MoveFrom (gameObject, iTween.Hash (
			"x",12, 
			"easeType", iTween.EaseType.easeInCubic, 
			"loopType", "none", 
			"time", 1f,
			"delay", 0.001f));
		StartCoroutine(StartMoveOut());
	}
	IEnumerator StartMoveOut()
	{
		yield return new WaitForSeconds(_introDelay);
		MoveOut();
	}
	void MoveOut()
	{
		iTween.MoveTo (gameObject, iTween.Hash (
			"x",-20, 
			"easeType", iTween.EaseType.easeInCubic, 
			"loopType", "none", 
			"time", 1f,
			"delay", 0.001f,
			"onComplete", "MenuScreen"));
	}
	void MenuScreen()
	{
		Application.LoadLevel("MenuScreen");
	}
}
