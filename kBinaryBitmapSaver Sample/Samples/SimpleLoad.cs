using UnityEngine;
using klock;

public class SimpleLoad : MonoBehaviour 
{
	private Texture2D texture = null;
	
	void Start () 
	{
	    if( texture == null )
  	    {
    			  texture = kBinaryData.Loader( 
				  Application.dataPath +
				  "/TextureBinary/Box.dat" );
  		    }
	}
	
	void OnGUI () 
	{
	    if( texture != null )
	    {
		  GUI.DrawTexture( new Rect( 50, 50, 
						 texture.width, 
						 texture.height ),
						 texture);
	    }	
	}
}