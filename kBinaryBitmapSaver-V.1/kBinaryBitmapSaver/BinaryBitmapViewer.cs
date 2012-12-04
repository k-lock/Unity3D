/* BinaryBitmapViewer V.0.2 - 2011 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.IO;
using klock;

public class BinaryBitmapViewer : EditorWindow
{
	private Rect 		tempRect;
	private string 		fPath 		= Application.dataPath;
	private bool 		show 		= false;
	private Texture2D	fTexture 	= null;
	
	#region EDITOR WINDOW
	
	private static BinaryBitmapViewer 	instance;

    [MenuItem("Window/klock/Binary Bitmap Viewer")]
	public static void ShowWindow()
	{
		if (instance != null)
		{	
			instance = null;	
		}
		
		instance = ( BinaryBitmapViewer )EditorWindow.GetWindow(typeof( BinaryBitmapViewer ), false, "Viewer");
		instance.position = new Rect( 50, 50, 600, 500);
		instance.wantsMouseMove = true;	
		instance.Show();
		
	}
	
	public Rect EditorRect
	{	
		get{ return ((BinaryBitmapViewer)this).position; }
		set{ ((BinaryBitmapViewer)this).position = value; }
	}
	#endregion

	private void Awake()
	{

	}
	
	#region GUI DRAW
	
	private void OnGUI()
	{
		
		DrawFileSelector();
		DrawFileView();
		
		
		ResetStyles();
		Repaint();
	
	}
	
	private void DrawFileSelector()
	{
		Rect   box 		= new Rect(5, 5, EditorRect.width-10, 48 );
		float  boxWidth = box.width - 20;
		string tempPath = fPath;

		DrawRect( new Rect( 0,0, EditorRect.width, 15), new Color(.6f,.6f,.6f));
		
		GUI.skin.label.fontSize = 8;
		GUI.skin.button.fontSize = 8;
		EditorStyles.popup.fontSize = 8;
		GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
		
		GUI.Label(new Rect( 5, 0, 100, 15) , "Binary Bitmap File" );
		GUI.BeginGroup( new Rect(5, 15, box.width, 150));
			
			fPath = EditorGUI.TextField( new Rect( 10, 8, boxWidth-55 , 20 ),  fPath);
            
			if(GUI.Button( new Rect( boxWidth-45, 8, 60, 20 ), new GUIContent("File", "Choose a file to view") )){
				fPath = EditorUtility.OpenFilePanel( "Choose a file to view", fPath, "");
				
				if( File.Exists( fPath )) {
					if( CheckFileType( fPath )){
						fTexture = BinaryData.Loader( fPath );
						show = true;
					}else{
						EditorUtility.DisplayDialog( "The File Format is not correct", "Please choose a different Format\n ( bin, lab, dat )", "Next");
						fPath = tempPath;
						return;
					}
				}
			}
			if( fPath == "" && tempPath != "" ) { fPath = tempPath; } 
		
		GUI.EndGroup();
		
	}

	private void DrawFileView()
	{
		Rect  box = new Rect( 5, 52, EditorRect.width-10, EditorRect.height - 50 );	
	
		DrawRect( new Rect( 0, 50, EditorRect.width, 15), new Color(.6f,.6f,.6f));

		GUI.skin.label.fontSize = 8;
			
		GUI.Label(new Rect( 5, 50, 100, 15) , "Binary Bitmap Viewer" );	
	
		GUI.BeginGroup( box );
		
			GUI.Box( new Rect(  5, 25, box.width-10, box.height - 40), "");
			GUI.Box( new Rect( 10, 30, box.width-20, box.height - 50), "");

			if( show && fTexture != null){
				GUI.DrawTexture( new Rect( 11, 31, box.width-22, box.height - 52), fTexture, ScaleMode.ScaleToFit );
				GUI.Label(new Rect( 120, -2, EditorRect.width, 18) ,"Size [ " + fTexture.width + " X " + fTexture.height + 
			          										 " ] \t Format [ " +  fTexture.format +
			          										 " ] \t AnisoLevel [ " + fTexture.anisoLevel +
			          										 " ] \t FilterMode [ " + fTexture.filterMode +			          										 
			          										 " ] \t WrapMode [ " + fTexture.wrapMode +
			          										 " ] \t Type [ " + fTexture.GetType() +
			          										 " ] \t MipMapBias [ " + fTexture.mipMapBias +
			          										 " ] \t texelSize [ " + fTexture.texelSize +" ]");
			}
		
		GUI.EndGroup();
	}
	#endregion
	#region HELPERS
	
	private bool CheckFileType( string file)
	{
		string _typ = Path.GetExtension( file );

		if( _typ == ".dat" ) return true;
		if( _typ == ".bin" ) return true;
		if( _typ == ".lab" ) return true;
		
		return false;	
	}
	
	private void DrawRect( Rect r, Color c)
	{

		Texture2D tex = EditorGUIUtility.whiteTexture;
		
		GUI.color = c;
		
			EditorGUI.DrawPreviewTexture( r, tex);

		GUI.color = Color.white;

	}
		
	private void ResetStyles()
	{
		GUI.skin.label.fontSize = 10;
		GUI.skin.button.fontSize = 10;
		EditorStyles.popup.fixedHeight= 15;
		EditorStyles.popup.fontSize = 10;
		GUI.skin.textField.alignment = TextAnchor.UpperLeft;	
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		
		EditorStyles.miniButtonLeft.fontSize = 10;
		EditorStyles.miniButtonMid.fontSize = 10;
		EditorStyles.miniButtonRight.fontSize = 10;

	}
	
	#endregion	
}
