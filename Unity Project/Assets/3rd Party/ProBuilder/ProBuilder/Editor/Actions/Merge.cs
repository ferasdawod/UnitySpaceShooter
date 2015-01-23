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

namespace ProBuilder2.Actions
{
	public class Merge : Editor
	{
		const int VERTEX_OPERATIONS = 300;

		[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Geometry/Collapse Selected Vertices &c", false, VERTEX_OPERATIONS + 0)]
		/**
		 *	Collapses all selected vertices to a single central vertex.
		 */
		public static void CollapseVertices()
		{

			bool success = false;
			foreach(pb_Object pb in pbUtil.GetComponents<pb_Object>(Selection.transforms))
			{
				if(pb.selected_triangles.Length > 1)
				{
					#if UNITY_4_3
					Undo.RecordObjects( new Object[2] { pb.msh, pb } , "Collapse Vertices");
					#else
					Undo.RegisterUndo( new Object[2] { pb.msh, pb } as Object[], "Collapse Vertices" );
					#endif	


					success = pb.MergeVertices(pb.selected_triangles);
				}
			}

			if(success)
				pb_Editor_Utility.ShowNotification("Collapse Vertices", "");

			pb_Editor.instance.UpdateSelection();
		}

		
		/**
		 *	For each vertex within epsilon distance, collapse.
		 */
		[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Geometry/Weld Selected Vertices &v", false, VERTEX_OPERATIONS + 1)]
		public static void WeldVertices()
		{

			bool success = false;
			foreach(pb_Object pb in pbUtil.GetComponents<pb_Object>(Selection.transforms))
			{
				if(pb.selected_triangles.Length > 1)
				{
					#if UNITY_4_3
					Undo.RecordObjects( new Object[2] { pb.msh, pb } , "Weld Vertices");
					#else
					Undo.RegisterUndo( new Object[2] { pb.msh, pb } as Object[], "Weld Vertices" );
					#endif	

					success = pb.WeldVertices(pb.selected_triangles, Mathf.Epsilon);//Mathf.Epsilon);
				}
			}

			if(success)
				pb_Editor_Utility.ShowNotification("Weld Vertices", "");

			pb_Editor.instance.UpdateSelection();
		}
	
		[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Geometry/Split Selected Vertices &x", false, VERTEX_OPERATIONS + 2)]
		public static void SplitVertices()
		{
			int splitCount = 0;
			foreach(pb_Object pb in pbUtil.GetComponents<pb_Object>(Selection.transforms))
			{
				#if UNITY_4_3
				Undo.RecordObjects( new Object[2] { pb.msh, pb } , "Split Vertices");
				#else
				Undo.RegisterUndo( new Object[2] { pb.msh, pb } as Object[], "Split Vertices" );
				#endif
			
				splitCount += pb.selected_triangles.Length;
				pb.SplitVertices(pb.selected_triangles);
			}

			pb_Editor_Utility.ShowNotification("Split " + splitCount + " Vertices", "");

			pb_Editor.instance.UpdateSelection();
		}
	}
}
#endif