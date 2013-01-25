using UnityEngine;
using UnityEditor;
using System.Collections;

using klock;

public class TestWindow :EditorWindow
{
    
	private static TestWindow instance;
	
	[MenuItem("Window/*TEST/Resource Bitmap Data")]
	public static void Init ()
	{

		if (instance != null) {
			instance.Show ();
			return;
		} else {
			instance = (TestWindow)EditorWindow.GetWindow (typeof(TestWindow), false, "Test Window");
			instance.wantsMouseMove = true;
			instance.Show ();
			instance.position = new Rect (50, 50, 400, 30);
		}
	}

	private void OnInspectorUpdate ()
	{
		Repaint ();
	}

	private void OnGUI ()
	{
		DRAW ();
	}

	private void DRAW ()
	{

		// Check if bitmap resource data - in the dll package is valid
		
		if (kResource.GetBinaryTexture (kResource.CLOSE, 36, 28) != null) {
			
			GUI.DrawTexture (new Rect (50, 50, 23, 23), kResource.GetBinaryTexture (kResource.CLOSE, 36, 28)); 
			GUI.DrawTexture (new Rect (75, 50, 23, 23), kResource.GetBinaryTexture (kResource.OPEN, 36, 28)); 		
		
		} else { 
			
			// if the references to the assets not available - set kResource to null. To refresh resource data.
			// Happens : on editor state change ( from playmode in editormode ) 
			
			kResource.Dispose ();
		}
	}
}
