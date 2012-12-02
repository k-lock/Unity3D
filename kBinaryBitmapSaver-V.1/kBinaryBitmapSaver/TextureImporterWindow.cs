/* TextureImporterWindow V.0.1 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.Collections;

public class TextureImporterWindow : EditorWindow
{
	private static TextureImporterWindow window;

	private string[] npot = new string[4]{ "None", "ToLarger", "ToNearest", "ToSmaller"};
	private string[] form = new string[2]{ "ARGB32", "RGB24" };
	private string[] size = new string[8]{ "32", "64", "128", "256", "512", "1024", "2048", "4096" };

	public static void Init ( Rect pos ) {
   
		TextureImporterSetting.show = true;
		
		if(window != null ) {
			
			window.Show();
			return;
			
		}else{
		
			window =( TextureImporterWindow )EditorWindow.GetWindow(typeof( TextureImporterWindow ), false, "TextureImporter Setting");
    		window.position = new Rect(pos.x + 100, pos.y + 100, 210, 200);
			window.Show();
		}		
    }
	
	private void OnGUI()
	{
		GUI.skin.label.fontSize = 9;
		GUI.skin.button.fontSize = 9;
		EditorStyles.popup.fixedHeight= 20;
		EditorStyles.popup.fontSize = 8;
		GUI.skin.textField.alignment = TextAnchor.UpperLeft;	
	
		GUI.Label(new Rect( 5, 20, 150, 20), "Read/Write Enabled" );
		TextureImporterSetting._readable = EditorGUI.Toggle( new Rect( 120, 20, 20, 20), TextureImporterSetting._readable );
		
		GUI.Label(new Rect( 5, 40, 150, 20), "Non Power of 2");
		TextureImporterSetting.npotIndex = EditorGUI.Popup( new Rect(120, 40, 80, 20 ), TextureImporterSetting.npotIndex, npot );
		TextureImporterSetting._npotScale = NpotScaleSetup();
		
		GUI.Label(new Rect( 5, 60, 150, 20), "Generate Mip Maps");
		TextureImporterSetting._mipmapEnabled = EditorGUI.Toggle( new Rect( 120, 60, 20, 20), TextureImporterSetting._mipmapEnabled );
		
		GUI.Label(new Rect( 5, 80, 150, 20), "Texture Format");
		TextureImporterSetting.formIndex = EditorGUI.Popup( new Rect(120, 80, 80, 20 ), TextureImporterSetting.formIndex, form );
		TextureImporterSetting._textureFormat = FormatSetup();
		
		GUI.Label(new Rect( 5, 100, 150, 20), "Max Texture Size");
		TextureImporterSetting.sizeIndex = EditorGUI.Popup( new Rect(120, 100, 80, 20 ), TextureImporterSetting.sizeIndex, size );
		TextureImporterSetting._maxTextureSize = System.Convert.ToInt32( size[TextureImporterSetting.sizeIndex] );

		if( GUI.Button(new Rect( 120, 170, 80, 20 ), "Apply")) Closer();
	
	}
	
	public TextureImporterNPOTScale NpotScaleSetup()
	{
		TextureImporterNPOTScale t = new TextureImporterNPOTScale();	

		if(TextureImporterSetting.npotIndex == 0) t = TextureImporterNPOTScale.None;	
		if(TextureImporterSetting.npotIndex == 1) t = TextureImporterNPOTScale.ToLarger;	
		if(TextureImporterSetting.npotIndex == 2) t = TextureImporterNPOTScale.ToNearest;	
		if(TextureImporterSetting.npotIndex == 3) t = TextureImporterNPOTScale.ToSmaller;	
		
		return t;
	}
	
	public TextureImporterFormat FormatSetup()
	{
		TextureImporterFormat t = new TextureImporterFormat();	

		if(TextureImporterSetting.formIndex == 0) t = TextureImporterFormat.ARGB32;	
		if(TextureImporterSetting.formIndex == 1) t = TextureImporterFormat.RGB24;	

		return t;
	}
	
	private void Closer(){ 
		
		window.Close();
		window = null;
	}
	
	private void OnDestroy()
	{
		TextureImporterSetting.show = false;	
	}
	

}

