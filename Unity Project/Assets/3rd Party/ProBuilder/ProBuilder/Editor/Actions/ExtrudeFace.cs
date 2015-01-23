#if UNITY_4_3 || UNITY_4_3_0 || UNITY_4_3_1
#define UNITY_4_3
#elif UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
#define UNITY_4
#elif UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
#define UNITY_3
#endif

using UnityEngine;
using UnityEditor;
using System.Collections;
using ProBuilder2.Common;
using ProBuilder2.MeshOperations;
using ProBuilder2.EditorEnum;

namespace ProBuilder2.Actions 
{
	public class ExtrudeFace : Editor
	{
		const int EXTRUDE = 100;
		
		[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Geometry/Extrude %#e", false, EXTRUDE + 1)]
		public static void ExtrudeNoTranslation()
		{
			PerformExtrusion(0f);
		}

		[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Geometry/Extrude with Translation %e", false, EXTRUDE)]
		public static void Extrude()
		{
			PerformExtrusion(.25f);
		}

		private static void PerformExtrusion(float dist)
		{
			SelectMode mode = pb_Editor.instance.GetSelectionMode();

			pb_Object[] pbs = pbUtil.GetComponents<pb_Object>(Selection.transforms);

			#if !UNITY_4_3
			Undo.RegisterUndo(pbUtil.GetComponents<pb_Object>(Selection.transforms), "extrude selected.");
			#else
			Undo.RecordObjects(pbUtil.GetComponents<pb_Object>(Selection.transforms), "extrude selected.");
			#endif

			int extrudedFaceCount = 0;
			foreach(pb_Object pb in pbs)
			{
				switch(mode)
				{
					case SelectMode.Face:
						if(pb.selected_faces.Length < 1)
							continue;
						
						extrudedFaceCount += pb.selected_faces.Length;
						pb.Extrude(pb.selected_faces, dist);
						break;

					case SelectMode.Edge:
						
						if(pb.selected_edges.Length < 1)
							continue;
						
						pb_Edge[] newEdges = pb.Extrude(pb.selected_edges, dist, pb_Preferences_Internal.GetBool(pb_Constant.pbPerimeterEdgeExtrusionOnly));

						if(newEdges != null)
						{
							extrudedFaceCount += pb.selected_edges.Length;
							pb.selected_edges = newEdges; 
							pb.selected_triangles = pb.SharedTrianglesWithTriangles( pb.selected_edges.ToIntArray() );
						}

						break;
				}
	
				pb.GenerateUV2(true);
			}

			if(extrudedFaceCount > 0)
			{
				string val = "";
				if(mode == SelectMode.Edge)
					val = (extrudedFaceCount > 1 ? extrudedFaceCount + " Edges" : "Edge");
				else
					val = (extrudedFaceCount > 1 ? extrudedFaceCount + " Faces" : "Face");
				pb_Editor_Utility.ShowNotification("Extrude " + val, "Extrudes the selected faces / edges.");
			}

			if(pb_Editor.instance)
				pb_Editor.instance.UpdateSelection();
		}
	}
}
