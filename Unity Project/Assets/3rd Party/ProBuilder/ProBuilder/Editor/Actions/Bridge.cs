#if !PROTOTYPE

#if UNITY_4_3 || UNITY_4_3_0 || UNITY_4_3_1 || UNITY_4_3_2 || UNITY_4_3_3 || UNITY_4_3_4 || UNITY_4_3_5
#define UNITY_4_3
#elif UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
#define UNITY_4
#elif UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
#define UNITY_3
#endif

using UnityEngine;
using UnityEditor;
using System.Collections;
using ProBuilder2.MeshOperations;

public class Bridge : Editor
{
	[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Geometry/Bridge Edges &b", false, 200)]
	public static void BridgeEdges()
	{
		#if !UNITY_4_3
		Undo.RegisterUndo(EditorUtility.CollectDeepHierarchy(Selection.transforms as Object[]), "Bridge selected edges.");	
		#else
		Undo.RecordObjects(EditorUtility.CollectDeepHierarchy(Selection.transforms as Object[]), "Bridge selected edges.");	
		#endif

		bool success = false;
		bool limitToPerimeterEdges = pb_Preferences_Internal.GetBool(pb_Constant.pbPerimeterEdgeBridgeOnly);
		foreach(pb_Object pb in pbUtil.GetComponents<pb_Object>(Selection.transforms))
		{
			if(pb.selected_edges.Length == 2)
				if(pb.Bridge(pb.selected_edges[0], pb.selected_edges[1], limitToPerimeterEdges))
					success = true;
		}
		if(success)
			pb_Editor_Utility.ShowNotification("Bridge Edges", "");

	}
}
#endif