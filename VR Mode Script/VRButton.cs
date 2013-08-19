using UnityEngine;
using System.Collections;

public class VRButton : MonoBehaviour {

	public bool events = true;
	private TileBase tile;
	public int id = -1;
	
	void Awake ()
	{
		tile = GetComponent<TileBase> ();
	
		//iTween.MoveTo (gameObject, iTween.Hash ("z", 2f, "time", .5f, "delay", 0f, "easetype", "easeInOutQuart"));
	}

	void OnMouseExit ()
	{
		if (!events)
			return;
	//	SetFrame (1);
	//	iTween.MoveTo (gameObject, iTween.Hash ("z", 2f, "time", .5f, "delay", 0f, "easetype", "easeInOutQuart"));
	}

	void OnMouseOver ()
	{
		if (!events)
			return;
	//	SetFrame (0);
	//	iTween.MoveTo (gameObject, iTween.Hash ("z", -2f, "time", .5f, "delay", 0f, "easetype", "easeInOutQuart"));
	}

	void OnMouseDown ()
	{
		if (!events)
			return;
		else
			events = false;
		
		if (id == 0) {
	//		SetFrame (1);
	//		iTween.MoveTo (gameObject, iTween.Hash ("z", -5f, "time", .5f, "delay", 0f, "easetype", "easeInOutQuart", "onComplete", "_btnSTART"));
			_btnSTART();
		}
		if (id == 1) {
	//		SetFrame (1);
	//		iTween.MoveTo (gameObject, iTween.Hash ("z", 5f, "time", .5f, "delay", 0f, "easetype", "easeInOutQuart", "onComplete", "_btnABOUT"));
		}
		if (id == 3) {
	//		SetFrame (1);
	//		iTween.MoveTo (gameObject, iTween.Hash ("z", 5f, "time", .5f, "delay", 0f, "easetype", "easeInOutQuart", "onComplete", "_btnBACK"));
		}
	}

	void _btnSTART ()
	{
		GameObject.Find("MainMenu").GetComponent<MenuScreen>().MoveOut();
	//	MAINmenu gl = GameObject.Find ("MAINMENU").GetComponent<MAINmenu> ();
	//	gl.START_GAME ();	
	}
	void _btnABOUT ()
	{
	//	MAINmenu gl = GameObject.Find ("MAINMENU").GetComponent<MAINmenu> ();
	//	gl.START_ABOUT ();	
	}
	void _btnBACK ()
	{
	//	MAINmenu gl = GameObject.Find ("MAINMENU").GetComponent<MAINmenu> ();
	//	gl.GO_BACK ();	
	}

	public void SetFrame (int frame)
	{
//		tile._currentFrame = frame;
		tile.MESH_refresh ();
	}
	
}
