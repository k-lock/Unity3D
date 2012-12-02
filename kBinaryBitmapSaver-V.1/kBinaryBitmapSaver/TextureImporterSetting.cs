/* TextureImporterSetting V.0.1 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;

public class TextureImporterSetting
{
	public static bool 						show 			= false;
	public static bool 						_readable 		= true;
	public static TextureImporterNPOTScale 	_npotScale 		= TextureImporterNPOTScale.None;
	public static bool 						_mipmapEnabled 	= false;
	public static TextureImporterFormat 	_textureFormat 	= TextureImporterFormat.ARGB32;
	public static int 						_maxTextureSize = 4096;
	
	public static int npotIndex = 0;
	public static int formIndex = 0;
	public static int sizeIndex = 7;

}

    