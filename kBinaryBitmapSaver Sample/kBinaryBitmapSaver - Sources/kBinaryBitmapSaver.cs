/* kBinaryBitmapSaver V.0.2.1 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.IO;

using klock;
using klock.GuiDraw;

public class kBinaryBitmapSaver : EditorWindow
{

	private string[]	_fTypes 	= new string[4]{ "bin", "dat", "lab", "create own"};
	private int			_fType 	    = 1;
	private int			_fTemp 	    = 1;
	private string[]	_fList;
	private int			_fLength    = 0;
	private string 		_tempDir;
    private Rect        _tempRect;
	private bool 		_saveSource = true;
	private bool 		_lineDrag   = false;
	private bool 		_draw 	    = true;
	private Vector2 	_scrollPos  = Vector2.zero;
	private float 		_sideWidth;
	private int 		_selectedIndex = -1;

    public  static string fPath = Application.dataPath + "/TextureBinary/";
    private static kBinaryBitmapSaver instance;

	#region EDITOR WINDOW

    [MenuItem("klock/kBinaryBitmapSaver/Converter")]
	public static void ShowWindow()
	{
		if (instance != null)
		{	
			instance.position = new Rect( 50, 50, 600, 500);
			//instance.ShowUtility();
			instance.Show();
			return;
		}
		
		instance = ( kBinaryBitmapSaver )EditorWindow.GetWindow(typeof( kBinaryBitmapSaver ), false, "Binary Export");
		instance.position = new Rect( 50, 50, 600, 500);
		instance.wantsMouseMove = true;	
		instance.Show();
		
	}
	public Rect EditorRect
	{	
		get{ return ((kBinaryBitmapSaver)this).position; }
		set{ ((kBinaryBitmapSaver)this).position = value; }
	}
	private void AutoEditorResizer()
	{
		if( _tempRect.width != EditorRect.width &&  !_lineDrag)
		{
			
			_tempRect = EditorRect;
			_sideWidth = (EditorRect.width-30)/2;
			
		}
	}

	#endregion

	private void Awake()
	{
		_tempRect = EditorRect;
		CheckDirectory();
	}
	
	#region GUI DRAW
	
	private void OnGUI()
	{
		_draw = (TextureImporterSetting.show) ? false: true;
		_draw = (Application.isPlaying) ? false: true;
		
		if( _fType == _fTypes.Length-1 ) 
		{ 
			_draw = false; 
			kBinaryBitmapExtension.Init( ((kBinaryBitmapSaver)this) ); 
		}
		Handles.DrawLine( new Vector3( 0,0,0), new Vector3(10,0,0));
        Handles.BeginGUI();
		
            AutoEditorResizer();
		    GUI.enabled = _draw ;	
				
		    DrawDragLine( new Rect( 15, 98, EditorRect.width-30, EditorRect.height - 190));
		    DrawDirectory();
		    DrawFileList();
		    DrawExporter();

		    if( TextureImporterSetting.show ) kBinaryBitmapImporter.Init( EditorRect );
		
		    Repaint();
		    ResetStyles();
		
        Handles.EndGUI();
	
	}
	
	private void DrawDragLine( Rect pos )
	{
		Event e = Event.current;	
		int   w = 10;
		Rect  d = new Rect( _sideWidth, pos.y, w, pos.height+25 );

		Gui.DrawRect( d, new Color( 0,0,0,0) );

		if( d.Contains( e.mousePosition ))
		{
			
			EditorGUIUtility.AddCursorRect (d, MouseCursor.ResizeHorizontal );        

			_lineDrag = (  e.type == EventType.mouseDrag  ) ? true : false;

		}
		if(_lineDrag && e.type == EventType.MouseUp || e.mousePosition.x<50 || e.mousePosition.x >EditorRect.width-50){ 
			_lineDrag = false;
		}
		if( _lineDrag ) 
		{
			_sideWidth = e.mousePosition.x - w/2;
			Repaint();
		}
	}
	
	private void DrawDirectory()
	{
		Rect   box 		= new Rect(5, 5, EditorRect.width-10, 48 );
		float  boxWidth = box.width - 20;
		string tempPath = fPath;

		DrawRect( new Rect( 0,0, EditorRect.width, 15), new Color(.6f,.6f,.6f));
		
		GUI.skin.label.fontSize = 8;
		GUI.skin.button.fontSize = 8;
		EditorStyles.popup.fontSize = 8;
		GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
		
		GUI.Label(new Rect( 5, 0, 70, 15) , "Target Folder" );
		GUI.BeginGroup( new Rect(5, 15, box.width, 150));
			
			//fPath = 
			EditorGUI.TextField( new Rect( 10, 8, boxWidth-55 , 20 ),  fPath);
            if (!fPath.EndsWith("/")) fPath += "/";
			if(GUI.Button( new Rect( boxWidth-45, 8, 60, 20 ), new GUIContent("Directory", "Choose or create a folder to copy files from user environment in the Unity project") )){
				fPath = EditorUtility.OpenFolderPanel( "Target Unity Folder", fPath, "");
				AssetDatabase.Refresh();
			}
			if( fPath == "" && tempPath != "" ) { fPath = tempPath; }
		
		GUI.EndGroup();
		
	}
	
	private void DrawFileList()
	{
		Rect  box = new Rect( 5, 52, EditorRect.width-10, EditorRect.height - 105 );	
	
		DrawRect( new Rect( 0, 50, EditorRect.width, 15), new Color(.6f,.6f,.6f));
			
		GUI.skin.label.fontSize = 8;
		EditorStyles.popup.fixedHeight= 20;
		EditorStyles.miniButtonLeft.fontSize = 8;
		EditorStyles.miniButtonMid.fontSize = 8;
		EditorStyles.miniButtonRight.fontSize = 8;
		
		GUI.Label(new Rect( 5, 50, 100, 15) , "Convert File List" );	
		GUI.Label(new Rect( 460, 70, 70, 25) , "Export\nExtension" );
		GUI.BeginGroup( box );
			
			GUI.Box( new Rect(  5, 45, box.width-10, box.height - 50) , "");
			GUI.Box( new Rect( 10, 60, box.width-20, box.height - 74) , "");

			GUI.skin.label.fontSize = 9;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect(15, 40, _sideWidth-15, 25 ), "Resource");
			GUI.Label(new Rect(_sideWidth+15, 40, box.width-20-_sideWidth, 25 ), "Convert To");
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.skin.label.fontSize = 8;
			
			GUI.skin.button.fontSize = 7;
				if(GUI.Button( new Rect(  5, 20, 80, 20 ), new GUIContent( "TextureImporter\nSettings","General settings for the unity TextureImporter"))) TextImportSetup();
			GUI.skin.button.fontSize = 8;

			if(GUI.Button( new Rect( 100, 20, 80, 20), new GUIContent("Add" , "Add a file to the converter list"), EditorStyles.miniButtonLeft)) AddFile(); 			
			if( _draw )GUI.enabled = ( _selectedIndex != -1 ) ? true : false;
				if(GUI.Button( new Rect( 180, 20, 80, 20),  new GUIContent("Delete", "Remove selected file from the converter list"),  EditorStyles.miniButtonMid))  DeleteFile(); 
			if( _draw )GUI.enabled = ( _fLength > 0 ) ? true : false;
				if(GUI.Button( new Rect( 260, 20, 80, 20), new GUIContent( "Clear List", "Clear the complete converter list"), EditorStyles.miniButtonRight))	DeleteAll(); 	
			if( _draw )GUI.enabled = true;
		
		_fTemp = _fType;
		_fType = EditorGUI.Popup( new Rect( 500, 20, 60, 25 ), _fType, _fTypes);
		
		GUI.Button( new Rect(box.width - 65, 20, 60, 25), new GUIContent("" , "Select a Extension for the export files"), GUIStyle.none); 
		GUI.EndGroup();

		if( _fLength > 0 ){

			_scrollPos = GUI.BeginScrollView( new Rect( 16, 113, box.width-22, box.height - 76), _scrollPos, new Rect( 0, 0, box.width-42 , (22 * _fLength)));

				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
	
				Handles.color = new Color( .6f, .6f, .6f, .6f);
						for( int j = 0; j < _fLength; j++  ) Handles.DrawLine( new Vector3(0 , Mathf.RoundToInt(-2+j*22)+22, 0) , new Vector3( box.width , Mathf.RoundToInt(-2+j*22)+22 , 0));
				Handles.color  = Color.white;
			
				for( int i = 0; i < _fLength; i++ )
				{
					if( _fList[i] != null ){
						GUI.color = (_selectedIndex == i) ? new Color( .8f, .8f, 0.8f, .8f) : new Color( 1, 1, 1, .5f);
						
						if( GUI.Button(new Rect( -10 , -2+i*22, box.width-5, 21), "", ListStyle( i ))){;
							if(_selectedIndex == i){ 
								_selectedIndex = -1; 
							}else if(_selectedIndex != i) _selectedIndex = i;
						}
						GUI.color = Color.white;
					
						GUI.Label( new Rect( 10 , 2 +i*22, _sideWidth-22, 20), _fList[i], GUIStyle.none );
						GUI.Label( new Rect( _sideWidth, 2+i*22, EditorRect.width-_sideWidth+10, 20),  Path.GetFileNameWithoutExtension( _fList[i] ) + "."+ _fTypes[_fType], GUIStyle.none);
					}
				}
        	GUI.EndScrollView();
		}
		
		Gui.DrawLine(new Vector2( _sideWidth+box.x, 45+box.y ), new Vector2( _sideWidth+box.x, box.height+box.y-15 ), Color.grey);

	}

	private void DrawExporter()
	{	
		Rect  box = new Rect(0, EditorRect.height - 50, EditorRect.width, 50);
		
		GUI.BeginGroup( box );
		
			DrawRect( new Rect( 0, 0, EditorRect.width, 15), new Color(.6f,.6f,.6f));
			
			GUI.skin.button.fontSize = 8;
			GUI.skin.label.fontSize = 8;

			GUI.Label(new Rect(   5,  0, 100, 15), "Export Files : " + _fLength );
			GUI.Label(new Rect( box.width/2 + 180, 20, 70, 15), "Delete Resource" );
			_saveSource = GUI.Toggle( new Rect(box.width/2 + 250, 20, 80, 20 ), _saveSource, new GUIContent("", "Remove the copied asset resource data"));
		 
			if( _draw )GUI.enabled = ( _fLength > 0 ) ? true : false;
				if( GUI.Button( new Rect( box.width/2 - 75, 20, 150, 20 ), new GUIContent( "Start Batch Export", "Press to begin the convert process")) )  SaveAssets();
			if( _draw )GUI.enabled = true;

		GUI.EndGroup();

	}
	
	private GUIStyle ListStyle( int selected )
	{

		GUIStyle _defaultStyle = new GUIStyle();

		_defaultStyle.normal.textColor   = Color.black;
		_defaultStyle.hover.background   = EditorGUIUtility.whiteTexture;
	    _defaultStyle.onHover.background = EditorGUIUtility.whiteTexture;

		_defaultStyle.onHover.textColor  = Color.blue;
	    _defaultStyle.padding.left = _defaultStyle.padding.right = _defaultStyle.padding.top = _defaultStyle.padding.bottom = 4;
		_defaultStyle.contentOffset = new Vector2(20, 0);

		GUIStyle _selectedStyle = new GUIStyle();
		_selectedStyle.normal.background  = EditorGUIUtility.whiteTexture;
		
		return ( selected == _selectedIndex ) ? _selectedStyle : _defaultStyle; 
	}
	
	#endregion
	#region FILE MANAGER

	private void AddFile()
	{
		CheckDirectory();
   								
    	string file = EditorUtility.OpenFilePanel("Select Bitmap",( _tempDir != "" ) ? _tempDir : "", "Image Files(*.BMP;*.GIF;*.JPG;*.PNG;*.PSD;*.TGA;*.TIF;*.IFF;*.PICT;)|*.BMP;*.GIF;*.JPG;*.PNG;*.PSD*.TGA;*.TIF;*.IFF;*.PICT;*|All files (*.*)|*.*");
        if( file  == "" )return;
        if( !CheckFileType( file ) )  
		{ 
			EditorUtility.DisplayDialog( "The File Format is not correct", 
		                            	 "Please choose a Bitmap Format\n ( bmp, gif, jpg, png, psd, tga, tiff, iff, pict )",
		                          	 	 "Next");
			return;
		}
        if( CheckFileTemp ( file ))
		{ 	
			EditorUtility.DisplayDialog( "A file with the same name exists in the resource temp folder", 
		                            	 fPath,
		                            	 "Next");
			return; 
		}
		

		if( !CheckFileAsset( CutFile(file) )){
			
			EditorUtility.DisplayProgressBar("Add File", Path.GetFileName(file), 40);
			
			string cpyPath = fPath + Path.GetFileName(file);
			System.IO.File.Copy( file, cpyPath, true );

			file = cpyPath;
		
			AssetDatabase.Refresh();

        }
		
		EditorUtility.DisplayProgressBar("Add File", Path.GetFileName(file), 60);
		
		Texture2D texture = Resources.LoadAssetAtPath( CutFile(file), typeof(Texture2D) ) as Texture2D;
        TextureImporterSetup( texture );
        AssetDatabase.Refresh();	
	
		EditorUtility.DisplayProgressBar("Add File", Path.GetFileName(file), 100);
		
        if( _fLength == 0) {
            _fList = new string[1]{ CutFile(file)};
            _fLength = 1;
            _tempDir = file;
        }else{
            string[] temp = _fList;
            _fList = null;
            _fList = new string[temp.Length + 1];
		
            for( int i=0; i< temp.Length; i++ ) _fList[i] = temp[i];		

            _fList[_fList.Length-1] = CutFile(file);
            _tempDir = file;
            _fLength = _fList.Length;
        }
		EditorUtility.ClearProgressBar();
	}	
	private void CheckDirectory()
	{
		if( !Directory.Exists( fPath )) Directory.CreateDirectory(fPath);AssetDatabase.Refresh();
	}
	private bool CheckFileType( string file)
	{
		string _typ = Path.GetExtension( file );
		
		//BMP; GIF; PSD; PNG; JPG; TGA; TIFF; IFF; PICT;
	
		if( _typ == ".bmp" ||  _typ == ".BMP" ) return true;
		if( _typ == ".gif" ||  _typ == ".GIF" ) return true;
		if( _typ == ".psd" ||  _typ == ".PSD" ) return true;
		if( _typ == ".png" ||  _typ == ".PNG" ) return true;
		if( _typ == ".jpg" ||  _typ == ".JPG" ) return true;
		
		if( _typ == ".tga" ||  _typ == ".TGA" ) return true;	
		if( _typ == ".tif" ||  _typ == ".TIF" ) return true;
		if( _typ == ".iff" ||  _typ == ".IFF" ) return true;
		if( _typ == ".pict"||  _typ == ".PICT") return true;
		
		return false;
		
	}
	private bool CheckFileAsset( string file )
	{
		Texture2D _texture = Resources.LoadAssetAtPath( file, typeof(Texture2D) ) as Texture2D;
		string 	  _path = AssetDatabase.GetAssetPath( _texture );
		
		if(_path != "" ) return true;
		
		
		
		return false;
	}	
	
	private bool CheckFileTemp( string file )
	{
		
		bool there = false;

		if( _fLength > 0 ) {
			foreach( string s in _fList){
				if( Path.GetFileName( s ) == Path.GetFileName( file ) ) there = true;
			}	
		}
	
		if( there && File.Exists( fPath + Path.GetFileName(file))) return true;

		return false;
	}
	
	private string CutFile( string file )
	{
		string s;
		
		if(file.Contains( Application.dataPath ) ){
		
			string appPath = Application.dataPath.Substring( 0, Application.dataPath.Length-6 );
			s = file.Substring( appPath.Length, file.Length - appPath.Length );
			
		}else{
			
			s = file;
			
		}
		
		return s;
	}	

	private void DeleteFile()
	{
		if(File.Exists( fPath + Path.GetFileName( _fList[ _selectedIndex ] ) )){
			
			switch ( EditorUtility.DisplayDialogComplex("What do you want to delete?",
			                                            "Please choose one of the following options.",
			                                            "Item and source",
			                                            "Item",
			                                            "Cancel")){
				case 0:
				
					System.IO.File.Delete( fPath + Path.GetFileName( _fList[ _selectedIndex ] ) );
					AssetDatabase.Refresh();
					
					DeleteSelectedListItem();
				
				break;
				case 1:
				
					DeleteSelectedListItem();
				
				break;
				case 2:
				
					//return;
				
				break;
			}
		}
	}
	private void DeleteSelectedListItem()
	{
		if( _fLength > 1 ){
				
			int i = 0;
			string selected = _fList[ _selectedIndex ];
			string[] temp = _fList;
			_fList = null;
			_fList = new string[temp.Length-1];
	
			foreach( string s in temp ){ 
				if( selected != s ) {
					_fList[i] = s;
					i++;
				}
			}
			
			_fLength = _fList.Length;

		}else{
			
			_fList = null;
			_fLength = 0;
		}
		
		_selectedIndex = -1;
		Repaint();
	}
	
	private void DeleteAll()
	{
		
		switch ( EditorUtility.DisplayDialogComplex("What do you want to delete?",
			                                        "Please choose one of the following options.",
			                                        "List and sources",
			                                        "List",
			                                        "Cancel"))
		{
			case 0:
			
				foreach( string s in _fList ) File.Delete( s );
				/*foreach( string s in _fList ) {
					FileInfo fi = new FileInfo( s );
					fi.Delete();
					fi = null;
				}*/
				AssetDatabase.Refresh();
				
				_fList = null;
				_fLength = 0;
			
			break;
			case 1:
			
				_fList = null;
				_fLength = 0;
			
			break;
			case 2:
			
				//return;
			
			break;
		}
		_selectedIndex = -1;
		Repaint();
	}	
	
	#endregion
	#region TEXTURE IMPORTER SETTINGS
	
	private void TextImportSetup()
	{
		TextureImporterSetting.show = true;
	}
	private void TextureImporterSetup( Texture2D _texture )
	{

		string 			       path = AssetDatabase.GetAssetPath( _texture );
		TextureImporter texImporter = AssetImporter.GetAtPath( path ) as TextureImporter;
		TextureImporterSettings tis = new TextureImporterSettings();
		
		texImporter.ReadTextureSettings(tis);
		
		tis.readable = 		 TextureImporterSetting._readable;			// true;
		tis.npotScale = 	 TextureImporterSetting._npotScale;			// TextureImporterNPOTScale.None;
		tis.mipmapEnabled =  TextureImporterSetting._mipmapEnabled; 	// false;
		tis.textureFormat =  TextureImporterSetting._textureFormat;	// TextureImporterFormat.ARGB32;
		tis.maxTextureSize = TextureImporterSetting._maxTextureSize;// 4096;
		
		texImporter.SetTextureSettings( tis );
		
		AssetDatabase.ImportAsset( path );

	}
	
	#endregion
	#region BINARY METHODS
	
	public void SaveAssets()
	{
		if(!Directory.Exists( fPath )){
	
			if ( EditorUtility.DisplayDialog("The Directory doesnt exist", "Create a new Directory in \n" + fPath, "Choose", "Cancel") )
				Directory.CreateDirectory( fPath );
			else
				return;
		}
		
		try
		{
		
			float i = 1;
			float f = _fLength;
			foreach( string s in _fList ){
				
				EditorUtility.DisplayProgressBar("Convert Data " + i + " / " + f, s, i/f);
				
				Texture2D texture = Resources.LoadAssetAtPath( s, typeof(Texture2D) ) as Texture2D;
				string 	  nFile   = fPath + Path.GetFileNameWithoutExtension( s ) + "."+ _fTypes[_fType];
		
				TextureImporterSetup( texture );
			
				if( AssetDatabase.GetAssetPath( texture ) != null ) {
					kBinaryData.Save( texture, nFile );
					if( _saveSource && IsTargetFolder( s )) File.Delete( s );
		
				}
	
				i++;
			}
		}
		finally
		{
			AssetDatabase.Refresh();
			EditorUtility.ClearProgressBar();
			
			_selectedIndex = -1;
			_fList = null;
	        _fLength = 0;
		}
	}

	#endregion	
	#region HELPERS
	
	private bool IsTargetFolder( string file )
	{
		string n = Path.GetFileName( file );
		string s1 = Path.GetFullPath( file  );
		string s2 = Path.GetFullPath( fPath );
		
		s1 = s1.Substring( 0, s1.Length - n.Length );
		
		if( s1 == s2 ) return true;
			
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

	public int      Ftype  { set{ _fType  = value; } get{ return _fTemp;  }}
	public string[] Ftypes { set{ _fTypes = value; } get{ return _fTypes; }}
}