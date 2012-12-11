using UnityEngine;
using System.Collections;

public class GridboardGUI : MonoBehaviour {

	MeshBoard MB = null;
	
	public Texture head;
	public Texture butt;
	public Texture btn;
	
	public Font    font;

	void Start () {
	
		if( MB == null ) MB = transform.gameObject.GetComponent<MeshBoard>();
		

	}
	
	void OnGUI () {
	
		Draw_GUI();	
	}
	
		
	private void Draw_GUI()
	{
		GUIStyle style = new GUIStyle();
		style.fontSize = 40;
		style.font = font ;
		style.alignment = TextAnchor.MiddleRight;
		
		GUI.DrawTexture( new Rect((Screen.width*.5f)-508, 0, 1024, 64), head );
	//	GUI.DrawTexture( new Rect((Screen.width*.5f)-508, Screen.height-64, 1024, 64), butt );
		GUI.BeginGroup ( new Rect(10, 0, 750, 40 ) );

			GUI.Label( new Rect( 170, 0, 190, 40 ),""+MB.DOLLARS, style);
			GUI.Label( new Rect( 448, 0,  80, 40 ),""+MB.EnemysComplete, style);

		GUI.EndGroup();	
		
	/*	float sc = (Screen.width*.5f)-120;
		
		GUI.DrawTexture( new Rect(sc + (100*1), Screen.height-55, 95, 50), btn );
		GUI.DrawTexture( new Rect(sc + (100*2), Screen.height-55, 95, 50), btn );
		GUI.DrawTexture( new Rect(sc + (100*3), Screen.height-55, 95, 50), btn );
		GUI.DrawTexture( new Rect(sc + (100*4), Screen.height-55, 95, 50), btn );
		GUI.DrawTexture( new Rect(sc + (100*5), Screen.height-55, 95, 50), btn );
	*/	
	}
}
