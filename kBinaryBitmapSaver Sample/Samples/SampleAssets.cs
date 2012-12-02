using UnityEngine;
using System.Collections;
using klock;
/**
 * Class to hold content in a dictionary for efficient load and unload objects.
 */

public class SampleAssets {
	
	// folder to the binary data files
	public static string globalPath = Application.dataPath + "/TextureBinary/";
	// path to each data file - for easy handling 
	public static string _BOX   = globalPath + "Box.dat";
	public static string _EYE_0 = globalPath + "Eye_empty.dat";
	public static string _EYE_1 = globalPath + "Eye_select.dat";
	// holder object for the textures
	private static IDictionary sTextures = new Hashtable();

	public static Texture2D GetTexture( string name )
    {
        if ( sTextures[name] == null )
        {
			Debug.Log ("Loads texture " + name);
            sTextures[name] = kBinaryData.Loader( name );
        }
        
        return sTextures[name] as Texture2D;
    }	

	public static void ClearList()
	{
		sTextures = new Hashtable();
	}
}