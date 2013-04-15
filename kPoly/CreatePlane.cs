using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreatePlane
{
	/** Create plane mesh in a unity gameobject.
     * 
     *  @params string - A string value for the gameobject.name
     *  @params int - The count of horizontal segments.
     *  @params int - The count of vertical segments.
     *  @params float - The horizontal size for the created mesh.
     *  @params float - The vertical size for the created mesh.
     *  @params int - The index for the normal facing.
     *  @params int - The index for the triangle winding.
     *  
     *  @return GameObject - The generated object.
     */
	public static GameObject CreateMesh 
		(
			string _meshName,
	      	int _uSegments,
	      	int _vSegments,
	      	float _width,
	      	float _height,
	      	int _faceIndex,
	      	int _windinIndex,
	      	int _pivotIndex,
		  	int _colliderIndex
		)
	{

		GameObject quad = new GameObject ();

		if (!string.IsNullOrEmpty (_meshName))
			quad.name = _meshName;
		else
			quad.name = "kPoly";

		quad.transform.position = Vector3.zero;

		((MeshFilter)quad.AddComponent<MeshFilter>()).sharedMesh = 
		klock.geometry.kPoly.Create_Plane(	quad.name,
										_uSegments,_vSegments,
										_width,_height,
										_faceIndex,
										_windinIndex,
										_pivotIndex/*,
										_colliderIndex*/
									);
		quad.AddComponent<MeshRenderer> ();
		if (!string.IsNullOrEmpty (_meshName))
			quad.name = _meshName;
		else
			quad.name = "kPoly";
		return quad;
	}
	/** Returns a vector2 containing the new position for the transform point of the generated mesh.*/
	private static Vector2 PivotVector
            (int _pivotIndex,
            float _width,
            float _height)
	{
		Vector2 p = Vector2.zero;
		switch (_pivotIndex) {
		case 0://TextAnchor.UpperLeft:
			p = new Vector2 (-_width, _height);
			break;
		case 1://TextAnchor.UpperCenter:
			p = new Vector2 (0, _height);
			break;
		case 2://TextAnchor.UpperRight:
			p = new Vector2 (_width, _height);
			break;
		case 3://TextAnchor.MiddleLeft:
			p = new Vector2 (-_width, 0);
			break;
		case 4://TextAnchor.MiddleCenter:
			p = Vector2.zero;
			break;
		case 5://TextAnchor.MiddleRight:
			p = new Vector2 (_width, 0);
			break;
		case 6://TextAnchor.LowerLeft:
			p = new Vector2 (-_width, -_height);
			break;
		case 7://TextAnchor.LowerCenter:
			p = new Vector2 (0, -_height);
			break;
		case 8://TextAnchor.LowerRight:
			p = new Vector2 (_width, -_height);
			break;
		}
		return p;

	}
	/** Add a collider object to the generated gameobject.
	 *  @params GameObject quad - The target gameobject.
	 * 	@params Mesh m - The sharedMesh property for a MeshCollider componete.
	 */
	public static void AddCollider (int colIndex, GameObject quad, Mesh m)
	{
		if (colIndex > 0) {
				
			switch (colIndex) {
			case 1: // mesh 
				MeshCollider mc = quad.AddComponent<MeshCollider> ();
				mc.sharedMesh = m;
				break;
			case 2: // box
				quad.AddComponent<BoxCollider> ();
				break;
			}				
		}
	}
}

