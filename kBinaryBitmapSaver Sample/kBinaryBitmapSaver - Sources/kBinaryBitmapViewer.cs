/* kBinaryBitmapViewer V.0.1 - 2011 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.IO;

using klock;

public class kBinaryBitmapViewer : EditorWindow
{
    /**
     * Draws a editor window, in which you can select and preview a binary bitmap data file. */

	private string 		_fPath 		= Application.dataPath;
	private bool 		_show 		= false;
	private Texture2D	_fTexture 	= null;
    private Rect        _bitmapRect;
    
    private static kBinaryBitmapViewer instance;

    #region EDITOR WINDOW

    [MenuItem ("klock/kBinaryBitmapSaver/Viewer")]
	public static void ShowWindow()
	{
		if (instance != null) instance = null;
	
		instance = ( kBinaryBitmapViewer )EditorWindow.GetWindow(typeof( kBinaryBitmapViewer ), false, "Binary Viewer");
		instance.position = new Rect( 50, 50, 600, 500);
		instance.wantsMouseMove = true;	
		instance.Show();
		
	}
    /**
     * Get or Set the EditorWindow.position rectangle.
     * @returns Rect Returns the editor window.position
     * Set - Adjust the editor window.position
     * @param Rect rect
     * @returns void*/
    public Rect EditorRect
    {	
        get{ return ((kBinaryBitmapViewer)this).position; }
        set{ ((kBinaryBitmapViewer)this).position = value; }
    }

    #endregion
    #region GUI DRAW
	
    private void OnGUI()
    {	
        DrawFileSelector();
        DrawFileView();
	
        ResetStyles();
        Repaint();
    }
	/**
     * Draw File Selection section */
    private void DrawFileSelector()
    {
        Rect   box 		= new Rect(5, 5, EditorRect.width-10, 48 );
        float  boxWidth = box.width - 20;
        string tempPath = _fPath;

        DrawRect( new Rect( 0,0, EditorRect.width, 15), new Color(.6f,.6f,.6f));
		
        GUI.skin.label.fontSize = 8;
        GUI.skin.button.fontSize = 8;
        EditorStyles.popup.fontSize = 8;
        GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
		
        GUI.Label(new Rect( 5, 0, 100, 15) , "Binary Bitmap File" );
        GUI.BeginGroup( new Rect(5, 15, box.width, 150));
			
            _fPath = EditorGUI.TextField( new Rect( 10, 8, boxWidth-55 , 20 ),  _fPath);
            
            if(GUI.Button( new Rect( boxWidth-45, 8, 60, 20 ), new GUIContent("File", "Choose a file to view") )){
                _fPath = EditorUtility.OpenFilePanel( "Choose a file to view", _fPath, "");
				
                if( File.Exists( _fPath )) {
                    if( CheckFileType( _fPath )){
                        _fTexture = kBinaryData.Loader( _fPath );
                        _show = true;
                    }else{
                        EditorUtility.DisplayDialog( "The File Format is not correct", "Please choose a different Format\n ( bin, lab, dat )", "Next");
                        _fPath = tempPath;
                        return;
                    }
                }
            }
            if( _fPath == "" && tempPath != "" ) { _fPath = tempPath; } 
		
        GUI.EndGroup();
		
    }
    /**
     * Draw Viewer section */
    private void DrawFileView()
    {
        Rect  box = new Rect( 5, 52, EditorRect.width-10, EditorRect.height - 50 );	
	
        DrawRect( new Rect( 0, 50, EditorRect.width, 15), new Color(.6f,.6f,.6f));

        GUI.skin.label.fontSize = 8;
			
        GUI.Label(new Rect( 5, 50, 100, 15) , "Binary Bitmap Viewer" );	
	
        GUI.BeginGroup( box );
		
            GUI.Box( new Rect(  5, 25, box.width-10, box.height - 40), "");
            GUI.Box( new Rect( 10, 30, box.width-20, box.height - 50), "");

            if( _show && _fTexture != null){
                GUI.DrawTexture( new Rect( 11, 31, box.width-22, box.height - 52), _fTexture, ScaleMode.ScaleToFit );
                GUI.Label(new Rect( 120, -2, EditorRect.width, 18) ,"Size [ " + _fTexture.width + " X " + _fTexture.height + 
                                                             " ] \t Format [ " +  _fTexture.format +
                                                             " ] \t AnisoLevel [ " + _fTexture.anisoLevel +
                                                             " ] \t FilterMode [ " + _fTexture.filterMode +			          										 
                                                             " ] \t WrapMode [ " + _fTexture.wrapMode +
                                                             " ] \t Type [ " + _fTexture.GetType() +
                                                             " ] \t MipMapBias [ " + _fTexture.mipMapBias +
                                                             " ] \t texelSize [ " + _fTexture.texelSize +" ]");
            }
		
        GUI.EndGroup();
    }
    #endregion
    #region HELPERS

    /**
     * Compares file string with existing type.  
     * @param string file The Extension to check.
     * @return Returns false if file string is not in the list.*/
    private bool CheckFileType( string file )
	{
		string _typ = Path.GetExtension( file );

		if( _typ == ".dat" ) return true;
		if( _typ == ".bin" ) return true;
		if( _typ == ".lab" ) return true;
		
		return false;
	}
    /** 
     * Draws a simple filled rectangle, with given size and position. 
     * @param Rect  r - Unity.Rect Holds the position and size data.
     * @param Color c - Unity.Color The Color to fill the rectangle. */
    private void DrawRect( Rect r, Color c)
	{
		Texture2D tex = EditorGUIUtility.whiteTexture;
		
		GUI.color = c;
			EditorGUI.DrawPreviewTexture( r, tex);
		GUI.color = Color.white;

	}
	/** Reset Gui Style of changed elements. */
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
		
		//EditorStyles.boldLabel.contentOffset = new Vector2( 0, 0);
		
	}
	
	#endregion	
}
