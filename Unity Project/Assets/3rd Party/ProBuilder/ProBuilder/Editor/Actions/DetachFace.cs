using UnityEngine;
using UnityEditor;
using System.Collections;

public class DetachFace : Editor
{
	[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Geometry/Detach Face(s)", false, 100 + 3)]
	public static void Detach()
	{
		pb_Object[] pbSelection = pbUtil.GetComponents<pb_Object>(Selection.transforms);
		#if UNITY_4_3_0 || UNITY_4_3_1 || UNITY_4_3_2
		Undo.RecordObjects(pbSelection as Object[], "Detach Face(s)");
		#else
		Undo.RegisterUndo(pbSelection as Object[], "Detach Face(s)");
		#endif
		foreach(pb_Object pb in pbSelection)
			foreach(pb_Face face in pb.selected_faces)
				pb.DetachFace(face);
	}
}
