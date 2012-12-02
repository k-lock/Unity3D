/* kBinaryBitmapExtension V.0.2 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.Collections;

using klock;

public class kBinaryBitmapExtension : EditorWindow
{
	
	private string[]    _types;
	private bool        _draw       = true;
	private bool        _namer      = false;
	private string      _name       = "own type";
	private Vector2     _scrollPos  = Vector2.zero;
	private int         _fLength    = 4;
	private int         _selected   = -1;
	private int         _tempIndex;

	private static kBinaryBitmapSaver parent;
	private static kBinaryBitmapExtension window;

    #region EDITOR WINDOW
    /**
     * Editor Window creation method
     * @param kBinaryBitmapSaver - The parent window class
     */
	public static void Init ( kBinaryBitmapSaver _parent ) 
	{		
		Rect pos 	= _parent.EditorRect;
		parent 		= _parent;
		
		if(window != null ) 
		{			
			window.Show();
			return;
			
		}else{
		
			window =( kBinaryBitmapExtension )EditorWindow.GetWindow(typeof( kBinaryBitmapExtension ), false, "Manage Export Extensions");
    		window.position = new Rect(pos.x + 150, pos.y + 20, 250, 300);
			window.wantsMouseMove = true;	
			window.Show();
		}		
    }

    /**
     * Editor Window Closing 
     */
    private void Closer()
    {
        window.Close();
        window = null;
    }
    #endregion
    #region UNITY
    private void Awake()
	{
		_types = parent.Ftypes;
		_tempIndex = parent.Ftype;
		_fLength = _types.Length;		 
	}

	private void OnGUI()
	{
		GUI.skin.label.fontSize = 9;
		GUI.skin.button.fontSize = 8;
		EditorStyles.miniButtonLeft.fontSize = EditorStyles.miniButtonMid.fontSize =  EditorStyles.miniButtonRight.fontSize = 8;
		EditorStyles.popup.fixedHeight= 20;
		EditorStyles.popup.fontSize = 8;
		GUI.skin.textField.alignment = TextAnchor.UpperLeft;	
	
		GUI.enabled = _draw;
		
		if(GUI.Button( new Rect(  5, 5, 80, 15), new GUIContent("Add"   , "Add a new file extension"), EditorStyles.miniButtonLeft)) 	_namer = true; 
		if(GUI.Button( new Rect( 85, 5, 80, 15), new GUIContent("Delete", "Remove a file extension" ), EditorStyles.miniButtonMid)) 	RemoveFile(); 
		if(GUI.Button( new Rect(165, 5, 80, 15), new GUIContent("Reset",  "Reset list to default"   ), EditorStyles.miniButtonRight)) 	ResetList(); 
		
		GUI.BeginGroup( new Rect( 5, 30, 240, 230 ));
			GUI.Box( new Rect( 0, 0, 240, 230), "");
			GUI.Box( new Rect( 5, 5, 230, 220), "");
				
			_scrollPos = GUI.BeginScrollView( new Rect( 6, 6, 228, 218), _scrollPos, new Rect( 0, 0, 213, 20 * _types.Length-1));
				Handles.color = new Color( .6f, .6f, .6f, .6f);
				    for( int j = 0; j < _fLength-1; j++ ) Handles.DrawLine( new Vector3(0 , (20*(j+1)- _scrollPos.y), 0) , new Vector3( (_fLength > 11 ) ? 212 :230 , (20*(j+1)- _scrollPos.y) , 0));
				Handles.color  = Color.white;
				for( int i = 0; i < _fLength-1; i++ )
				{
					GUI.color = (_selected == i) ? new Color( .8f, .8f, 0.8f, .8f) : new Color( 1, 1, 1, .5f);
					
					if(( i != _fLength-1 )  && GUI.Button(new Rect(  0, (20*(i))-1, 228, 20), "", ListStyle( i )))
					{
						if(_selected == i)
						{ 
							_selected = -1; 
						}else if( _selected != i ) _selected = i;
					}
					GUI.color = Color.white;	
					GUI.Label( new Rect(  50,(20*(i))+2, 150, 20), "." + _types[i], GUIStyle.none );

				}
			
			GUI.EndScrollView();
		GUI.EndGroup();

		if( GUI.Button(new Rect( 5, 280, 240, 15 ), "Ready")) Closer();
		if(!_draw) GUI.enabled = true;
		if( _namer )
		{
			_draw = false;
			
			GUI.Box(   new Rect( 5,  30, 240, 90 ), "");
			GUI.Box(   new Rect( 10, 35, 230, 80 ), "");
			GUI.Label( new Rect( 15, 40, 230, 15 ), "Input new extension :");
			GUI.skin.textField.alignment = TextAnchor.MiddleCenter;
			_name = EditorGUI.TextField( new Rect( 15, 55, 220 , 18 ), _name);
			GUI.skin.textField.alignment = TextAnchor.UpperLeft;	
			
			if(GUI.Button( new Rect(  45, 80, 80, 15), "Apply", EditorStyles.miniButtonLeft))
			{ 
				_namer = false; 
				_draw = true; 
				AddFile( _name );
			}
			if(GUI.Button( new Rect(  125, 80, 80, 15), "Cancel", EditorStyles.miniButtonRight)) 
			{ 
				_namer = false; 
				_draw = true;
			}
		}
		Repaint ();
	}

    private void OnDestroy()
    {
        if (parent.Ftypes != _types)
        {
            parent.Ftypes = _types;
            parent.Ftype = 0;
        }else{
            parent.Ftype = _tempIndex;
        }
        _types = null;
        parent = null;
        window = null;
    }
    #endregion
    #region MENU COMMANDS
    /**
    * Menu command - Add a file extension to the list.
    * @param string str New extension name.
    */
    private void AddFile( string str )
    {
        if ( str != "" )
        {
            bool isthere = false;
            foreach (string s in _types) if ( s == str ) isthere = true;

            if ( isthere )
            {
                EditorUtility.DisplayDialog("This extension exists in the format list", "." + str, "Next");
                return;

            }else{

                string[] temp = _types;
                _types = null;
                _types = new string[temp.Length + 1];
                int i = 0;
                for (i = 0; i < temp.Length - 1; i++) _types[i] = temp[i];

                _types[i] = str;
                _types[i + 1] = "create own";

                _fLength = _types.Length;
                parent.Ftypes = _types;
            }
        }
    }

    /**
    * Menu command - Remove selected index from extension list
    * if( _selected == -1) return;
    */
    private void RemoveFile()
    {
        if (_selected == -1) return;

        int i = 0;
        string selectedS = _types[_selected];
        string[] temp = _types;
        _types = null;
        _types = new string[temp.Length - 1];

        foreach (string s in temp) if (selectedS != s) { _types[i] = s; i++; }

        _fLength = _types.Length;

        _selected = -1;
    }

    /**
     * Menu command - Clear complete extension list and set to default value.
     */
    private void ResetList()
	{
		_types = null;
		_types = new string[4]{ "bin", "dat", "lab", "create own"};	
		_fLength = 4;
		
		Repaint();
	}

    #endregion
    #region HELPER

    /** Check if the index of points to an old value */
    private int OldIndex()
	{
		if( _tempIndex > _types.Length ) return 0;
		return _tempIndex;		
	}

    /** Setup a GUIStyle for the list elements */
	private GUIStyle ListStyle( int selectedIndex )
	{

		GUIStyle _defaultStyle = new GUIStyle();

		_defaultStyle.normal.textColor   = Color.black;
		_defaultStyle.hover.background   = EditorGUIUtility.whiteTexture;
	    _defaultStyle.onHover.background = EditorGUIUtility.whiteTexture;

		_defaultStyle.onHover.textColor  = Color.blue;
	    _defaultStyle.padding.left = _defaultStyle.padding.right = _defaultStyle.padding.top = _defaultStyle.padding.bottom = 4;

		GUIStyle _selectedStyle = new GUIStyle();
		_selectedStyle.normal.background  = EditorGUIUtility.whiteTexture;
		
		return ( _selected == selectedIndex ) ? _selectedStyle : _defaultStyle;
    }
    #endregion 
}
