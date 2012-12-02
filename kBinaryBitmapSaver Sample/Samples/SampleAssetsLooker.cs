using UnityEngine;
using klock;

public class SampleAssetsLooker : MonoBehaviour 
{
	
	private bool showIt = false;
	
	
	/**
		// Load a binary byte array in a texture2D use the kBinaryData.cs
		
			kBinaryData.Loader( Application.dataPath + "/TextureBinary/DIALOG_play.dat" )
		
		// using the SampleAssets.cs to hold more content - the texture where packed in a dictionary, so you now that the file loads only one time
		// and you use static path strings - no need to typ in every path 
			
			SampleAssets.GetTexture( SampleAssets._Box );
	*/
	
	void Start () 
	{	
		
	}

	void OnGUI () 
	{
	
		GUI.DrawTexture( new Rect( 50, 50, 125, 125 ),  SampleAssets.GetTexture( SampleAssets._BOX ));
		
		if( GUI.Button( new Rect( 350, 50, 125, 125 ),"", GUIStyle.none ))
		{
			showIt = !showIt;
		}
		GUI.DrawTexture( new Rect( 350, 50, 125, 125 ), SampleAssets.GetTexture(( showIt ) ? SampleAssets._EYE_1 : SampleAssets._EYE_0 ));
		
	}
	

}