/* ExportTypesWindow V.0.2 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.Collections;

public class ExportTypesWindow : EditorWindow
{
	
	private string[] types;
	private bool draw = true;
	private bool namer = false;
	private string _name = "own type";
	private Vector2 scrollPos = Vector2.zero;
	private int fLength = 4;
	
	private int selected = -1;
	private  int tempIndex;
	//private string[] tempTypes;
	
	private static BitmapSaverWindow parent;
	private static ExportTypesWindow window;

	//[MenuItem ("Klock/Tools/Binary Bitmap File Types %7")]
	public static void Init ( BitmapSaverWindow _parent ) 
	{		
		Rect pos 	= _parent.EditorRect;
		parent 		= _parent;
		
		if(window != null ) 
		{			
			window.Show();
			return;
			
		}else{
		
			window =( ExportTypesWindow )EditorWindow.GetWindow(typeof( ExportTypesWindow ), false, "Extension");
    		window.position = new Rect(pos.x + 150, pos.y + 20, 250, 300);
			window.wantsMouseMove = true;	
			window.Show();
		}		
    }
	private void Awake()
	{
		//tempTypes = parent.Ftypes;
		tempIndex = parent.Ftype;
		
		ResetList();

	}
	private void OnGUI()
	{
		GUI.skin.label.fontSize = 9;
		GUI.skin.button.fontSize = 8;
		EditorStyles.miniButtonLeft.fontSize = EditorStyles.miniButtonMid.fontSize =  EditorStyles.miniButtonRight.fontSize = 8;
		EditorStyles.popup.fixedHeight= 20;
		EditorStyles.popup.fontSize = 8;
		GUI.skin.textField.alignment = TextAnchor.UpperLeft;	
	
		GUI.enabled = draw;
		
		if(GUI.Button( new Rect(  5, 5, 80, 15), new GUIContent("Add"   , "Add a new file extension"), EditorStyles.miniButtonLeft)) 	namer = true; 
		if(GUI.Button( new Rect( 85, 5, 80, 15), new GUIContent("Delete", "Remove a file extension" ), EditorStyles.miniButtonMid)) 	RemoveFile(); 
		if(GUI.Button( new Rect(165, 5, 80, 15), new GUIContent("Reset",  "Reset list to default"   ), EditorStyles.miniButtonRight)) 	ResetList(); 
		
		GUI.BeginGroup( new Rect( 5, 30, 240, 230 ));
			GUI.Box( new Rect( 0, 0, 240, 230), "");
			GUI.Box( new Rect( 5, 5, 230, 220), "");
				
			scrollPos = GUI.BeginScrollView( new Rect( 6, 6, 228, 218), scrollPos, new Rect( 0, 0, 213, 20 * types.Length-1));
				Handles.color = new Color( .6f, .6f, .6f, .6f);
				for( int j = 0; j < fLength-1; j++ ) Handles.DrawLine( new Vector3(0 , (20*(j+1)- scrollPos.y), 0) , new Vector3( (fLength > 11 ) ? 212 :230 , (20*(j+1)- scrollPos.y) , 0));
				Handles.color  = Color.white;
				for( int i = 0; i < fLength-1; i++ )
				{
					GUI.color = (selected == i) ? new Color( 1, .5f, 0, .3f) : new Color( 1, 1, 1, .5f);
					
					if(( i != fLength-1 )  && GUI.Button(new Rect(  0, (20*(i))-1, 228, 20), "", ListStyle( i )))
					{
						if(selected == i)
						{ 
							selected = -1; 
						}else if( selected != i ) selected = i;
					}
					GUI.color = Color.white;	
					GUI.Label( new Rect(  50,(20*(i))+2, 150, 20), "." + types[i], GUIStyle.none );

				}
			
			GUI.EndScrollView();
		GUI.EndGroup();

		if( GUI.Button(new Rect( 5, 280, 240, 15 ), "Apply")) Closer();
		
		if(!draw) GUI.enabled = true;
		
		if( namer )
		{
			draw = false;
			
			GUI.Box(   new Rect( 5,  30, 240, 90 ), "");
			GUI.Box(   new Rect( 10, 35, 230, 80 ), "");
			GUI.Label( new Rect( 15, 40, 230, 15 ), "Input new extension :");
			GUI.skin.textField.alignment = TextAnchor.MiddleCenter;
			_name = EditorGUI.TextField( new Rect( 15, 55, 220 , 18 ), _name);
			GUI.skin.textField.alignment = TextAnchor.UpperLeft;	
			
			if(GUI.Button( new Rect(  45, 80, 80, 15), "Apply", EditorStyles.miniButtonLeft))
			{ 
				namer = false; 
				draw = true; 
				AddFile( _name );
			}
			if(GUI.Button( new Rect(  125, 80, 80, 15), "Cancel", EditorStyles.miniButtonRight)) 
			{ 
				namer = false; 
				draw = true;
			}
		}
	}
	
	private void ResetList()
	{
		types = null;
		types = new string[4]{ "bin", "dat", "lab", "create own"};	
		fLength = 4;
		
		Repaint();
	}
	
	private void RemoveFile()
	{
		if( selected == -1) return;
	
		int i = 0;
		string selectedS = types[ selected ];
		string[] temp = types;
		types = null;
		types = new string[temp.Length-1];

		foreach( string s in temp ) if( selectedS != s ) { types[i] = s; i++; }
				
		fLength = types.Length;

		selected = -1;
	}
	
	private void AddFile( string str )
	{

		if( str != "")
		{			
			bool isthere = false;
			foreach( string s in types ) if( s == str ) isthere = true; 
			
			if( isthere ) 
			{				
				EditorUtility.DisplayDialog( "This extension exists in the format list",   "."+str,  "Next");
				return;	

			}else{
	
				string[] temp = types;
		        types = null;
		        types = new string[temp.Length + 1];
				int i=0;
		        for( i=0; i< temp.Length-1; i++ ) types[i] = temp[i];		
		
		        types[i] = str;
				types[i+1] = "create own";
				
		        fLength = types.Length;
				
			}
		}
	}
	
	private void Closer()
	{ 
		window.Close();
		window = null;
	}
	
	private void OnDestroy()
	{
		if( parent.Ftypes != types ) 
		{
			parent.Ftypes = types;
			parent.Ftype = 0;
		}else{
			parent.Ftype = tempIndex;	
		}
	}	
	
	private int OldIndex()
	{
		if( tempIndex > types.Length ) return 0;
		return tempIndex;		
	}
	
	private GUIStyle ListStyle( int selectedIndex )
	{

		GUIStyle _defaultStyle = new GUIStyle();

		_defaultStyle.normal.textColor   = Color.black;
		_defaultStyle.hover.background   = EditorGUIUtility.whiteTexture;
	    _defaultStyle.onHover.background = EditorGUIUtility.whiteTexture;

		_defaultStyle.onHover.textColor  = Color.blue;
	    _defaultStyle.padding.left = _defaultStyle.padding.right = _defaultStyle.padding.top = _defaultStyle.padding.bottom = 4;
	//	_defaultStyle.contentOffset = new Vector2(-10, 0);

		GUIStyle _selectedStyle = new GUIStyle();
		_selectedStyle.normal.background  = EditorGUIUtility.whiteTexture;
		
		return ( selected == selectedIndex ) ? _selectedStyle : _defaultStyle; 
	}
}
